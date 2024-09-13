using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmingController : MonoBehaviour, IDataRecipient
{
    [SerializeField] private Color m_ActiveColor;
    [SerializeField] private Color m_InactiveColor;
    [SerializeField] private Button m_ArmButton;
    [SerializeField] private Toggle m_3v3VToggle;
    [SerializeField] private Toggle m_5VToggle;
    [SerializeField] private Toggle m_VBATToggle;
    [SerializeField] private Image m_IGN1StatusImage;
    [SerializeField] private Image m_IGN2StatusImage;
    [SerializeField] private Image m_IGN3StatusImage;
    [SerializeField] private Image m_IGN4StatusImage;
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
        m_ArmButton.GetComponentInChildren<TextMeshProUGUI>().SetText(CheckFlag(flags, RecipientDataControlFlags.Armed) ? "DISARM" : "ARM");

        SetIGNStatusImage(m_IGN1StatusImage, CheckFlag(flags, RecipientDataControlFlags.IGN1));
        SetIGNStatusImage(m_IGN2StatusImage, CheckFlag(flags, RecipientDataControlFlags.IGN2));
        SetIGNStatusImage(m_IGN3StatusImage, CheckFlag(flags, RecipientDataControlFlags.IGN3));
        SetIGNStatusImage(m_IGN4StatusImage, CheckFlag(flags, RecipientDataControlFlags.IGN4));

        if (_armed == CheckFlag(flags, RecipientDataControlFlags.Armed) &&
            m_3v3VToggle.isOn == CheckFlag(flags, RecipientDataControlFlags.V3V3) && m_5VToggle.isOn == CheckFlag(flags, RecipientDataControlFlags.V5) && m_VBATToggle.isOn == CheckFlag(flags, RecipientDataControlFlags.VBat) &&
            GetIGNImageStatus(m_IGN1StatusImage) == CheckFlag(flags, RecipientDataControlFlags.IGN1) && GetIGNImageStatus(m_IGN2StatusImage) == CheckFlag(flags, RecipientDataControlFlags.IGN2) && GetIGNImageStatus(m_IGN3StatusImage) == CheckFlag(flags, RecipientDataControlFlags.IGN3) && GetIGNImageStatus(m_IGN4StatusImage) == CheckFlag(flags, RecipientDataControlFlags.IGN4))
        {
            m_LoadingPanel.SetActive(false);
        }
    }

    private bool CheckFlag(int flags, RecipientDataControlFlags flag)
    {
        return (flags & (int)flag) > 0;
    }

    private void SetIGNStatusImage(Image img, bool cont)
    {
        img.color = cont ? Color.green : Color.red;
    }

    private bool GetIGNImageStatus(Image img)
    {
        return img.color == Color.green;
    }
}