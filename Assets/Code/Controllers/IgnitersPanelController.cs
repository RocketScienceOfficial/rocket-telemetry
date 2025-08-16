using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IgnitersPanelController : MonoBehaviour
{
    [SerializeField] private Image m_IGN1StatusImage;
    [SerializeField] private Image m_IGN2StatusImage;
    [SerializeField] private Image m_IGN3StatusImage;
    [SerializeField] private Image m_IGN4StatusImage;

    private void Start()
    {
        SerialCommunication.Instance.OnConnected += (sender, args) =>
        {
            SetIGNStatusImage(m_IGN1StatusImage, false);
            SetIGNStatusImage(m_IGN2StatusImage, false);
            SetIGNStatusImage(m_IGN3StatusImage, false);
            SetIGNStatusImage(m_IGN4StatusImage, false);
        };

        SerialCommunication.Instance.OnRead += (sender, args) =>
        {
            var msg = args.Frame;

            if (msg.msgId == DataLinkMessageType.DATALINK_MESSAGE_TELEMETRY_DATA_GCS)
            {
                var payload = BytesConverter.FromBytes<DataLinkFrameTelemetryDataGCS>(msg.payload);

                SetIGNStatusImage(m_IGN1StatusImage, (payload.controlFlags & (byte)DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_IGN_1) > 0);
                SetIGNStatusImage(m_IGN2StatusImage, (payload.controlFlags & (byte)DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_IGN_2) > 0);
                SetIGNStatusImage(m_IGN3StatusImage, (payload.controlFlags & (byte)DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_IGN_3) > 0);
                SetIGNStatusImage(m_IGN4StatusImage, (payload.controlFlags & (byte)DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_IGN_4) > 0);
            }
        };
    }

    private void SetIGNStatusImage(Image img, bool cont)
    {
        img.color = cont ? Color.green : Color.red;
    }
}