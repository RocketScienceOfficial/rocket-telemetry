using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaugesPanelController : MonoBehaviour, IDataRecipient
{
    [SerializeField] private GaugePanelController m_SpeedPanel;
    [SerializeField] private GaugePanelController m_AltitudePanel;

    public void OnSetData(RecipientData data)
    {
        m_SpeedPanel.SetValue(data.velocity * 3.6f, 0f, 2000f);
        m_AltitudePanel.SetValue(data.altitude, 0, 2000);
    }
}