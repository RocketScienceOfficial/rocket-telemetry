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
    private const int MAP_ZOOM = 15;
    private const string MAPS_URL = "https://tile.openstreetmap.org";
    private const float MAP_UPDATE_RATE = 5f;

    [SerializeField] private TextMeshProUGUI m_LatitudeText;
    [SerializeField] private TextMeshProUGUI m_LongtitudeText;
    [SerializeField] private RawImage[] m_Tiles;

    private float _lastUpdateTime;

    public void OnSetData(RecipientData data)
    {
        m_LatitudeText.SetText($"{MathUtils.NumberSevenDecimalPlaces(data.latitude)}°");
        m_LongtitudeText.SetText($"{MathUtils.NumberSevenDecimalPlaces(data.longitude)}°");

        if (Time.time - MAP_UPDATE_RATE > _lastUpdateTime || _lastUpdateTime == 0)
        {
            StartCoroutine(GetLocationRoutine(data.latitude, data.longitude));

            _lastUpdateTime = Time.time;
        }
    }

    private IEnumerator GetLocationRoutine(double lat, double lon)
    {
        var pos = GetTileXY(new LatLon(lat, lon));
        var i = 0;
        var textures = new Texture2D[m_Tiles.Length];

        for (int h = -1; h <= 1; h++)
        {
            for (int w = -1; w <= 1; w++)
            {
                var url = $"{MAPS_URL}/{MAP_ZOOM}/{pos.x + w}/{pos.y + h}.png";

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
            m_Tiles[i].texture = textures[i];
        }

        print("Map has been updated!");
    }

    private Vector2Int GetTileXY(LatLon ll)
    {
        var x = (int)Math.Floor((ll.lon + 180.0) / 360.0 * Math.Pow(2, MAP_ZOOM));
        var y = (int)Math.Floor((1 - Math.Log(Math.Tan(ll.lat * Math.PI / 180.0) + 1 / Math.Cos(ll.lat * Math.PI / 180.0)) / Math.PI) * Math.Pow(2, MAP_ZOOM - 1));

        return new Vector2Int(x, y);
    }

    private LatLon GetTileLatLon(Vector2Int tile)
    {
        var lon = tile.x / Math.Pow(2, MAP_ZOOM) * 360.0 - 180.0;
        var lat = Math.Atan(Math.Sinh(Math.PI - tile.y / Math.Pow(2, MAP_ZOOM) * 2 * Math.PI)) * 180.0 / Math.PI;

        return new LatLon() { lat = lat, lon = lon };
    }


    public struct LatLon
    {
        public double lat, lon;

        public LatLon(double lat, double lon)
        {
            this.lat = lat;
            this.lon = lon;
        }
    }
}