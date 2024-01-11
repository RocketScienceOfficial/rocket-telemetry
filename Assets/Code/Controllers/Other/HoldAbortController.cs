using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoldAbortController : MonoBehaviour
{
    private const float HOLD_THRESHOLD = 0.2f;
    private const float ABORT_THRESHOLD = 2f;

    public static event EventHandler OnHold;
    public static event EventHandler OnUnhold;
    public static event EventHandler OnAbort;

    [SerializeField] private TextMeshProUGUI m_HoldText;

    private bool _isHold;
    private float _startTime;
    private bool _enabled;

    private void Start()
    {
        SerialPortController.Instance.OnConnected += (s, a) =>
        {
            _enabled = true;
        };
    }

    private void Update()
    {
        if (_enabled)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _startTime = Time.time;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                var diff = Time.time - _startTime;

                if (diff <= HOLD_THRESHOLD)
                {
                    Hold();
                }
                else if (diff >= ABORT_THRESHOLD)
                {
                    Abort();
                }
            }
        }
    }

    private void Hold()
    {
        if (!_isHold)
        {
            OnHold?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnUnhold?.Invoke(this, EventArgs.Empty);
        }

        m_HoldText.gameObject.SetActive(_isHold);
    }

    public static void Abort()
    {
        OnAbort?.Invoke(null, EventArgs.Empty);
    }
}