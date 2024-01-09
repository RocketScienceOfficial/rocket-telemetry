using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlertManager : MonoBehaviour
{
    private static AlertManager _instance;

    [SerializeField] private GameObject _alertObj;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _alertObj.SetActive(false);
    }

    public static void Alert(string message)
    {
        _instance._alertObj.SetActive(true);
        _instance.transform.Find("Panel/Content Text").GetComponent<TextMeshProUGUI>().SetText(message);
    }
}