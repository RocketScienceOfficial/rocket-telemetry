using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelemetryFlow : IFlowController
{
    public void Init()
    {
        PanelsManager.Instance.DeactiveAllPanels();
        PanelsManager.Instance.SetPanelActive(Panel.PortSelection, true);

        SerialPortController.Instance.OnConnected += (sender, args) =>
        {
            MissionTimerController.Instance.StartMission();

            PanelsManager.Instance.DeactiveAllPanels();
            PanelsManager.Instance.SetPanelActive(Panel.Visualization, true);
        };

        DataProvider.SetRecipientType<ITelemetryDataRecipient>();
        DataProvider.SetDataSupply(SerialPortController.Instance);
    }
}