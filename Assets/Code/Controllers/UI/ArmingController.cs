using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmingController : MonoBehaviour, IDataRecipient
{
    [SerializeField] private Image m_SelfTestCheckImage;
    [SerializeField] private Image m_GpsFixCheckImage;
    [SerializeField] private Image m_CalibrationCheckImage;
    [SerializeField] private Color m_ActiveColor;
    [SerializeField] private Color m_InactiveColor;
    [SerializeField] private Button m_ArmButton;
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

        m_LoadingPanel.SetActive(false);

        UpdateFlags(0);
    }

    public void OnSetData(RecipientData recipient)
    {
        UpdateFlags(recipient.controlFlags);
    }

    private void UpdateFlags(int flags)
    {
        m_SelfTestCheckImage.color = CheckFlag(flags, RecipientDataControlFlags.SelfTest) ? m_ActiveColor : m_InactiveColor;
        m_GpsFixCheckImage.color = CheckFlag(flags, RecipientDataControlFlags.GPS) ? m_ActiveColor : m_InactiveColor;
        m_CalibrationCheckImage.color = CheckFlag(flags, RecipientDataControlFlags.Calibration) ? m_ActiveColor : m_InactiveColor;

        m_ArmButton.GetComponentInChildren<TextMeshProUGUI>().SetText(CheckFlag(flags, RecipientDataControlFlags.Armed) ? "DISARM" : "ARM");

        if ((_armed && CheckFlag(flags, RecipientDataControlFlags.Armed)) || (!_armed && !CheckFlag(flags, RecipientDataControlFlags.Armed)))
        {
            m_LoadingPanel.SetActive(false);
        }
    }

    private bool CheckFlag(int flags, RecipientDataControlFlags flag)
    {
        return (flags & (int)flag) > 0;
    }
}