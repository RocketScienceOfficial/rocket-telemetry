using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GaugePanelController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_ValueText;
    [SerializeField] private Image m_FillImage;

    private float _maxFill;
    private bool _initialized;

    private void Init()
    {
        if (!_initialized)
        {
            _maxFill = transform.Find("Red Background").GetComponent<Image>().fillAmount;

            _initialized = true;
        }
    }

    public void SetValue(float value, float minValue, float maxValue)
    {
        Init();

        if (value < minValue || value > maxValue)
        {
            return;
        }

        m_ValueText.SetText(MathUtils.NumberOneDecimalPlace(value));
        m_FillImage.fillAmount = (value - minValue) / (maxValue - minValue) * _maxFill;
    }

    public void SetValue(int value, int minValue, int maxValue)
    {
        Init();

        if (value < minValue || value > maxValue)
        {
            return;
        }

        m_ValueText.SetText(value.ToString());
        m_FillImage.fillAmount = (float)(value - minValue) / (maxValue - minValue) * _maxFill;
    }
}