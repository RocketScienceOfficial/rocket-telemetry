using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.IO;
using UnityEngine;
using System.Threading;

using Debug = UnityEngine.Debug;

public class SerialPortController : DataSupplyBase
{
    public static SerialPortController Instance { get; private set; }

    private readonly Queue<string> _serialReadQueue = new();
    private readonly object _serialReadQueueLock = new();
    private bool _closePort;
    private readonly object _closePortLock = new();
    private SerialPort _currentSerialPort;
    private Thread _serialReadThread;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        lock (_serialReadQueueLock)
        {
            if (_serialReadQueue.Count > 0)
            {
                CallOnSupplyReadData(_serialReadQueue.Dequeue());
            }
        }

        lock (_closePortLock)
        {
            if (_closePort)
            {
                Disconnect();

                _closePort = false;
            }
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

            CallOnConnected();

            Debug.Log("COM Port Connected!");
        }
        catch (Exception ex)
        {
            Debug.Log("Could not find serial port: " + ex);

            _currentSerialPort = null;
        }
    }

    private void Disconnect()
    {
        if (IsConnected())
        {
            _currentSerialPort.Close();
            _currentSerialPort = null;

            _serialReadThread.Join();
            _serialReadThread = null;

            CallOnDisconnected();
        }
    }

    private void SerialReadThread()
    {
        while (IsConnected())
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
        if (IsConnected())
        {
            _currentSerialPort.WriteLine(data + "\r");
        }
    }

    public IEnumerable<string> ListSerialPorts()
    {
        return SerialPort.GetPortNames();
    }

    public override bool IsConnected() => _currentSerialPort != null;
    public override string GetPath() => _currentSerialPort?.PortName;
}