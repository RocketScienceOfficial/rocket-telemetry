using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaugesPanelController : MonoBehaviour
{
    [SerializeField] private GaugePanelController m_SpeedPanel;
    [SerializeField] private GaugePanelController m_AltitudePanel;

    private void Start()
    {
        SerialCommunication.Instance.OnRead += (sender, args) =>
        {
            var msg = args.Frame;

            if (msg.msgId == DataLinkMessageType.DATALINK_MESSAGE_TELEMETRY_DATA_GCS)
            {
                var payload = BytesConverter.FromBytes<DataLinkFrameTelemetryDataGCS>(msg.payload);

                m_SpeedPanel.SetValue(payload.velocity, 0, 2000);
                m_AltitudePanel.SetValue(payload.alt, 0, 2000);
            }
        };
    }
}