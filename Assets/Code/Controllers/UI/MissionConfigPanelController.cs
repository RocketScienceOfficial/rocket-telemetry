using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionConfigPanelController : MonoBehaviour
{
    [SerializeField] private GameObject m_MissionNameConfig;
    [SerializeField] private GameObject m_MainParachuteHeightConfig;
    [SerializeField] private GameObject m_LauncherHeightConfig;
    [SerializeField] private GameObject m_SecondIgniterDelayConfig;
    [SerializeField] private GameObject m_ParachuteErrorSpeedConfig;
    [SerializeField] private Button m_StartButton;

    private void Start()
    {
        m_StartButton.enabled = false;
        m_StartButton.onClick.AddListener(() =>
        {
            if (IsValid())
            {
                PanelsManager.Instance.DeactiveAllPanels();
                PanelsManager.Instance.SetPanelActive(Panel.Visualization, true);

                MissionTimerController.Instance.SetData(GetMissionName());
            }
        });
    }

    private string GetMissionName()
    {
        return m_MissionNameConfig.GetComponentInChildren<InputField>().text;
    }

    private int GetMainParachuteHeight()
    {
        return int.TryParse(m_MainParachuteHeightConfig.GetComponentInChildren<InputField>().text, out var res) ? res : 0;
    }

    private int GetLauncherHeight()
    {
        return int.TryParse(m_LauncherHeightConfig.GetComponentInChildren<InputField>().text, out var res) ? res : 0;
    }

    private float GetSecondIgniterDelay()
    {
        return float.TryParse(m_SecondIgniterDelayConfig.GetComponentInChildren<InputField>().text, out var res) ? res : 0;
    }

    private int GetParachuteErrorSpeed()
    {
        return int.TryParse(m_ParachuteErrorSpeedConfig.GetComponentInChildren<InputField>().text, out var res) ? res : 0;
    }

    private bool IsValid()
    {
        return !string.IsNullOrEmpty(GetMissionName()) && GetMainParachuteHeight() > 0 && GetLauncherHeight() > 0 && GetSecondIgniterDelay() > 0 && GetParachuteErrorSpeed() > 0;
    }
}