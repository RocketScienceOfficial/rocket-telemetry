using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataProvider : MonoBehaviour
{
    private static DataRecipient[] _recipents;

    private void Start()
    {
        _recipents = FindObjectsOfType<DataRecipient>();
    }

    public static void SetDataSupply(DataSupplyBase dataSupply)
    {
        dataSupply.OnSupplyReadData += (sender, args) =>
        {
            var msg = args.Data;

            if (string.IsNullOrEmpty(msg))
            {
                return;
            }

            if (msg.StartsWith("/*") && msg.EndsWith("*/"))
            {
                msg = msg.Remove(0, 2);
                msg = msg.Remove(msg.Length - 2, 2);

                var data = msg.Split(',', StringSplitOptions.RemoveEmptyEntries);

                for (var i = 0; i < data.Length; i++)
                {
                    data[i] = data[i].Replace('.', ',');
                }

                if (!AppModeController.IsDebugMode)
                {
                    var recipentData = new RecipentData
                    {
                        positionX = float.Parse(data[0]),
                        positionY = float.Parse(data[1]),
                        positionZ = float.Parse(data[2]),
                        roll = float.Parse(data[3]),
                        pitch = float.Parse(data[4]),
                        yaw = float.Parse(data[5]),
                        latitude = float.Parse(data[6]),
                        longitude = float.Parse(data[7]),
                        altitude = float.Parse(data[8]),
                        velocity = float.Parse(data[9]),
                    };

                    SetData(recipentData);
                }
                else
                {
                    var rawRecipentData = new RawRecipentData
                    {
                        accelX_1 = float.Parse(data[0]),
                        accelX_2 = float.Parse(data[1]),
                        accelX_3 = float.Parse(data[2]),
                        accelY_1 = float.Parse(data[3]),
                        accelY_2 = float.Parse(data[4]),
                        accelY_3 = float.Parse(data[5]),
                        accelZ_1 = float.Parse(data[6]),
                        accelZ_2 = float.Parse(data[7]),
                        accelZ_3 = float.Parse(data[8]),
                        gyrolX_1 = float.Parse(data[9]),
                        gyrolX_2 = float.Parse(data[10]),
                        gyrolY_1 = float.Parse(data[11]),
                        gyrolY_2 = float.Parse(data[12]),
                        gyrolZ_1 = float.Parse(data[13]),
                        gyrolZ_2 = float.Parse(data[14]),
                        magX_1 = float.Parse(data[15]),
                        magY_1 = float.Parse(data[16]),
                        magZ_1 = float.Parse(data[17]),
                        lat = float.Parse(data[18]),
                        lon = float.Parse(data[19]),
                        alt = float.Parse(data[20]),
                        pressure = float.Parse(data[21]),
                        temperature = float.Parse(data[22]),
                    };

                    SetRawData(rawRecipentData);
                }
            }
            else if (msg.StartsWith("CMD:"))
            {
                msg = msg.Remove(0, 4);

                SetCommand(msg);
            }
        };
    }

    private static void SetData(RecipentData data)
    {
        foreach (var item in _recipents)
        {
            item.OnSetData(data);
        }
    }

    private static void SetRawData(RawRecipentData data)
    {
        foreach (var item in _recipents)
        {
            item.OnSetRawData(data);
        }
    }

    private static void SetCommand(string command)
    {
        foreach (var item in _recipents)
        {
            item.OnSetCommand(command);
        }
    }
}