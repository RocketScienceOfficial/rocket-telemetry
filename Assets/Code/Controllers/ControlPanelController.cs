using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanelController : MonoBehaviour
{
    [SerializeField] private Button m_ArmButton;
    [SerializeField] private Toggle m_3v3VToggle;
    [SerializeField] private Toggle m_5VToggle;
    [SerializeField] private Toggle m_VBATToggle;
    [SerializeField] private Image m_IGN1StatusImage;
    [SerializeField] private Image m_IGN2StatusImage;
    [SerializeField] private Image m_IGN3StatusImage;
    [SerializeField] private Image m_IGN4StatusImage;
    [SerializeField] private GameObject m_LoadingPanel;

    private byte _currentFlags;

    private void Start()
    {
        m_ArmButton.onClick.AddListener(() =>
        {
            ToggleCurrentFlag(DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_ARM_ENABLED);

            m_ArmButton.GetComponentInChildren<TextMeshProUGUI>().SetText((_currentFlags & (byte)DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_ARM_ENABLED) > 0 ? "DISARM" : "ARM");
        });

        m_3v3VToggle.isOn = false;
        m_3v3VToggle.onValueChanged.AddListener(v =>
        {
            ToggleCurrentFlag(DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_3V3_ENABLED);
        });

        m_5VToggle.isOn = false;
        m_5VToggle.onValueChanged.AddListener(v =>
        {
            ToggleCurrentFlag(DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_5V_ENABLED);
        });

        m_VBATToggle.isOn = false;
        m_VBATToggle.onValueChanged.AddListener(v =>
        {
            ToggleCurrentFlag(DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_VBAT_ENABLED);
        });

        SerialCommunication.Instance.OnRead += (sender, args) =>
        {
            var msg = args.Frame;

            if (msg.msgId == DataLinkMessageType.DATALINK_MESSAGE_TELEMETRY_DATA_GCS)
            {
                var payload = BytesConverter.FromBytes<DataLinkFrameTelemetryDataGCS>(msg.payload);

                UpdateFlags(payload.controlFlags);
            }
        };

        UpdateFlags(0x00);
    }

    private void ToggleCurrentFlag(DataLinkFlagsTelemetryResponseControlFlags flag)
    {
        m_LoadingPanel.SetActive(true);

        _currentFlags ^= (byte)flag;

        SerialCommunication.Instance.SerialPortWrite(new DataLinkFrame
        {
            msgId = DataLinkMessageType.DATALINK_MESSAGE_TELEMETRY_RESPONSE,
            payload = BytesConverter.GetBytes(new DataLinkFrameTelemetryResponse
            {
                controlFlags = _currentFlags,
            }),
        });
    }

    private void UpdateFlags(byte newFlags)
    {
        SetIGNStatusImage(m_IGN1StatusImage, (newFlags & (byte)DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_IGN_1) > 0);
        SetIGNStatusImage(m_IGN2StatusImage, (newFlags & (byte)DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_IGN_2) > 0);
        SetIGNStatusImage(m_IGN3StatusImage, (newFlags & (byte)DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_IGN_3) > 0);
        SetIGNStatusImage(m_IGN4StatusImage, (newFlags & (byte)DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_IGN_4) > 0);

        m_ArmButton.GetComponentInChildren<TextMeshProUGUI>().SetText((newFlags & (byte)DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_ARM_ENABLED) > 0 ? "DISARM" : "ARM");
        m_3v3VToggle.isOn = (newFlags & (byte)DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_3V3_ENABLED) > 0;
        m_5VToggle.isOn = (newFlags & (byte)DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_5V_ENABLED) > 0;
        m_VBATToggle.isOn = (newFlags & (byte)DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_VBAT_ENABLED) > 0;

        SynchronizeFlags(newFlags);

        m_LoadingPanel.SetActive(false);
    }

    private void SetIGNStatusImage(Image img, bool cont)
    {
        img.color = cont ? Color.green : Color.red;
    }

    private void SynchronizeFlags(byte newFlags)
    {
        _currentFlags = 0;

        _currentFlags |= (byte)(((newFlags & (byte)DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_ARM_ENABLED) > 0) ? (byte)DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_ARM_ENABLED : 0);
        _currentFlags |= (byte)(((newFlags & (byte)DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_3V3_ENABLED) > 0) ? (byte)DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_3V3_ENABLED : 0);
        _currentFlags |= (byte)(((newFlags & (byte)DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_5V_ENABLED) > 0) ? (byte)DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_5V_ENABLED : 0);
        _currentFlags |= (byte)(((newFlags & (byte)DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_VBAT_ENABLED) > 0) ? (byte)DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_VBAT_ENABLED : 0);
    }
}