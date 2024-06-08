using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignalPanelController : MonoBehaviour, IDataRecipient
{
    private const float MIN_SIGNAL = -130f;
    private const float MAX_SIGNAL = -20f;

    [SerializeField] private GameObject m_SignalBigPanel;
    [SerializeField] private Image m_SignalFillImage;
    [SerializeField] private TextMeshProUGUI m_RSSIText;
    [SerializeField] private TextMeshProUGUI m_PacketLossText;

    private void Start()
    {
        m_SignalBigPanel.SetActive(true);
    }

    public void OnSetData(RecipientData recipient)
    {
        m_SignalBigPanel.SetActive(false);
        m_SignalFillImage.fillAmount = (recipient.signalStrength - MIN_SIGNAL) / (MAX_SIGNAL - MIN_SIGNAL);
        m_RSSIText.SetText("RSSI: " + recipient.signalStrength + " dbm");
        m_PacketLossText.SetText("Packet Loss: " + recipient.packetLoss + "%");
    }
}