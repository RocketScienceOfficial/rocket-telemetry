using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MissionConfigPanelController : MonoBehaviour, ICommandRecipient
{
    public static MissionConfigPanelController Instance { get; private set; }

    [SerializeField] private GameObject m_MainParachuteHeightConfig;
    [SerializeField] private GameObject m_LauncherHeightConfig;
    [SerializeField] private GameObject m_SecondIgniterDelayConfig;
    [SerializeField] private GameObject m_ParachuteErrorSpeedConfig;
    [SerializeField] private Button m_SaveButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //m_SaveButton.onClick.AddListener(() =>
        //{
        //    var mainParachuteHeight = int.TryParse(m_MainParachuteHeightConfig.GetComponentInChildren<InputField>().text, out var res1) ? res1 : 0;
        //    var launcherHeight = int.TryParse(m_LauncherHeightConfig.GetComponentInChildren<InputField>().text, out var res2) ? res2 : 0;
        //    var secondIgniterDelay = int.TryParse(m_SecondIgniterDelayConfig.GetComponentInChildren<InputField>().text, out var res3) ? res3 : 0;
        //    var parachuteErrorSpeed = int.TryParse(m_ParachuteErrorSpeedConfig.GetComponentInChildren<InputField>().text, out var res4) ? res4 : 0;

        //    if (mainParachuteHeight > 0 && launcherHeight > 0 && secondIgniterDelay > 0 && parachuteErrorSpeed > 0)
        //    {
        //        SerialPortController.Instance.SerialPortWrite($"config-write {mainParachuteHeight},{launcherHeight},{secondIgniterDelay},{parachuteErrorSpeed}");

        //        m_SaveButton.gameObject.SetActive(false);
        //    }
        //});
    }

    private void SetConfigActivation(GameObject config, bool activation)
    {
        //config.GetComponentInChildren<InputField>().interactable = activation;
    }

    private void SetConfigValue(GameObject config, int value)
    {
        //config.GetComponentInChildren<InputField>().text = value.ToString();
    }

    public void FetchData()
    {
        //SerialPortController.Instance.SerialPortWrite("config-get");

        //SetConfigActivation(m_MainParachuteHeightConfig, false);
        //SetConfigActivation(m_LauncherHeightConfig, false);
        //SetConfigActivation(m_SecondIgniterDelayConfig, false);
        //SetConfigActivation(m_ParachuteErrorSpeedConfig, false);
    }

    public void OnSetCommand(string cmd)
    {
        //if (cmd.Contains("config-read"))
        //{
        //    var txt = cmd.Replace("config-read ", "");
        //    var data = txt.Split(',').Select(d => int.TryParse(d, out var res) ? res : 0).ToList();

        //    SetConfigActivation(m_MainParachuteHeightConfig, true);
        //    SetConfigActivation(m_LauncherHeightConfig, true);
        //    SetConfigActivation(m_SecondIgniterDelayConfig, true);
        //    SetConfigActivation(m_ParachuteErrorSpeedConfig, true);

        //    SetConfigValue(m_MainParachuteHeightConfig, data[0]);
        //    SetConfigValue(m_LauncherHeightConfig, data[1]);
        //    SetConfigValue(m_SecondIgniterDelayConfig, data[2]);
        //    SetConfigValue(m_ParachuteErrorSpeedConfig, data[3]);
        //}
    }
}