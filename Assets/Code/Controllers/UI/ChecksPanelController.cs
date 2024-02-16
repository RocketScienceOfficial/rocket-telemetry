using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChecksPanelController : MonoBehaviour//, ICommandRecipient
{
    [SerializeField] private Button m_OpenPanelButton;
    [SerializeField] private Button m_RunChecksButton;
    [SerializeField] private GameObject m_Panel;
    [SerializeField] private GameObject m_CalibrationCheck;
    [SerializeField] private GameObject m_SelfTestCheck;
    [SerializeField] private GameObject m_GPSCheck;

    //private void Start()
    //{
    //    SetCheckState(m_CalibrationCheck, CheckState.None);
    //    SetCheckState(m_SelfTestCheck, CheckState.None);
    //    SetCheckState(m_GPSCheck, CheckState.None);

    //    m_OpenPanelButton.onClick.AddListener(() =>
    //    {
    //        m_Panel.SetActive(!m_Panel.activeSelf);
    //    });

    //    m_RunChecksButton.onClick.AddListener(() =>
    //    {
    //        RunChecks();

    //        m_RunChecksButton.interactable = false;
    //    });
    //}

    //private void SetCheckState(GameObject check, CheckState state)
    //{
    //    if (state == CheckState.Failed)
    //    {
    //        // Abort
    //    }

    //    check.GetComponentInChildren<Image>().color = state switch { CheckState.Passed => Color.green, CheckState.Failed => Color.red, CheckState.Checking => Color.blue, CheckState.None => Color.white, _ => Color.red };
    //}

    //private void RunChecks()
    //{
    //    SetCheckState(m_CalibrationCheck, CheckState.Checking);
    //    SetCheckState(m_SelfTestCheck, CheckState.Checking);
    //    SetCheckState(m_GPSCheck, CheckState.Checking);

    //    SerialPortController.Instance.SerialPortWrite("check-run");
    //}

    //public void OnSetCommand(string cmd)
    //{
    //    if (cmd == "check-calib-passed")
    //    {
    //        SetCheckState(m_CalibrationCheck, CheckState.Passed);
    //    }
    //    else if (cmd == "check-calib-failed")
    //    {
    //        SetCheckState(m_CalibrationCheck, CheckState.Failed);
    //    }
    //    else if (cmd == "check-selftest-passed")
    //    {
    //        SetCheckState(m_SelfTestCheck, CheckState.Passed);
    //    }
    //    else if (cmd == "check-selftest-failed")
    //    {
    //        SetCheckState(m_SelfTestCheck, CheckState.Failed);
    //    }
    //    else if (cmd == "check-gps-passed")
    //    {
    //        SetCheckState(m_GPSCheck, CheckState.Passed);
    //    }
    //    else if (cmd == "check-gps-failed")
    //    {
    //        SetCheckState(m_GPSCheck, CheckState.Failed);
    //    }
    //}

    //private enum CheckState
    //{
    //    Passed,
    //    Failed,
    //    Checking,
    //    None
    //}
}