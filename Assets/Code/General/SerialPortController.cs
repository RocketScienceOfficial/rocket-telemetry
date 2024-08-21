using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Linq;
using UnityEngine;

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
    private bool _openPort;
    private readonly object _openPortLock = new();
    private bool _disconnectPort;
    private readonly object _disconnectPortLock = new();
    private SerialPort _currentSerialPort;
    private Thread _serialOpenThread;
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
                            qw = float.Parse(data[0]),
                            qx = float.Parse(data[1]),
                            qy = float.Parse(data[2]),
                            qz = float.Parse(data[3]),
                            velocity = float.Parse(data[4]),
                            batteryVoltage = float.Parse(data[5]),
                            batteryPercentage = int.Parse(data[6]),
                            latitude = double.Parse(data[7]),
                            longitude = double.Parse(data[8]),
                            altitude = int.Parse(data[9]),
                            state = int.Parse(data[10]),
                            controlFlags = int.Parse(data[11]),
                            signalStrength = int.Parse(data[12]),
                            packetLoss = int.Parse(data[13]),
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

        lock (_openPortLock)
        {
            if (_openPort)
            {
                EndConnect();

                _openPort = false;
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
        _serialOpenThread = new Thread(() =>
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

                lock (_openPortLock)
                {
                    _openPort = true;
                }
            }
            catch (Exception ex)
            {
                print("Could not find serial port: " + ex);

                BeginDisconnect();
            }
        });

        _serialOpenThread.Start();
    }

    private void EndConnect()
    {
        OnConnected?.Invoke(this, EventArgs.Empty);

        print("COM Port Connected!");
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
        _serialReadThread?.Join();
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
                BeginDisconnect();

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

public enum RecipientDataControlFlags
{
    Armed = 1 << 0,
    VBat = 1 << 1,
    V5 = 1 << 2,
    V3V3 = 1 << 3,
    Calibration = 1 << 4,
    GPS = 1 << 5,
    SelfTest = 1 << 6,
}

public struct RecipientData
{
    public float qw;
    public float qx;
    public float qy;
    public float qz;
    public float velocity;
    public float batteryVoltage;
    public int batteryPercentage;
    public double latitude;
    public double longitude;
    public int altitude;
    public int state;
    public int controlFlags;
    public int signalStrength;
    public int packetLoss;
}