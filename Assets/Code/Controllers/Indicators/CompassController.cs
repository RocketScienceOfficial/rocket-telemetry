using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompassController : MonoBehaviour
{
    private const char DEGREE_SIGN = '°';

    [SerializeField] private RectTransform m_ArrowPivot;
    [SerializeField] private TextMeshProUGUI m_AngleText;

    public void SetAngle(float angle)
    {
        m_ArrowPivot.eulerAngles = new Vector3(0f, 0f, angle);
        m_AngleText.SetText(angle.ToString() + DEGREE_SIGN);
    }
}