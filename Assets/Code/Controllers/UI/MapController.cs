/*
 * DOCUMENTATION: https://developers.google.com/maps/documentation/maps-static/start
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MapController : MonoBehaviour, ITelemetryDataRecipient, IReplayDataRecipient, ISimulationDataRecipient
{
    private const int MAP_ZOOM = 13;
    private const int MAP_IMAGE_SIZE = 350;
    private const string MAPS_URL = "https://maps.googleapis.com/maps/api/staticmap?";
    private const float MAP_UPDATE_RATE = 20f;

    [SerializeField] private TextMeshProUGUI m_LatitudeText;
    [SerializeField] private TextMeshProUGUI m_LongtitudeText;
    [SerializeField] private RawImage m_MapImage;
    [SerializeField] private RawImage m_MapBigImage;

    private float _lastUpdateTime = -999f;

    public void OnSetData(RecipientData data)
    {
        m_LatitudeText.SetText($"{MathUtils.NumberSevenDecimalPlaces(data.latitude)}°");
        m_LongtitudeText.SetText($"{MathUtils.NumberSevenDecimalPlaces(data.longitude)}°");

        if (Time.time > _lastUpdateTime + MAP_UPDATE_RATE)
        {
            StartCoroutine(GetLocationRoutine(data.latitude, data.longitude));

            _lastUpdateTime = Time.time;
        }
    }

    private IEnumerator GetLocationRoutine(double lat, double lon)
    {
        if (!string.IsNullOrEmpty(ConfigLoader.CurrentData.mapsApiKey))
        {
            var url = $"{MAPS_URL}center={lat},{lon}&zoom={MAP_ZOOM}&size={MAP_IMAGE_SIZE}x{MAP_IMAGE_SIZE}&key={ConfigLoader.CurrentData.mapsApiKey}";

            using var map = UnityWebRequestTexture.GetTexture(url);

            yield return map.SendWebRequest();

            if (map.result == UnityWebRequest.Result.ConnectionError || map.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Map error: " + map.error);

                AlertManager.Alert("Map error occured");
            }

            var content = DownloadHandlerTexture.GetContent(map);

            m_MapImage.texture = content;
            m_MapBigImage.texture = content;
        }
    }
}