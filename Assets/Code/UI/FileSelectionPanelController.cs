using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FileSelectionPanelController : MonoBehaviour
{    
    [SerializeField] private TMP_InputField m_PathInputField;
    [SerializeField] private Button m_SubmitButton;

    private void Start()
    {
        m_SubmitButton.onClick.AddListener(() =>
        {
            var text = m_PathInputField.text;

            if (!string.IsNullOrEmpty(text))
            {
                FileReader.Instance.ReadFile(text);
            }
        });
    }
}