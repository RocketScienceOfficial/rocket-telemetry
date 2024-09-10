using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmingController : MonoBehaviour, IDataRecipient
{
    [SerializeField] private Image m_GpsFixCheckImage;
    [SerializeField] private Color m_ActiveColor;
    [SerializeField] private Color m_InactiveColor;
    [SerializeField] private Button m_ArmButton;
    [SerializeField] private Toggle m_3v3VToggle;
    [SerializeField] private Toggle m_5VToggle;
    [SerializeField] private Toggle m_VBATToggle;
    [SerializeField] private GameObject m_LoadingPanel;

    private bool _armed;

    private void Start()
    {
        m_ArmButton.onClick.AddListener(() =>
        {
            m_LoadingPanel.SetActive(true);

            _armed = !_armed;

            SerialPortController.Instance.SerialPortWrite($"\\arm-{(_armed ? "enable" : "disable")}");
        });

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

        UpdateFlags(0);
    }

    public void OnSetData(RecipientData recipient)
    {
        UpdateFlags(recipient.controlFlags);
    }

    private void UpdateFlags(int flags)
    {
        m_GpsFixCheckImage.color = CheckFlag(flags, RecipientDataControlFlags.GPS) ? m_ActiveColor : m_InactiveColor;

        m_ArmButton.GetComponentInChildren<TextMeshProUGUI>().SetText(CheckFlag(flags, RecipientDataControlFlags.Armed) ? "DISARM" : "ARM");

        if ((_armed && CheckFlag(flags, RecipientDataControlFlags.Armed)) || (!_armed && !CheckFlag(flags, RecipientDataControlFlags.Armed)))
        {
            m_LoadingPanel.SetActive(false);
        }

        if (m_3v3VToggle.isOn == CheckFlag(flags, RecipientDataControlFlags.V3V3) && m_5VToggle.isOn == CheckFlag(flags, RecipientDataControlFlags.V5) && m_VBATToggle.isOn == CheckFlag(flags, RecipientDataControlFlags.VBat))
        {
            m_LoadingPanel.SetActive(false);
        }
    }

    private bool CheckFlag(int flags, RecipientDataControlFlags flag)
    {
        return (flags & (int)flag) > 0;
    }
}