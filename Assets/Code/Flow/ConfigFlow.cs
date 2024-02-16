using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigFlow : IFlowController
{
    public void Init()
    {
        //PanelsManager.Instance.DeactiveAllPanels();
        //PanelsManager.Instance.SetPanelActive(Panel.PortSelection, true);

        //SerialPortController.Instance.OnConnected += (sender, args) =>
        //{
        //    MissionConfigPanelController.Instance.FetchData();

        //    PanelsManager.Instance.DeactiveAllPanels();
        //    PanelsManager.Instance.SetPanelActive(Panel.ConfigPanel, true);
        //};

        //DataProvider.SetRecipientType<IConfigDataRecipient>();
        //DataProvider.SetDataSupply(SerialPortController.Instance);
    }
}