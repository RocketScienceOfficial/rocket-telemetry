using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// REF: https://openweathermap.org/current

public class WindPanelController : MonoBehaviour, IDataRecipient
{
    private const float HELLMAN_EXPONENT = 0.2f;
    private const string API_KEY_SAVE_KEY = "WindApiKey";

    [Header("General")]
    [SerializeField] private GameObject m_WindPanel_5000m;
    [SerializeField] private GameObject m_WindPanel_2000m;
    [SerializeField] private GameObject m_WindPanel_1000m;
    [SerializeField] private GameObject m_WindPanel_500m;
    [SerializeField] private GameObject m_WindPanel_100m;
    [SerializeField] private GameObject m_WindPanel_50m;
    [SerializeField] private GameObject m_WindPanel_10m;
    [SerializeField] private TextMeshProUGUI m_DirectionText;
    [SerializeField] private TextMeshProUGUI m_DirectionInDegText;
    [SerializeField] private Gradient m_WindColorGradient;
    [SerializeField] private float m_MaxWindSpeed;

    [Header("API Key")]
    [SerializeField] private GameObject m_ApiKeyPanel;
    [SerializeField] private TMP_InputField m_ApiKeyInputField;
    [SerializeField] private Button m_ApiKeySubmitButton;

    private string _apiKey;
    private bool _updated;

    private void Start()
    {
        LoadApiKey();
    }

    public void OnSetData(RecipientData data)
    {
        if (!string.IsNullOrEmpty(_apiKey) && data.latitude != 0 && !_updated)
        {
            StartCoroutine(GetWindData(data.latitude, data.longitude));

            _updated = true;
        }
    }

    private void LoadApiKey()
    {
        var key = PlayerPrefs.GetString(API_KEY_SAVE_KEY);

        if (!string.IsNullOrEmpty(key))
        {
            m_ApiKeyPanel.SetActive(false);

            _apiKey = key;

            UpdateUI(0, -1);
        }
        else
        {
            m_ApiKeyPanel.SetActive(true);

            m_ApiKeySubmitButton.onClick.RemoveAllListeners();
            m_ApiKeySubmitButton.onClick.AddListener(() =>
            {
                PlayerPrefs.SetString(API_KEY_SAVE_KEY, m_ApiKeyInputField.text);

                LoadApiKey();
            });
        }
    }

    private IEnumerator GetWindData(double lat, double lon)
    {
        using var req = UnityWebRequest.Get($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={_apiKey}");

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(req.error);

            _updated = false;

            LoadApiKey();
        }
        else
        {
            var response = JsonConvert.DeserializeObject<OpenWeatherMapResponse>(req.downloadHandler.text);

            UpdateUI(response.wind.deg, response.wind.speed);

            print("Succesfully updated wind data!");
        }
    }

    private void UpdateUI(float windDeg, float windSpeed)
    {
        UpdateWindPanel(windSpeed, m_WindPanel_5000m, 5000);
        UpdateWindPanel(windSpeed, m_WindPanel_2000m, 2000);
        UpdateWindPanel(windSpeed, m_WindPanel_1000m, 1000);
        UpdateWindPanel(windSpeed, m_WindPanel_500m, 500);
        UpdateWindPanel(windSpeed, m_WindPanel_100m, 100);
        UpdateWindPanel(windSpeed, m_WindPanel_50m, 50);
        UpdateWindPanel(windSpeed, m_WindPanel_10m, 10);

        m_DirectionText.SetText(DegreesToWindDirection(windDeg));
        m_DirectionInDegText.SetText(windDeg + "°");
    }

    private void UpdateWindPanel(float windSpeed, GameObject panel, float height)
    {
        if (windSpeed != -1)
        {
            var speed = CalculateWindGradient(windSpeed, height);
            var color = m_WindColorGradient.Evaluate(speed / m_MaxWindSpeed);

            panel.transform.Find("Speed Text").GetComponent<TextMeshProUGUI>().SetText(MathUtils.NumberOneDecimalPlace(speed));
            panel.transform.Find("Speed Text").GetComponent<TextMeshProUGUI>().color = color;
        }
        else
        {
            panel.transform.Find("Speed Text").GetComponent<TextMeshProUGUI>().SetText("NaN");
        }
    }

    private float CalculateWindGradient(float baseSpeed, float height)
    {
        return baseSpeed * Mathf.Pow(height / 10f, HELLMAN_EXPONENT);
    }

    private string DegreesToWindDirection(float deg)
    {
        const float RES = 360.0f / 8 / 2;

        if (deg > 360 - RES || deg <= 0 + RES)
        {
            return "N";
        }
        else if (deg > 45 - RES && deg <= 45 + RES)
        {
            return "NE";
        }
        else if (deg > 90 - RES && deg <= 90 + RES)
        {
            return "E";
        }
        else if (deg > 135 - RES && deg <= 135 + RES)
        {
            return "SE";
        }
        else if (deg > 180 - RES && deg <= 180 + RES)
        {
            return "S";
        }
        else if (deg > 225 - RES && deg <= 225 + RES)
        {
            return "SW";
        }
        else if (deg > 270 - RES && deg <= 270 + RES)
        {
            return "W";
        }
        else if (deg > 315 - RES && deg <= 315 + RES)
        {
            return "NW";
        }
        else
        {
            return "NaN";
        }
    }


    [Serializable]
    struct OpenWeatherMapResponse
    {
        public OpenWeatherMapCoord coord;
        public OpenWeatherMapWeather[] weather;
        public string @base;
        public OpenWeatherMapMain main;
        public int visibility;
        public OpenWeatherMapWind wind;
        public OpenWeatherMapRain rain;
        public OpenWeatherMapClouds clouds;
        public int dt;
        public OpenWeatherMapSys sys;
        public int timezone;
        public int id;
        public string name;
        public int cod;
    }

    [Serializable]
    struct OpenWeatherMapCoord
    {
        public float lat;
        public float lon;
    }

    [Serializable]
    struct OpenWeatherMapWeather
    {
        public int id;
        public string main;
        public string description;
        public string icon;
    }

    [Serializable]
    struct OpenWeatherMapMain
    {
        public float temp;
        public float feels_like;
        public float temp_min;
        public float temp_max;
        public int pressure;
        public int humidity;
        public int sea_level;
        public int grnd_level;
    }

    [Serializable]
    struct OpenWeatherMapWind
    {
        public float speed;
        public int deg;
        public float gust;
    }

    [Serializable]
    struct OpenWeatherMapRain
    {
        [JsonProperty("1h")] public float h1;
    }

    [Serializable]
    struct OpenWeatherMapClouds
    {
        public int all;
    }

    [Serializable]
    struct OpenWeatherMapSys
    {
        public int type;
        public int id;
        public string country;
        public int sunrise;
        public int subset;
    }
}