using System.Collections;
using System.Collections.Generic;
using System.Web.UI.Design;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanelController : MonoBehaviour
{
    private const float MAX_TIME_OF_NEW_FLAGS = 10f;

    [SerializeField] private Button m_ArmButton;
    [SerializeField] private Toggle m_3v3VToggle;
    [SerializeField] private Toggle m_5VToggle;
    [SerializeField] private Toggle m_VBATToggle;
    [SerializeField] private Image m_IGN1StatusImage;
    [SerializeField] private Image m_IGN2StatusImage;
    [SerializeField] private Image m_IGN3StatusImage;
    [SerializeField] private Image m_IGN4StatusImage;
    [SerializeField] private GameObject m_LoadingPanel;

    private float _startTimeOfNewFlags;
    private bool _initialized;
    private bool _uiEventsDisabled;
    private byte _currentFlags;

    private void Start()
    {
        m_ArmButton.GetComponentInChildren<TextMeshProUGUI>().SetText("ARM");
        m_ArmButton.onClick.AddListener(() =>
        {
            if (!_uiEventsDisabled)
            {
                ToggleCurrentFlag(DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_ARM_ENABLED);

                m_ArmButton.GetComponentInChildren<TextMeshProUGUI>().SetText((_currentFlags & (byte)DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_ARM_ENABLED) > 0 ? "DISARM" : "ARM");
            }
        });

        m_3v3VToggle.isOn = false;
        m_3v3VToggle.onValueChanged.AddListener(v =>
        {
            if (!_uiEventsDisabled)
            {
                ToggleCurrentFlag(DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_3V3_ENABLED);
            }
        });

        m_5VToggle.isOn = false;
        m_5VToggle.onValueChanged.AddListener(v =>
        {
            if (!_uiEventsDisabled)
            {
                ToggleCurrentFlag(DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_5V_ENABLED);
            }
        });

        m_VBATToggle.isOn = false;
        m_VBATToggle.onValueChanged.AddListener(v =>
        {
            if (!_uiEventsDisabled)
            {
                ToggleCurrentFlag(DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_VBAT_ENABLED);
            }
        });

        SetIGNStatusImage(m_IGN1StatusImage, false);
        SetIGNStatusImage(m_IGN2StatusImage, false);
        SetIGNStatusImage(m_IGN3StatusImage, false);
        SetIGNStatusImage(m_IGN4StatusImage, false);

        m_LoadingPanel.SetActive(true);

        SerialCommunication.Instance.OnRead += (sender, args) =>
        {
            var msg = args.Frame;

            if (msg.msgId == DataLinkMessageType.DATALINK_MESSAGE_TELEMETRY_DATA_GCS)
            {
                var payload = BytesConverter.FromBytes<DataLinkFrameTelemetryDataGCS>(msg.payload);

                UpdateFlags(payload.controlFlags);
            }
        };
    }

    private void ToggleCurrentFlag(DataLinkFlagsTelemetryResponseControlFlags flag)
    {
        _currentFlags ^= (byte)flag;
        _startTimeOfNewFlags = Time.time;

        m_LoadingPanel.SetActive(true);

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

        if (CheckFlag(newFlags, DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_ARM_ENABLED, DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_ARM_ENABLED) &&
            CheckFlag(newFlags, DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_3V3_ENABLED, DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_3V3_ENABLED) &&
            CheckFlag(newFlags, DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_5V_ENABLED, DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_5V_ENABLED) &&
            CheckFlag(newFlags, DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_VBAT_ENABLED, DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_VBAT_ENABLED))
        {
            _startTimeOfNewFlags = 0f;

            m_LoadingPanel.SetActive(false);
        }
        else
        {
            if (Time.time - _startTimeOfNewFlags >= MAX_TIME_OF_NEW_FLAGS || !_initialized)
            {
                _startTimeOfNewFlags = 0f;

                SynchronizeFlags(newFlags);

                m_LoadingPanel.SetActive(false);
            }
        }

        _initialized = true;
    }

    private bool CheckFlag(byte newFlags, DataLinkFlagsTelemetryDataControlFlags obcFlag, DataLinkFlagsTelemetryResponseControlFlags controlFlag)
    {
        return ((newFlags & (byte)obcFlag) == 0 && (_currentFlags & (byte)controlFlag) == 0) || ((newFlags & (byte)obcFlag) > 0 && (_currentFlags & (byte)controlFlag) > 0);
    }

    private void SetIGNStatusImage(Image img, bool cont)
    {
        img.color = cont ? Color.green : Color.red;
    }

    private void SynchronizeFlags(byte newFlags)
    {
        _currentFlags = 0;
        _currentFlags |= (byte)(((newFlags & (byte)DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_ARM_ENABLED) > 0) ? DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_ARM_ENABLED : 0);
        _currentFlags |= (byte)(((newFlags & (byte)DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_3V3_ENABLED) > 0) ? DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_3V3_ENABLED : 0);
        _currentFlags |= (byte)(((newFlags & (byte)DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_5V_ENABLED) > 0) ? DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_5V_ENABLED : 0);
        _currentFlags |= (byte)(((newFlags & (byte)DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_VBAT_ENABLED) > 0) ? DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_VBAT_ENABLED : 0);

        _uiEventsDisabled = true;

        m_ArmButton.GetComponentInChildren<TextMeshProUGUI>().SetText((_currentFlags & (byte)DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_ARM_ENABLED) > 0 ? "DISARM" : "ARM");
        m_3v3VToggle.isOn = (_currentFlags & (byte)DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_3V3_ENABLED) > 0;
        m_5VToggle.isOn = (_currentFlags & (byte)DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_5V_ENABLED) > 0;
        m_VBATToggle.isOn = (_currentFlags & (byte)DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_VBAT_ENABLED) > 0;

        _uiEventsDisabled = false;

        print("Control flags synchronized!");
    }
}