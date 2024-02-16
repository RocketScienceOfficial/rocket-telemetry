using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataDownloadPanelController : MonoBehaviour, ICommandRecipient
{
    public static DataDownloadPanelController Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI m_PortText;
    [SerializeField] private TextMeshProUGUI m_StatusText;

    private void Awake()
    {
        Instance = this;
    }

    private void SetStatus(bool done)
    {
        m_PortText.SetText(SerialPortController.Instance.GetPath());
        m_StatusText.SetText(done ? "DONE!" : "Downloading...");
    }

    public void OnSetCommand(string command)
    {
        if (command == "data-end")
        {
            SetStatus(true);
        }
    }

    public void StartDownload()
    {
        SetStatus(false);

        SerialPortController.Instance.SerialPortWrite("start-download");
    }
}