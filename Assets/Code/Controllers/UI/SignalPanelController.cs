using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignalPanelController : MonoBehaviour, IDataRecipient
{
    private const float MIN_SIGNAL = -130f;
    private const float MAX_SIGNAL = -10f;
    private const float SIGNAL_LOST_TIMEOUT = 1.0f;

    [SerializeField] private GameObject m_SignalBigPanel;
    [SerializeField] private Image m_SignalFillImage;
    [SerializeField] private TextMeshProUGUI m_RSSIText;
    [SerializeField] private TextMeshProUGUI m_PacketLossText;

    private float _signalTimeout;

    private void Start()
    {
        m_SignalBigPanel.SetActive(true);
        m_SignalFillImage.fillAmount = 0;
        m_RSSIText.SetText("RSSI: NaN");
        m_PacketLossText.SetText("Packet Loss: NaN");
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

    public void OnSetData(RecipientData recipient)
    {
        m_SignalBigPanel.SetActive(false);
        m_SignalFillImage.fillAmount = (recipient.signalStrength - MIN_SIGNAL) / (MAX_SIGNAL - MIN_SIGNAL);
        m_RSSIText.SetText("RSSI: " + recipient.signalStrength + " dbm");
        m_PacketLossText.SetText("Packet Loss: " + recipient.packetLoss + "%");

        _signalTimeout = 0f;
    }
}