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
    private const float MAP_UPDATE_RATE = 5f;
    private const int MAP_CELL_UI_SIZE = 250;

    [SerializeField] private TextMeshProUGUI m_LatitudeText;
    [SerializeField] private TextMeshProUGUI m_LongtitudeText;
    [SerializeField] private RawImage[] m_SmallTiles;
    [SerializeField] private RawImage[] m_BigTiles;
    [SerializeField] private RectTransform m_RocketMarker;
    [SerializeField] private GameObject m_NoDataSmallPanel;
    [SerializeField] private GameObject m_NoDataBigPanel;

    private float _lastUpdateTime;
    private Vector2Int _currentCenter;

    private void Start()
    {
        m_NoDataSmallPanel.SetActive(true);
        m_NoDataBigPanel.SetActive(true);

        m_LatitudeText.SetText("NaN");
        m_LongtitudeText.SetText("NaN");
    }

    public void OnSetData(RecipientData data)
    {
        StartCoroutine(UpdateMap(data.latitude, data.longitude));
    }

    private IEnumerator UpdateMap(double lat, double lon)
    {
        m_LatitudeText.SetText($"{MathUtils.NumberSevenDecimalPlaces(lat)}�");
        m_LongtitudeText.SetText($"{MathUtils.NumberSevenDecimalPlaces(lon)}�");

        if (lat != 0)
        {
            var pos = GetTileXY(lat, lon);

            if ((Mathf.Abs(pos.x - _currentCenter.x) >= 2 || Mathf.Abs(pos.y - _currentCenter.y) >= 2) && (Time.time - MAP_UPDATE_RATE >= _lastUpdateTime || _lastUpdateTime == 0))
            {
                yield return SetTiles(pos);

                _lastUpdateTime = Time.time;
                _currentCenter = pos;
            }

            SetMarkerLocation(_currentCenter, lat, lon);
        }
    }

    private IEnumerator SetTiles(Vector2Int center)
    {
        var indexSmall = 0;
        var indexBig = 0;
        var texturesSmall = new Texture2D[m_SmallTiles.Length];
        var texturesBig = new Texture2D[m_BigTiles.Length];

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

                if (w >= -1 && w <= 1)
                {
                    texturesSmall[indexSmall++] = content;
                }

                texturesBig[indexBig++] = content;
            }
        }

        for (var i = 0; i < texturesSmall.Length; i++)
        {
            m_SmallTiles[i].texture = texturesSmall[i];
        }

        for (var i = 0; i < texturesBig.Length; i++)
        {
            m_BigTiles[i].texture = texturesBig[i];
        }

        m_NoDataSmallPanel.SetActive(false);
        m_NoDataBigPanel.SetActive(false);

        print("Maps have been updated!");
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