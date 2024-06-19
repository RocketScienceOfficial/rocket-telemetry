using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionTimerController : MonoBehaviour
{
    private const int MISSION_START_TIME = 0;

    public static MissionTimerController Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI m_MissionTimerText;
    [SerializeField] private TextMeshProUGUI m_MissionNameText;

    private bool _running;
    private float _currentTimer;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (_running)
        {
            _currentTimer -= Time.deltaTime;

            UpdateTimer();
        }
    }

    public void StartMission()
    {
        _running = true;
        _currentTimer = MISSION_START_TIME;
    }

    private void UpdateTimer()
    {
        var tmpTimer = Mathf.Abs(_currentTimer);
        var hours = Mathf.FloorToInt(tmpTimer / 3600f);
        var minutes = Mathf.FloorToInt(tmpTimer % 3600f / 60f);
        var seconds = Mathf.FloorToInt(tmpTimer - hours * 3600f - minutes * 60f);

        m_MissionTimerText.SetText("T" + (_currentTimer >= 0f ? "+" : "-") + (hours <= 9 ? "0" + hours : hours) + ":" + (minutes <= 9 ? "0" + minutes : minutes) + ":" + (seconds <= 9 ? "0" + seconds : seconds));
    }
}