using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Panel
{
    Visualization,
    DataDownload,
    FileSelection,
    PortSelection,
    ModeSelection,
}

public class PanelsManager : MonoBehaviour
{
    public static PanelsManager Instance { get; private set; }

    [SerializeField] private GameObject m_VisualizationPanel;
    [SerializeField] private GameObject m_DataDownloadPanel;
    [SerializeField] private GameObject m_FileSelecrtionPanel;
    [SerializeField] private GameObject m_PortSelectionPanel;
    [SerializeField] private GameObject m_ModeSelectionPanel;

    private Dictionary<Panel, GameObject> m_Panels;

    private void Awake()
    {
        Instance = this;

        m_Panels = new()
        {
            { Panel.Visualization, m_VisualizationPanel },
            { Panel.DataDownload, m_DataDownloadPanel },
            { Panel.FileSelection, m_FileSelecrtionPanel },
            { Panel.PortSelection, m_PortSelectionPanel },
            { Panel.ModeSelection, m_ModeSelectionPanel },
        };
    }

    public void SetPanelActive(Panel panel, bool active)
    {
        m_Panels[panel].SetActive(active);
    }

    public void DeactiveAllPanels()
    {
        foreach (var panel in Enum.GetValues(typeof(Panel)))
        {
            SetPanelActive((Panel)panel, false);
        }
    }
}