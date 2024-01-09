using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BatteryPanelController : MonoBehaviour, ITelemetryDataRecipient, IReplayDataRecipient, ISimulationDataRecipient
{
    [SerializeField] private TextMeshProUGUI m_PercentageText;
    [SerializeField] private TextMeshProUGUI m_VoltageText;

    public void OnSetData(RecipientData recipient)
    {
        m_PercentageText.SetText(recipient.batteryPercentage + "%");
        m_VoltageText.SetText(recipient.batteryVoltage + "V");
    }
}