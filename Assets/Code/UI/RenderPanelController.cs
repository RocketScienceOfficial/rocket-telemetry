using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderPanelController : DataRecipient
{
    [SerializeField] private GaugePanelController m_VelocityPanel;
    [SerializeField] private GaugePanelController m_AltitudePanel;
    [SerializeField] private CompassUIController m_Compass;

    public override void OnSetData(RecipentData data)
    {
        if (AppModeController.CurrentMode == AppMode.DataDownload)
        {
            return;
        }

        m_VelocityPanel.SetValue(data.velocity, 0f, 1000f);
        m_AltitudePanel.SetValue(data.altitude, 0f, 5000f);
        m_Compass.SetAngle(data.roll);
    }
}