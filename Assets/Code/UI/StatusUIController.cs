using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_PortText;
    [SerializeField] private TextMeshProUGUI m_StatusText;

    private void Start()
    {
        SerialPortController.Instance.OnConnected += (sender, args) =>
        {
            SetConnected(SerialPortController.Instance.GetPath(), false);
        };

        SerialPortController.Instance.OnDisconnected += (sender, args) =>
        {
            SetDisconnected();
        };

        FileReader.Instance.OnConnected += (sender, args) =>
        {
            SetConnected(FileReader.Instance.GetPath(), true);
        };

        FileReader.Instance.OnDisconnected += (sender, args) =>
        {
            SetDisconnected();
        };
    }

    private void SetConnected(string path, bool isFile)
    {
        m_PortText.SetText(path);
        m_StatusText.SetText(!isFile ? "Connected" : "File");
    }

    private void SetDisconnected()
    {
        m_PortText.SetText("None");
        m_StatusText.SetText("Disconnected");
    }
}