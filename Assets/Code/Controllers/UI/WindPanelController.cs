using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WindPanelController : MonoBehaviour
{
    private const float HELLMAN_EXPONENT = 0.2f;

    [SerializeField] private Button m_OpenPanelButton;
    [SerializeField] private GameObject m_Panel;
    [SerializeField] private GameObject m_WindPanel_5000m;
    [SerializeField] private GameObject m_WindPanel_2000m;
    [SerializeField] private GameObject m_WindPanel_1000m;
    [SerializeField] private GameObject m_WindPanel_100m;
    [SerializeField] private GameObject m_WindPanel_10m;
    [SerializeField] private TextMeshProUGUI m_DirectionText;
    [SerializeField] private Gradient m_WindColorGradient;
    [SerializeField] private float m_MaxWindSpeed;

    private string _ip;
    private float _lat;
    private float _lon;
    private float _windSpeed;
    private float _windDeg;

    private void Start()
    {
        Refresh();

        m_OpenPanelButton.onClick.AddListener(() =>
        {
            m_Panel.SetActive(!m_Panel.activeSelf);

            if (m_Panel.activeSelf)
            {
                Refresh();
            }
        });
    }

    private void Refresh()
    {
        StartCoroutine(Fetch());
    }

    private void UpdateUI()
    {
        UpdateWindPanel(m_WindPanel_5000m, 5000);
        UpdateWindPanel(m_WindPanel_2000m, 2000);
        UpdateWindPanel(m_WindPanel_1000m, 1000);
        UpdateWindPanel(m_WindPanel_100m, 100);
        UpdateWindPanel(m_WindPanel_10m, 10);

        m_DirectionText.SetText(_windDeg + "°");
    }

    private void UpdateWindPanel(GameObject panel, float height)
    {
        var speed = CalculateWindGradient(_windSpeed, height);
        var color = m_WindColorGradient.Evaluate(speed / m_MaxWindSpeed);

        panel.transform.Find("Speed Text").GetComponent<TextMeshProUGUI>().SetText(MathUtils.NumberOneDecimalPlace(speed) + "m/s");
        panel.transform.Find("Speed Text").GetComponent<TextMeshProUGUI>().color = color;
    }

    private float CalculateWindGradient(float baseSpeed, float height)
    {
        return baseSpeed * Mathf.Pow(height / 10f, HELLMAN_EXPONENT);
    }


    private IEnumerator Fetch()
    {
        yield return GetIP();
        yield return GetLocation();
        yield return GetWindData();

        UpdateUI();
    }

    private IEnumerator GetIP()
    {
        using var req = UnityWebRequest.Get("https://api.myip.com");

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(req.error);
        }
        else
        {
            var response = JsonUtility.FromJson<IPResponse>(req.downloadHandler.text);

            _ip = response.ip;
        }
    }

    private IEnumerator GetLocation()
    {
        using var req = UnityWebRequest.Get($"http://ip-api.com/json/{_ip}");

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(req.error);
        }
        else
        {
            var response = JsonUtility.FromJson<LocationResponse>(req.downloadHandler.text);

            _lat = response.lat;
            _lon = response.lon;
        }
    }

    private IEnumerator GetWindData()
    {
        if (!string.IsNullOrEmpty(ConfigLoader.CurrentData.openWeatherMapApiKey))
        {
            using var req = UnityWebRequest.Get($"https://api.openweathermap.org/data/2.5/weather?lat={_lat}&lon={_lon}&appid={ConfigLoader.CurrentData.openWeatherMapApiKey}");

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(req.error);
            }
            else
            {
                var response = JsonConvert.DeserializeObject<OpenWeatherMapResponse>(req.downloadHandler.text);

                _windSpeed = response.wind.speed;
                _windDeg = response.wind.deg;
            }
        }
    }


    [Serializable]
    struct IPResponse
    {
        public string ip;
        public string country;
        public string cc;
    }

    [Serializable]
    struct LocationResponse
    {
        public string query;
        public string status;
        public string continent;
        public string continentCode;
        public string country;
        public string countryCode;
        public string region;
        public string regionName;
        public string city;
        public string district;
        public string zip;
        public float lat;
        public float lon;
        public string timezone;
        public int offset;
        public string currency;
        public string isp;
        public string org;
        public string @as;
        public string asname;
        public string mobile;
        public string proxy;
        public string hosting;
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