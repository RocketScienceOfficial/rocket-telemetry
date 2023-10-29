using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataSupplyBase : MonoBehaviour
{
    public event EventHandler OnConnected;
    public event EventHandler OnDisconnected;
    public event EventHandler<OnSupplyReadDataEventArgs> OnSupplyReadData;

    protected void CallOnConnected() => OnConnected?.Invoke(this, EventArgs.Empty);
    protected void CallOnDisconnected() => OnDisconnected?.Invoke(this, EventArgs.Empty);
    protected void CallOnSupplyReadData(string data) => OnSupplyReadData?.Invoke(this, new OnSupplyReadDataEventArgs { Data = data });

    public abstract bool IsConnected();
    public abstract string GetPath();
}

public class OnSupplyReadDataEventArgs : EventArgs
{
    public string Data { get; set; }
}