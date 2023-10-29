using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AppMode { Telemetry, DataDownload, DataVisualization, }

public static class AppModeController
{
    public static event EventHandler OnSetMode;
    public static bool IsDebugMode { get; set; }
    public static AppMode CurrentMode { get; private set; }
    public static DataSupplyBase CurrentDataSupply { get; private set; }

    public static void SetMode(AppMode mode)
    {
        CurrentMode = mode;

        switch (mode)
        {
            case AppMode.Telemetry:
                CurrentDataSupply = UnityEngine.Object.FindObjectOfType<SerialPortController>();
                break;
            case AppMode.DataDownload:
                CurrentDataSupply = UnityEngine.Object.FindObjectOfType<SerialPortController>();
                break;
            case AppMode.DataVisualization:
                CurrentDataSupply = UnityEngine.Object.FindObjectOfType<FileReader>();
                break;
            default:
                return;
        }

        OnSetMode?.Invoke(null, EventArgs.Empty);

        DataProvider.SetDataSupply(CurrentDataSupply);
    }
}