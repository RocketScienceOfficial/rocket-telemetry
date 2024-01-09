using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConfigLoader : MonoBehaviour
{
    private const string CONFIG_FILE_PATH = "Utils/config.json";

    public static ConfigData CurrentData { get; private set; }

    private void Start()
    {
        if (!File.Exists(CONFIG_FILE_PATH))
        {
            AlertManager.Alert("Couldn't find config file");
        }
        else
        {
            var content = File.ReadAllText(CONFIG_FILE_PATH);

            try
            {
                CurrentData = JsonUtility.FromJson<ConfigData>(content);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);

                AlertManager.Alert("Unexpected error occured while parsing config file");
            }
        }
    }
}

[Serializable]
public struct ConfigData
{
    public string mapsApiKey;
    public string openWeatherMapApiKey;
}