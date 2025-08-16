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
        SerialCommunication.Instance.OnConnected += (sender, args) =>
        {
            SetData(0, 0);
        };

        SerialCommunication.Instance.OnRead += (sender, args) =>
        {
            var msg = args.Frame;

            if (msg.msgId == DataLinkMessageType.DATALINK_MESSAGE_TELEMETRY_DATA_GCS)
            {
                var payload = BytesConverter.FromBytes<DataLinkFrameTelemetryDataGCS>(msg.payload);

                SetData(payload.batteryPercentage, payload.batteryVoltage100 / 100f);
            }
        };
    }

    private void SetData(int batteryPercentage, float batteryVoltage)
    {
        m_PercentageText.SetText(batteryPercentage + "%");
        m_VoltageText.SetText(string.Format("{0:0.00}", batteryVoltage).Replace(',', '.') + "V");
        m_Fill.fillAmount = 1f - batteryPercentage / 100f;
    }
}