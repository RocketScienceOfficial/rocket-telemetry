using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignalPanelController : MonoBehaviour
{
    private const float MIN_SIGNAL = -130f;
    private const float MAX_SIGNAL = -10f;
    private const float SIGNAL_LOST_TIMEOUT = 3.0f;

    [SerializeField] private GameObject m_SignalBigPanel;
    [SerializeField] private Image m_SignalFillImage;
    [SerializeField] private TextMeshProUGUI m_RSSIText;
    [SerializeField] private TextMeshProUGUI m_PacketLossText;
    [SerializeField] private TextMeshProUGUI m_RateText;

    private float _signalTimeout;
    private float _lastPacketTime;

    private void Start()
    {
        m_SignalBigPanel.SetActive(true);
        m_SignalFillImage.fillAmount = 0;
        m_RSSIText.SetText("RSSI: NaN");
        m_PacketLossText.SetText("Packet Loss: NaN");
        m_RateText.SetText("Rate: NaN");

        SerialCommunication.Instance.OnRead += (sender, args) =>
        {
            var msg = args.Frame;

            if (msg.msgId == DataLinkMessageType.DATALINK_MESSAGE_TELEMETRY_DATA_GCS)
            {
                var payload = BytesConverter.FromBytes<DataLinkFrameTelemetryDataGCS>(msg.payload);
                var signalStrength = -((int)payload.signalStrengthNeg);
                var packetLoss = (int)payload.packetLossPercentage;
                var rate = 1f / (Time.time - _lastPacketTime);

                m_SignalBigPanel.SetActive(false);
                m_SignalFillImage.fillAmount = (signalStrength - MIN_SIGNAL) / (MAX_SIGNAL - MIN_SIGNAL);
                m_RSSIText.SetText("RSSI: " + signalStrength + " dbm");
                m_PacketLossText.SetText("Packet Loss: " + packetLoss + "%");
                m_RateText.SetText("Rate: " + MathUtils.NumberOneDecimalPlace(rate));

                _signalTimeout = 0f;
                _lastPacketTime = Time.time;
            }
        };
    }

    private void Update()
    {
        if (_signalTimeout >= SIGNAL_LOST_TIMEOUT)
        {
            m_SignalBigPanel.SetActive(true);
        }
        else
        {
            _signalTimeout += Time.deltaTime;
        }
    }
}