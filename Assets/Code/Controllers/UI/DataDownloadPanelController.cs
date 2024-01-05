using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataDownloadPanelController : MonoBehaviour, ICommandRecipient
{
    [SerializeField] private TextMeshProUGUI m_PortText;
    [SerializeField] private TextMeshProUGUI m_StatusText;

    private void SetStatus(bool done)
    {
        m_PortText.SetText(SerialPortController.Instance.GetPath());
        m_StatusText.SetText(done ? "DONE!" : "Downloading...");
    }

    public void StartDownload()
    {
        SetStatus(false);

        SerialPortController.Instance.SerialPortWrite("data-read\r");
    }

    public void OnSetCommand(string command)
    {
        if (command == "data-end")
        {
            SetStatus(true);
        }
    }
}