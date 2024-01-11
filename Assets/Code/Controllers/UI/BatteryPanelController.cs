using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BatteryPanelController : MonoBehaviour, ITelemetryDataRecipient, IReplayDataRecipient, ISimulationDataRecipient
{
    [SerializeField] private TextMeshProUGUI m_PercentageText;
    [SerializeField] private TextMeshProUGUI m_VoltageText;
    [SerializeField] private Image m_Fill;

    public void OnSetData(RecipientData recipient)
    {
        m_PercentageText.SetText(recipient.batteryPercentage + "%");
        m_VoltageText.SetText(recipient.batteryVoltage + "V");
        m_Fill.fillAmount = 1f - recipient.batteryPercentage / 100f;
    }
}