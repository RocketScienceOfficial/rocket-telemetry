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
        AppModeController.OnSetMode += (sender, args) =>
        {
            AppModeController.CurrentDataSupply.OnConnected += (sender, args) =>
            {
                UpdatePortUI();
            };

            AppModeController.CurrentDataSupply.OnDisconnected += (sender, args) =>
            {
                UpdatePortUI();
            };
        };
    }

    private void UpdatePortUI()
    {
        if (AppModeController.CurrentDataSupply.IsConnected())
        {
            m_PortText.SetText(AppModeController.CurrentDataSupply.GetPath());

            switch (AppModeController.CurrentMode)
            {
                case AppMode.Telemetry:
                    m_StatusText.SetText("Connected");
                    break;
                case AppMode.DataVisualization:
                    m_StatusText.SetText("File");
                    break;
                default:
                    break;
            }
        }
        else
        {
            m_PortText.SetText("None");
            m_StatusText.SetText("Disconnected");
        }
    }
}