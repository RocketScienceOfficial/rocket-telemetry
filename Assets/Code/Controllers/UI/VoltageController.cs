using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoltageController : MonoBehaviour, IDataRecipient
{
    [SerializeField] private Toggle m_3v3VToggle;
    [SerializeField] private Toggle m_5VToggle;
    [SerializeField] private Toggle m_VBATToggle;
    [SerializeField] private GameObject m_LoadingPanel;

    private void Start()
    {
        m_3v3VToggle.isOn = false;
        m_5VToggle.isOn = false;
        m_VBATToggle.isOn = false;

        m_3v3VToggle.onValueChanged.AddListener(v =>
        {
            m_LoadingPanel.SetActive(true);

            SerialPortController.Instance.SerialPortWrite($"\\voltage-3v3-{(m_3v3VToggle.isOn ? "enable" : "disable")}");
        });

        m_5VToggle.onValueChanged.AddListener(v =>
        {
            m_LoadingPanel.SetActive(true);

            SerialPortController.Instance.SerialPortWrite($"\\voltage-5v-{(m_5VToggle.isOn ? "enable" : "disable")}");
        });

        m_VBATToggle.onValueChanged.AddListener(v =>
        {
            m_LoadingPanel.SetActive(true);

            SerialPortController.Instance.SerialPortWrite($"\\voltage-vbat-{(m_VBATToggle.isOn ? "enable" : "disable")}");
        });

        m_LoadingPanel.SetActive(false);
    }

    public void OnSetData(RecipientData data)
    {
        if (m_3v3VToggle.isOn == CheckFlag(data.controlFlags, RecipientDataControlFlags.V3V3) && m_5VToggle.isOn == CheckFlag(data.controlFlags, RecipientDataControlFlags.V5) && m_VBATToggle.isOn == CheckFlag(data.controlFlags, RecipientDataControlFlags.VBat))
        {
            m_LoadingPanel.SetActive(false);
        }
    }

    private bool CheckFlag(int flags, RecipientDataControlFlags flag)
    {
        return (flags & (int)flag) > 0;
    }
}