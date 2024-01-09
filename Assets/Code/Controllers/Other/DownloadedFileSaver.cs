using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

public class DownloadedFileSaver : MonoBehaviour, IDownloadDataRecipient, ICommandRecipient
{
    private const string FILES_DIR = "Flight/";

    private readonly StringBuilder _csvBuilder = new();

    private StreamWriter _writer;
    private string _fullPath;

    private void OnApplicationQuit()
    {
        CloseFile();
    }

    private void CreateFile()
    {
        var path = FILES_DIR + $"FlightLog_{DateTime.Now:yyyy-dd-M--HH-mm-ss}.csv";

        IOUtils.EnsureDirectoryExists(path);

        _fullPath = Path.GetFullPath(path);
        _writer = new StreamWriter(path);
    }

    private void CloseFile()
    {
        if (_writer != null)
        {
            _writer.Close();
            _writer = null;

            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false,
                FileName = "cmd.exe",
                Arguments = $"/C cd Utils && python reportGen.py {_fullPath}",
            };

            var process = Process.Start(startInfo);

            process.WaitForExit();

            _fullPath = null;
        }
    }

    private void WriteFileValue(float value)
    {
        _csvBuilder.Append(value.ToString().Replace(',', '.'));
        _csvBuilder.Append(",");
    }

    private void WriteFileValue(int value)
    {
        _csvBuilder.Append(value.ToString());
        _csvBuilder.Append(",");
    }

    public void OnSetData(RecipientData data)
    {
        if (_writer != null)
        {
            WriteFileValue(data.positionX);
            WriteFileValue(data.positionY);
            WriteFileValue(data.positionZ);
            WriteFileValue(data.roll);
            WriteFileValue(data.pitch);
            WriteFileValue(data.yaw);
            WriteFileValue(data.latitude);
            WriteFileValue(data.longitude);
            WriteFileValue(data.altitude);
            WriteFileValue(data.acceleration);
            WriteFileValue(data.velocity);
            WriteFileValue(data.batteryVoltage);
            WriteFileValue(data.batteryPercentage);
            WriteFileValue(data.pressure);
            WriteFileValue(data.temperature);
            WriteFileValue(data.signalStrength);
            WriteFileValue(data.packetLoss);

            _writer.WriteLine(_csvBuilder.ToString());
            _csvBuilder.Clear();
        }
    }

    public void OnSetCommand(string cmd)
    {
        if (cmd == "data-start")
        {
            CreateFile();
        }
        else if (cmd == "data-end")
        {
            CloseFile();
        }
    }
}