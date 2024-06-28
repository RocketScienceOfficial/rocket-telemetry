using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BatteryPanelController : MonoBehaviour, IDataRecipient
{
    [SerializeField] private TextMeshProUGUI m_PercentageText;
    [SerializeField] private TextMeshProUGUI m_VoltageText;
    [SerializeField] private Image m_Fill;

    private void Start()
    {
        m_PercentageText.SetText("NaN %");
        m_VoltageText.SetText("NaN V");
        m_Fill.fillAmount = 0;
    }

    public void OnSetData(RecipientData recipient)
    {
        m_PercentageText.SetText(recipient.batteryPercentage + "%");
        m_VoltageText.SetText(recipient.batteryVoltage + "V");
        m_Fill.fillAmount = 1f - recipient.batteryPercentage / 100f;
    }
}