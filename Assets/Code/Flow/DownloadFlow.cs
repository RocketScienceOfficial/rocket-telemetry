using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadFlow : IFlowController
{
    public void Init()
    {
        PanelsManager.Instance.DeactiveAllPanels();
        PanelsManager.Instance.SetPanelActive(Panel.PortSelection, true);

        SerialPortController.Instance.OnConnected += (sender, args) =>
        {
            SerialPortController.Instance.SerialPortWrite("app-mode-download");

            PanelsManager.Instance.DeactiveAllPanels();
            PanelsManager.Instance.SetPanelActive(Panel.DataDownload, true);
        };

        DataProvider.SetRecipientType<IDownloadDataRecipient>();
        DataProvider.SetDataSupply(SerialPortController.Instance);
    }
}