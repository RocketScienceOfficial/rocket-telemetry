using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeSelectionPanelController : MonoBehaviour
{
    [SerializeField] private Button m_TelemetryButton;
    [SerializeField] private Button m_DataDownloadButton;
    [SerializeField] private Button m_DataVisualizationButton;
    [SerializeField] private Toggle m_DebugModeToggle;

    private void Start()
    {
        m_TelemetryButton.onClick.AddListener(() =>
        {
            PanelsManager.Instance.DeactiveAllPanels();
            PanelsManager.Instance.SetPanelActive(Panel.PortSelection, true);

            AppModeController.SetMode(AppMode.Telemetry);
        });

        m_DataDownloadButton.onClick.AddListener(() =>
        {
            PanelsManager.Instance.DeactiveAllPanels();
            PanelsManager.Instance.SetPanelActive(Panel.PortSelection, true);

            AppModeController.SetMode(AppMode.DataDownload);
        });

        m_DataVisualizationButton.onClick.AddListener(() =>
        {
            PanelsManager.Instance.DeactiveAllPanels();
            PanelsManager.Instance.SetPanelActive(Panel.DataVisualizationPicker, true);

            AppModeController.SetMode(AppMode.DataVisualization);
        });

        m_DebugModeToggle.isOn = AppModeController.IsDebugMode;
        m_DebugModeToggle.onValueChanged.AddListener(val => AppModeController.IsDebugMode = val);
    }
}