using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BatteryPanelController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_PercentageText;
    [SerializeField] private TextMeshProUGUI m_VoltageText;
    [SerializeField] private Image m_Fill;

    private void Start()
    {
        m_PercentageText.SetText("NaN %");
        m_VoltageText.SetText("NaN V");
        m_Fill.fillAmount = 0;

        SerialCommunication.Instance.OnRead += (sender, args) =>
        {
            var msg = args.Frame;

            if (msg.msgId == DataLinkMessageType.DATALINK_MESSAGE_TELEMETRY_DATA_GCS)
            {
                var payload = BytesConverter.FromBytes<DataLinkFrameTelemetryDataGCS>(msg.payload);

                m_PercentageText.SetText(payload.batteryPercentage + "%");
                m_VoltageText.SetText(MathUtils.NumberTwoDecimalPlaces(payload.batteryVoltage100 / 100.0f) + "V");
                m_Fill.fillAmount = 1f - payload.batteryPercentage / 100f;
            }
        };
    }
}