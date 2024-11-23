using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowManager : MonoBehaviour
{
    public void Start()
    {
        PanelsManager.Instance.DeactiveAllPanels();
        PanelsManager.Instance.SetPanelActive(Panel.PortSelection, true);

        SerialCommunication.Instance.OnConnected += (sender, args) =>
        {
            MissionTimerController.Instance.StartMission();

            PanelsManager.Instance.DeactiveAllPanels();
            PanelsManager.Instance.SetPanelActive(Panel.Visualization, true);
        };

        SerialCommunication.Instance.OnDisconnected += (sender, args) =>
        {
            PanelsManager.Instance.DeactiveAllPanels();
            PanelsManager.Instance.SetPanelActive(Panel.PortSelection, true);
        };
    }
}