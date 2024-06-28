using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabsManager : MonoBehaviour
{
    [SerializeField] private TabData[] m_Tabs;
    [SerializeField] private Color m_NormalColor;
    [SerializeField] private Color m_SelectedColor;
    [SerializeField] private int m_DefaultIndex;

    private void Start()
    {
        for (var i = 0; i < m_Tabs.Length; i++)
        {
            var index = i;
            m_Tabs[i].button.onClick.AddListener(() => SetActiveTab(index));
        }

        SetActiveTab(m_DefaultIndex);
    }

    private void SetActiveTab(int index)
    {
        foreach (var tab in m_Tabs)
        {
            tab.button.enabled = true;
            tab.button.GetComponent<Image>().color = m_NormalColor;
            tab.panel.SetActive(false);
        }

        m_Tabs[index].button.enabled = true;
        m_Tabs[index].button.GetComponent<Image>().color = m_SelectedColor;
        m_Tabs[index].panel.SetActive(true);
    }


    [Serializable]
    public struct TabData
    {
        public Button button;
        public GameObject panel;
    }
}