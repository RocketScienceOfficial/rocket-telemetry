using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataRecipient : MonoBehaviour
{
    public virtual void OnSetData(RecipentData data) { }
    public virtual void OnSetRawData(RawRecipentData data) { }
    public virtual void OnSetCommand(string command) { }
}

public struct RecipentData
{
    public float positionX;
    public float positionY;
    public float positionZ;
    public float roll;
    public float pitch;
    public float yaw;
    public float latitude;
    public float longitude;
    public float altitude;
    public float velocity;
}

public struct RawRecipentData
{
    public float accelX_1;
    public float accelX_2;
    public float accelX_3;
    public float accelY_1;
    public float accelY_2;
    public float accelY_3;
    public float accelZ_1;
    public float accelZ_2;
    public float accelZ_3;
    public float gyrolX_1;
    public float gyrolX_2;
    public float gyrolY_1;
    public float gyrolY_2;
    public float gyrolZ_1;
    public float gyrolZ_2;
    public float magX_1;
    public float magY_1;
    public float magZ_1;
    public float lat;
    public float lon;
    public float alt;
    public float pressure;
    public float temperature;
}