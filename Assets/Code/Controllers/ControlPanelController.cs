using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanelController : MonoBehaviour
{
    private const float MAX_TIME_OF_NEW_FLAGS = 5f;

    [SerializeField] private Button m_ArmButton;
    [SerializeField] private Toggle m_3v3VToggle;
    [SerializeField] private Toggle m_5VToggle;
    [SerializeField] private Toggle m_VBATToggle;
    [SerializeField] private GameObject m_LoadingVoltagePanel;

    private float _startTimeOfNewFlags;
    private bool _uiEventsDisabled;
    private bool _initialSync;
    private byte _currentFlags;

    private void Start()
    {
        m_ArmButton.onClick.AddListener(() =>
        {
            if (!_uiEventsDisabled)
            {
                ToggleCurrentFlag(DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_ARM_ENABLED);
            }
        });

        m_3v3VToggle.onValueChanged.AddListener(v =>
        {
            if (!_uiEventsDisabled)
            {
                ToggleCurrentFlag(DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_3V3_ENABLED);
            }
        });

        m_5VToggle.onValueChanged.AddListener(v =>
        {
            if (!_uiEventsDisabled)
            {
                ToggleCurrentFlag(DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_5V_ENABLED);
            }
        });

        m_VBATToggle.onValueChanged.AddListener(v =>
        {
            if (!_uiEventsDisabled)
            {
                ToggleCurrentFlag(DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_VBAT_ENABLED);
            }
        });

        SerialCommunication.Instance.OnConnected += (sender, args) =>
        {
            _startTimeOfNewFlags = 0f;
            _initialSync = false;
            _currentFlags = 0;

            _uiEventsDisabled = true;

            m_ArmButton.GetComponentInChildren<TextMeshProUGUI>().SetText("ARM");
            m_3v3VToggle.isOn = false;
            m_5VToggle.isOn = false;
            m_VBATToggle.isOn = false;

            _uiEventsDisabled = false;

            m_ArmButton.interactable = false;
            m_LoadingVoltagePanel.SetActive(true);
        };

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
        if (!_initialSync)
        {
            return;
        }

        _currentFlags ^= (byte)flag;
        _startTimeOfNewFlags = Time.time;

        if (flag == DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_ARM_ENABLED)
        {
            m_ArmButton.GetComponentInChildren<TextMeshProUGUI>().SetText((_currentFlags & (byte)DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_ARM_ENABLED) > 0 ? "DISARM" : "ARM");
            m_ArmButton.interactable = false;
        }
        else
        {
            m_LoadingVoltagePanel.SetActive(true);
        }

        WriteFlagsToSerialPort();
    }

    private void UpdateFlags(byte newFlags)
    {
        if (CheckFlag(newFlags, DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_ARM_ENABLED, DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_ARM_ENABLED) &&
            CheckFlag(newFlags, DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_3V3_ENABLED, DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_3V3_ENABLED) &&
            CheckFlag(newFlags, DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_5V_ENABLED, DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_5V_ENABLED) &&
            CheckFlag(newFlags, DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_VBAT_ENABLED, DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_VBAT_ENABLED))
        {
            _startTimeOfNewFlags = 0f;

            m_ArmButton.interactable = true;
            m_LoadingVoltagePanel.SetActive(false);
        }
        else
        {
            if (Time.time - _startTimeOfNewFlags >= MAX_TIME_OF_NEW_FLAGS || _startTimeOfNewFlags == 0f || !_initialSync)
            {
                _startTimeOfNewFlags = 0f;

                m_ArmButton.interactable = true;
                m_LoadingVoltagePanel.SetActive(false);

                SynchronizeFlags(newFlags);
            }
        }

        _initialSync = true;
    }

    private bool CheckFlag(byte newFlags, DataLinkFlagsTelemetryDataControlFlags obcFlag, DataLinkFlagsTelemetryResponseControlFlags controlFlag)
    {
        return ((newFlags & (byte)obcFlag) == 0 && (_currentFlags & (byte)controlFlag) == 0) || ((newFlags & (byte)obcFlag) > 0 && (_currentFlags & (byte)controlFlag) > 0);
    }

    private void SynchronizeFlags(byte newFlags)
    {
        _currentFlags = 0;
        _currentFlags |= (byte)(((newFlags & (byte)DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_ARM_ENABLED) > 0) ? DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_ARM_ENABLED : 0);
        _currentFlags |= (byte)(((newFlags & (byte)DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_3V3_ENABLED) > 0) ? DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_3V3_ENABLED : 0);
        _currentFlags |= (byte)(((newFlags & (byte)DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_5V_ENABLED) > 0) ? DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_5V_ENABLED : 0);
        _currentFlags |= (byte)(((newFlags & (byte)DataLinkFlagsTelemetryDataControlFlags.DATALINK_FLAGS_TELEMETRY_DATA_CONTROL_VBAT_ENABLED) > 0) ? DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_VBAT_ENABLED : 0);

        WriteFlagsToSerialPort();

        _uiEventsDisabled = true;

        m_ArmButton.GetComponentInChildren<TextMeshProUGUI>().SetText((_currentFlags & (byte)DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_ARM_ENABLED) > 0 ? "DISARM" : "ARM");
        m_3v3VToggle.isOn = (_currentFlags & (byte)DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_3V3_ENABLED) > 0;
        m_5VToggle.isOn = (_currentFlags & (byte)DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_5V_ENABLED) > 0;
        m_VBATToggle.isOn = (_currentFlags & (byte)DataLinkFlagsTelemetryResponseControlFlags.DATALINK_FLAGS_TELEMETRY_RESPONSE_CONTROL_VBAT_ENABLED) > 0;

        _uiEventsDisabled = false;

        print("Control flags synchronized!");
    }

    private void WriteFlagsToSerialPort()
    {
        SerialCommunication.Instance.SerialPortWrite(new DataLinkFrame
        {
            msgId = DataLinkMessageType.DATALINK_MESSAGE_TELEMETRY_RESPONSE,
            payload = BytesConverter.GetBytes(new DataLinkFrameTelemetryResponse
            {
                controlFlags = _currentFlags,
            }),
        });
    }
}