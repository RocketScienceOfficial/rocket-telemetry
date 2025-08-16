using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaugesPanelController : MonoBehaviour
{
    [SerializeField] private GaugePanelController m_SpeedPanel;
    [SerializeField] private GaugePanelController m_AltitudePanel;

    private void Start()
    {
        SerialCommunication.Instance.OnConnected += (sender, args) =>
        {
            SetValues(0, 0);
        };

        SerialCommunication.Instance.OnRead += (sender, args) =>
        {
            var msg = args.Frame;

            if (msg.msgId == DataLinkMessageType.DATALINK_MESSAGE_TELEMETRY_DATA_GCS)
            {
                var payload = BytesConverter.FromBytes<DataLinkFrameTelemetryDataGCS>(msg.payload);

                SetValues(payload.velocity_kmh, payload.alt);
            }
        };
    }

    private void SetValues(int vel, int alt)
    {
        m_SpeedPanel.SetValue(vel, 0, 2000);
        m_AltitudePanel.SetValue(alt, 0, 2000);
    }
}