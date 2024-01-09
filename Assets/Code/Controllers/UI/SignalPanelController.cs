using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignalPanelController : MonoBehaviour, ITelemetryDataRecipient, IReplayDataRecipient, ISimulationDataRecipient
{
    private const float MIN_SIGNAL = -150f;
    private const float MAX_SIGNAL = -50f;

    [SerializeField] private Image m_SignalFillImage;
    [SerializeField] private TextMeshProUGUI m_RSSIText;
    [SerializeField] private TextMeshProUGUI m_PacketLossText;

    public void OnSetData(RecipientData recipient)
    {
        m_SignalFillImage.fillAmount = (recipient.signalStrength - MIN_SIGNAL) / (MAX_SIGNAL - MIN_SIGNAL);
        m_RSSIText.SetText("RSSI: " + recipient.signalStrength + " dbm");
        m_PacketLossText.SetText("Packet Loss: " + recipient.packetLoss + "%");
    }
}