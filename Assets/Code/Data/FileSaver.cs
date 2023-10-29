using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

public class FileSaver : DataRecipient
{
    private const string FILES_DIR = "Flight/";

    private readonly StringBuilder _csvBuilder = new();

    private StreamWriter _writer;
    private string _fullPath;

    private void Start()
    {
        AppModeController.OnSetMode += (sender, args) =>
        {
            if (AppModeController.CurrentMode == AppMode.DataVisualization)
            {
                return;
            }

            AppModeController.CurrentDataSupply.OnConnected += (sender, args) =>
            {
                CreateFile();
            };

            AppModeController.CurrentDataSupply.OnDisconnected += (sender, args) =>
            {
                CloseFile();
            };
        };
    }

    private void OnApplicationQuit()
    {
        CloseFile();
    }

    private void CreateFile()
    {
        var path = FILES_DIR + $"FlightLog_{DateTime.Now:yyyy-dd-M--HH-mm-ss}.csv";

        _fullPath = Path.GetFullPath(path);

        IOUtils.EnsureDirectoryExists(path);

        _writer = new StreamWriter(path);
    }

    private void CloseFile()
    {
        if (_writer != null)
        {
            _writer.Close();
            _writer = null;

            //var startInfo = new ProcessStartInfo
            //{
            //    WindowStyle = ProcessWindowStyle.Hidden,
            //    CreateNoWindow = true,
            //    UseShellExecute = false,
            //    FileName = "cmd.exe",
            //    Arguments = $"/C cd Utils && python reportGen.py {_fullPath}",
            //};

            //var process = Process.Start(startInfo);

            //process.WaitForExit();

            _fullPath = null;
        }
    }

    private void WriteFileValue(float value)
    {
        _csvBuilder.Append(value.ToString().Replace(',', '.'));
        _csvBuilder.Append(",");
    }

    public override void OnSetData(RecipentData data)
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
            WriteFileValue(data.velocity);

            _writer.WriteLine(_csvBuilder.ToString());
            _csvBuilder.Clear();
        }
    }

    public override void OnSetRawData(RawRecipentData data)
    {
        if (_writer != null)
        {
            WriteFileValue(data.accelX_1);
            WriteFileValue(data.accelX_2);
            WriteFileValue(data.accelX_3);
            WriteFileValue(data.accelY_1);
            WriteFileValue(data.accelY_2);
            WriteFileValue(data.accelY_3);
            WriteFileValue(data.accelZ_1);
            WriteFileValue(data.accelZ_2);
            WriteFileValue(data.accelZ_3);
            WriteFileValue(data.gyrolX_1);
            WriteFileValue(data.gyrolX_2);
            WriteFileValue(data.gyrolY_1);
            WriteFileValue(data.gyrolY_2);
            WriteFileValue(data.gyrolZ_1);
            WriteFileValue(data.gyrolZ_2);
            WriteFileValue(data.magX_1);
            WriteFileValue(data.magY_1);
            WriteFileValue(data.magZ_1);
            WriteFileValue(data.lat);
            WriteFileValue(data.lon);
            WriteFileValue(data.alt);
            WriteFileValue(data.pressure);
            WriteFileValue(data.temperature);

            _writer.WriteLine(_csvBuilder.ToString());
            _csvBuilder.Clear();
        }
    }
}