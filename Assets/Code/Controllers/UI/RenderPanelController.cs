using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderPanelController : MonoBehaviour, ITelemetryDataRecipient, IReplayDataRecipient, ISimulationDataRecipient
{
    [SerializeField] private GaugePanelController m_VelocityPanel;
    [SerializeField] private GaugePanelController m_AltitudePanel;
    [SerializeField] private GaugePanelController m_OverloadPanel;
    [SerializeField] private CompassController m_Compass;

    public void OnSetData(RecipientData data)
    {
        m_VelocityPanel.SetValue(data.velocity, 0f, 1000f);
        m_AltitudePanel.SetValue(data.altitude, 0f, 5000f);
        m_OverloadPanel.SetValue(data.acceleration / 9.81f, 0f, 50f);
        m_Compass.SetAngle(data.roll);
    }
}