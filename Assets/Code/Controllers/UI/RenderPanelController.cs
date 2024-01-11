using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderPanelController : MonoBehaviour, ITelemetryDataRecipient, IReplayDataRecipient, ISimulationDataRecipient
{
    [SerializeField] private GaugePanelController m_SpeedPanel;
    [SerializeField] private GaugePanelController m_AltitudePanel;

    public void OnSetData(RecipientData data)
    {
        m_SpeedPanel.SetValue(data.speed * 3.6f, 0f, 1500f);
        m_AltitudePanel.SetValue(data.altitude / 1000f, 0f, 5f);
    }
}