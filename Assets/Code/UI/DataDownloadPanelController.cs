using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataDownloadPanelController : DataRecipient
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

        SerialPortController.Instance.SerialPortWrite("read-data\r");
    }

    public override void OnSetCommand(string command)
    {
        if (command == "read-done")
        {
            SetStatus(true);
        }
    }
}