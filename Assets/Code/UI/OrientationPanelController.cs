using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrientationPanelController : DataRecipient
{
    private const char DEGREE_SIGN = '°';

    [SerializeField] private TextMeshProUGUI m_RollText;
    [SerializeField] private TextMeshProUGUI m_PitchText;
    [SerializeField] private TextMeshProUGUI m_YawText;

    public override void OnSetData(RecipentData data)
    {
        if (AppModeController.CurrentMode == AppMode.DataDownload)
        {
            return;
        }

        m_RollText.SetText("Roll: " + Mathf.RoundToInt(data.roll).ToString() + DEGREE_SIGN);
        m_PitchText.SetText("Pitch: " + Mathf.RoundToInt(data.pitch).ToString() + DEGREE_SIGN);
        m_YawText.SetText("Yaw: " + Mathf.RoundToInt(data.yaw).ToString() + DEGREE_SIGN);
    }
}