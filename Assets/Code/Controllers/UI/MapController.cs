using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// REF: https://wiki.openstreetmap.org/wiki/Slippy_map_tilenames
// REF: https://operations.osmfoundation.org/policies/tiles/
// REF: https://en.wikipedia.org/wiki/Web_Mercator_projection

public class MapController : MonoBehaviour, IDataRecipient
{
    private const int MAP_ZOOM = 16;
    private const float MAP_UPDATE_RATE = 10f;
    private const int MAP_CELL_UI_SIZE = 250;

    [SerializeField] private TextMeshProUGUI m_LatitudeText;
    [SerializeField] private TextMeshProUGUI m_LongtitudeText;
    [SerializeField] private RawImage[] m_SmallTiles;
    [SerializeField] private RawImage[] m_BigTiles;
    [SerializeField] private RectTransform m_RocketMarker;
    [SerializeField] private GameObject m_NoDataSmallPanel;
    [SerializeField] private GameObject m_NoDataBigPanel;

    private float _lastUpdateTime;
    private Vector2Int _lastCenter;

    private void Start()
    {
        m_NoDataSmallPanel.SetActive(true);
        m_NoDataBigPanel.SetActive(true);

        m_LatitudeText.SetText("NaN");
        m_LongtitudeText.SetText("NaN");
    }

    public void OnSetData(RecipientData data)
    {
        m_LatitudeText.SetText($"{MathUtils.NumberSevenDecimalPlaces(data.latitude)}°");
        m_LongtitudeText.SetText($"{MathUtils.NumberSevenDecimalPlaces(data.longitude)}°");

        if (data.latitude != 0)
        {
            var pos = GetTileXY(data.latitude, data.longitude);

            SetMarkerLocation(pos, data.latitude, data.longitude);

            if ((Mathf.Abs(pos.x - _lastCenter.x) >= 2 || Mathf.Abs(pos.y - _lastCenter.y) >= 2) && (Time.time - MAP_UPDATE_RATE >= _lastUpdateTime || _lastUpdateTime == 0))
            {
                StartCoroutine(SetSmallTiles(pos));
                StartCoroutine(SetBigTiles(pos));

                _lastUpdateTime = Time.time;
            }

            _lastCenter = pos;
        }
    }

    private IEnumerator SetSmallTiles(Vector2Int center)
    {
        var i = 0;
        var textures = new Texture2D[m_SmallTiles.Length];

        for (int h = -1; h <= 1; h++)
        {
            for (int w = -1; w <= 1; w++)
            {
                var url = $"https://tile.openstreetmap.org/{MAP_ZOOM}/{center.x + w}/{center.y + h}.png";

                using var map = UnityWebRequestTexture.GetTexture(url);

                yield return map.SendWebRequest();

                if (map.result == UnityWebRequest.Result.ConnectionError || map.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Map error: " + map.error);
                }

                var content = DownloadHandlerTexture.GetContent(map);

                textures[i++] = content;
            }
        }

        for (i = 0; i < textures.Length; i++)
        {
            m_SmallTiles[i].texture = textures[i];
        }

        m_NoDataSmallPanel.SetActive(false);

        print("Small map has been updated!");
    }

    private IEnumerator SetBigTiles(Vector2Int center)
    {
        var i = 0;
        var textures = new Texture2D[m_BigTiles.Length];

        for (int h = -1; h <= 1; h++)
        {
            for (int w = -2; w <= 2; w++)
            {
                var url = $"https://tile.openstreetmap.org/{MAP_ZOOM}/{center.x + w}/{center.y + h}.png";

                using var map = UnityWebRequestTexture.GetTexture(url);

                yield return map.SendWebRequest();

                if (map.result == UnityWebRequest.Result.ConnectionError || map.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Map error: " + map.error);
                }

                var content = DownloadHandlerTexture.GetContent(map);

                textures[i++] = content;
            }
        }

        for (i = 0; i < textures.Length; i++)
        {
            m_BigTiles[i].texture = textures[i];
        }

        m_NoDataBigPanel.SetActive(false);

        print("Big map has been updated!");
    }

    private void SetMarkerLocation(Vector2Int center, double lat, double lon)
    {
        var x = GetWebMercatorX(lon) - center.x;
        var y = GetWebMercatorY(lat) - center.y;

        x = (2 * x - 1) * MAP_CELL_UI_SIZE / 2.0;
        y = (1 - 2 * y) * MAP_CELL_UI_SIZE / 2.0;

        m_RocketMarker.anchoredPosition = new Vector2((float)x, (float)y);

        print("Marker position has been set!");
    }

    private Vector2Int GetTileXY(double lat, double lon)
    {
        var x = (int)Math.Floor(GetWebMercatorX(lon));
        var y = (int)Math.Floor(GetWebMercatorY(lat));

        return new Vector2Int(x, y);
    }

    private double GetWebMercatorX(double lon)
    {
        return (lon + 180.0) / 360.0 * Math.Pow(2, MAP_ZOOM);
    }

    private double GetWebMercatorY(double lat)
    {
        return (1 - Math.Log(Math.Tan(lat * Math.PI / 180.0) + 1 / Math.Cos(lat * Math.PI / 180.0)) / Math.PI) * Math.Pow(2, MAP_ZOOM - 1);
    }
}