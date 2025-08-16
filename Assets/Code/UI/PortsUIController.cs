using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PortsUIController : MonoBehaviour
{
    [SerializeField] private GameObject m_LoadingPanel;
    [SerializeField] private GameObject m_PortSelectPanel;
    [SerializeField] private Transform m_Parent;
    [SerializeField] private Button m_RefreshButton;

    private void Start()
    {
        SerialCommunication.Instance.OnConnected += (sender, args) =>
        {
            FetchPorts();
        };

        SerialCommunication.Instance.OnDisconnected += (sender, args) =>
        {
            FetchPorts();
        };

        m_RefreshButton.onClick.AddListener(() => FetchPorts());

        FetchPorts();
    }

    private void FetchPorts()
    {
        m_LoadingPanel.SetActive(true);

        Clear();

        foreach (var port in SerialPort.GetPortNames())
        {
            SetupMicrocontroller(port);
        }

        m_LoadingPanel.SetActive(false);
    }

    private void Clear()
    {
        for (int i = 0; i < m_Parent.childCount; i++)
        {
            Destroy(m_Parent.GetChild(i).gameObject);
        }
    }

    private void SetupMicrocontroller(string path)
    {
        var obj = Instantiate(m_PortSelectPanel, m_Parent);

        obj.GetComponentInChildren<TextMeshProUGUI>().SetText(path);
        obj.GetComponent<Button>().onClick.AddListener(() =>
        {
            m_LoadingPanel.SetActive(true);

            SerialCommunication.Instance.Connect(path);
        });
    }
}