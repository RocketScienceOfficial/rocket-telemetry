using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class DataProvider : MonoBehaviour
{
    private static IDataRecipient[] _dataRecipients;
    private static ICommandRecipient[] _commandsRecipients;

    public static void SetRecipientType<T>() where T : IDataRecipient
    {
        _dataRecipients = FindObjectsOfType<MonoBehaviour>().OfType<T>().Select(d => (IDataRecipient)d).ToArray();
        _commandsRecipients = FindObjectsOfType<MonoBehaviour>().OfType<ICommandRecipient>().ToArray();
    }

    public static void SetDataSupply(DataSupplyBase dataSupply)
    {
        dataSupply.OnSupplyReadData += (sender, args) =>
        {
            try
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

                    var recipentData = new RecipientData
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
                        speed = float.Parse(data[9]),
                        batteryVoltage = float.Parse(data[10]),
                        batteryPercentage = float.Parse(data[11]),
                        pressure = float.Parse(data[12]),
                        temperature = float.Parse(data[13]),
                        signalStrength = int.Parse(data[14]),
                        packetLoss = int.Parse(data[15]),
                    };

                    SetData(recipentData);
                }
                else if (msg.StartsWith("CMD:"))
                {
                    msg = msg.Remove(0, 4);

                    SetCommand(msg);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);

                AlertManager.Alert("Unexpected error occured while parsing data frame");
            }
        };
    }

    private static void SetData(RecipientData data)
    {
        foreach (var item in _dataRecipients)
        {
            item.OnSetData(data);
        }
    }

    private static void SetCommand(string cmd)
    {
        foreach (var item in _commandsRecipients)
        {
            item.OnSetCommand(cmd);
        }
    }
}