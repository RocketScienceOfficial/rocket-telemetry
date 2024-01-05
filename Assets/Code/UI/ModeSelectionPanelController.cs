using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeSelectionPanelController : MonoBehaviour
{
    [SerializeField] private Button m_TelemetryButton;
    [SerializeField] private Button m_DownloadButton;
    [SerializeField] private Button m_ReplayButton;
    [SerializeField] private Button m_SimulationButton;

    private void Start()
    {
        PanelsManager.Instance.DeactiveAllPanels();
        PanelsManager.Instance.SetPanelActive(Panel.ModeSelection, true);

        m_TelemetryButton.onClick.AddListener(() =>
        {
            FlowManager.SelectFlow(new TelemetryFlow());
        });

        m_DownloadButton.onClick.AddListener(() =>
        {
            FlowManager.SelectFlow(new DownloadFlow());
        });

        m_ReplayButton.onClick.AddListener(() =>
        {
            FlowManager.SelectFlow(new ReplayFlow());
        });

        m_SimulationButton.onClick.AddListener(() =>
        {
            FlowManager.SelectFlow(new SimulationFlow());
        });
    }
}