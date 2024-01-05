using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayFlow : IFlowController
{
    public void Init()
    {
        PanelsManager.Instance.DeactiveAllPanels();
        PanelsManager.Instance.SetPanelActive(Panel.FileSelection, true);

        FileReader.Instance.OnConnected += (sender, args) =>
        {
            PanelsManager.Instance.DeactiveAllPanels();
            PanelsManager.Instance.SetPanelActive(Panel.Visualization, true);
        };

        DataProvider.SetRecipientType<IReplayDataRecipient>();
        DataProvider.SetDataSupply(FileReader.Instance);
    }
}