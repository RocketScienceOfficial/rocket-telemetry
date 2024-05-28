using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Panel
{
    Visualization,
    PortSelection,
}

public class PanelsManager : MonoBehaviour
{
    public static PanelsManager Instance { get; private set; }

    [SerializeField] private GameObject m_VisualizationPanel;
    [SerializeField] private GameObject m_PortSelectionPanel;

    private Dictionary<Panel, GameObject> m_Panels;

    private void Awake()
    {
        Instance = this;

        m_Panels = new()
        {
            { Panel.Visualization, m_VisualizationPanel },
            { Panel.PortSelection, m_PortSelectionPanel },
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