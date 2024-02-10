using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationFlow : IFlowController
{
    public void Init()
    {
        //PanelsManager.Instance.DeactiveAllPanels();
        //PanelsManager.Instance.SetPanelActive(Panel.FileSelection, true);

        //FileReader.Instance.OnConnected += (sender, args) =>
        //{
        //    PanelsManager.Instance.DeactiveAllPanels();
        //    PanelsManager.Instance.SetPanelActive(Panel.PortSelection, true);

        //    SerialPortController.Instance.OnConnected += (sender, args) =>
        //    {
        //        SerialPortController.Instance.SerialPortWrite("app-mode-simulation");

        //        PanelsManager.Instance.DeactiveAllPanels();
        //        PanelsManager.Instance.SetPanelActive(Panel.Visualization, true);
        //    };
        //};

        //DataProvider.SetRecipientType<ISimulationDataRecipient>();
        //DataProvider.SetDataSupply(SerialPortController.Instance);
    }
}