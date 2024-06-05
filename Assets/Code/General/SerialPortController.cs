using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.IO;
using UnityEngine;
using System.Threading;
using System.Linq;

public class SerialPortController : MonoBehaviour
{
    public static SerialPortController Instance { get; private set; }

    public event EventHandler OnConnected;
    public event EventHandler OnDisconnected;

    public bool IsConnected => _currentSerialPort != null;
    public string Path => _currentSerialPort?.PortName;

    private IDataRecipient[] _dataRecipients;
    private readonly Queue<string> _serialReadQueue = new();
    private readonly object _serialReadQueueLock = new();
    private bool _closePort;
    private readonly object _closePortLock = new();
    private bool _disconnectPort;
    private readonly object _disconnectPortLock = new();
    private SerialPort _currentSerialPort;
    private Thread _serialReadThread;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _dataRecipients = FindObjectsOfType<MonoBehaviour>().OfType<IDataRecipient>().ToArray();
    }

    private void Update()
    {
        lock (_serialReadQueueLock)
        {
            if (_serialReadQueue.Count > 0)
            {
                var msg = _serialReadQueue.Dequeue();

                if (string.IsNullOrEmpty(msg))
                {
                    return;
                }

                try
                {
                    if (msg.StartsWith("/*") && msg.EndsWith("*/"))
                    {
                        msg = msg.Remove(0, 2);
                        msg = msg.Remove(msg.Length - 2, 2);

                        var data = msg.Split(',', StringSplitOptions.RemoveEmptyEntries);

                        for (var i = 0; i < data.Length; i++)
                        {
                            data[i] = data[i].Replace('.', ',');
                        }

                        var recipentData = new RecipientData
                        {
                            roll = float.Parse(data[0]),
                            pitch = float.Parse(data[1]),
                            yaw = float.Parse(data[2]),
                            velocity = float.Parse(data[3]),
                            batteryVoltage = float.Parse(data[4]),
                            batteryPercentage = int.Parse(data[5]),
                            latitude = double.Parse(data[6]),
                            longitude = double.Parse(data[7]),
                            altitude = float.Parse(data[8]),
                            signalStrength = int.Parse(data[9]),
                            packetLoss = int.Parse(data[10]),
                        };

                        foreach (var item in _dataRecipients)
                        {
                            item.OnSetData(recipentData);
                        }
                    }
                }
                catch (Exception ex)
                {
                    print(ex.Message);
                }
            }
        }

        lock (_closePortLock)
        {
            if (_closePort)
            {
                BeginDisconnect();

                _closePort = false;
            }
        }

        lock (_disconnectPortLock)
        {
            if (_disconnectPort)
            {
                EndDisconnect();

                _disconnectPort = false;
            }
        }
    }

    private void OnApplicationQuit()
    {
        BeginDisconnect();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            BeginDisconnect();
        }
    }


    public void Connect(string port)
    {
        try
        {
            _currentSerialPort = new SerialPort()
            {
                PortName = port,
                BaudRate = 115200,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                RtsEnable = true,
                DtrEnable = true,
                ReadTimeout = 1000,
                WriteTimeout = 1000,
            };

            _currentSerialPort.Open();

            _serialReadThread = new Thread(SerialReadThread);
            _serialReadThread.Start();

            OnConnected?.Invoke(this, EventArgs.Empty);

            print("COM Port Connected!");
        }
        catch (Exception ex)
        {
            print("Could not find serial port: " + ex);

            _currentSerialPort = null;
        }
    }

    private void BeginDisconnect()
    {
        if (IsConnected)
        {
            var closeThread = new Thread(() =>
            {
                _currentSerialPort.Close();
                _currentSerialPort = null;

                lock (_disconnectPortLock)
                {
                    _disconnectPort = true;
                }
            });

            closeThread.Start();
        }
    }

    private void EndDisconnect()
    {
        _serialReadThread.Join();
        _serialReadThread = null;

        OnDisconnected?.Invoke(this, EventArgs.Empty);

        print("COM Port Disconnected!");
    }

    private void SerialReadThread()
    {
        while (IsConnected)
        {
            try
            {
                var message = _currentSerialPort.ReadLine();

                if (!string.IsNullOrEmpty(message))
                {
                    lock (_serialReadQueueLock)
                    {
                        _serialReadQueue.Enqueue(message);
                    }
                }
            }
            catch (TimeoutException) { }
            catch (InvalidOperationException) { }
            catch (IOException)
            {
                lock (_closePortLock)
                {
                    _closePort = true;
                }

                break;
            }
        }
    }

    public void SerialPortWrite(string data)
    {
        if (IsConnected)
        {
            _currentSerialPort.WriteLine(data + "\r");

            print("Written to serial port: " + data);
        }
    }


    public IEnumerable<string> ListSerialPorts()
    {
        return SerialPort.GetPortNames();
    }
}

public interface IDataRecipient
{
    void OnSetData(RecipientData recipient);
}

public struct RecipientData
{
    public float roll;
    public float pitch;
    public float yaw;
    public float velocity;
    public float batteryVoltage;
    public int batteryPercentage;
    public double latitude;
    public double longitude;
    public float altitude;
    public int signalStrength;
    public int packetLoss;
}