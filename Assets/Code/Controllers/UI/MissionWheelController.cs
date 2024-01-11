using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionWheelController : MonoBehaviour, ITelemetryDataRecipient, IReplayDataRecipient, ISimulationDataRecipient
{
    private const float MIN_FILL = 0.346f;
    private const float FILL_SPEED = 0.2f;

    [SerializeField] private Image m_Fill;
    [SerializeField] private CheckpointData[] m_Checkpoints;

    private CheckpointData _currentCheckpoint;

    private void Start()
    {
        m_Fill.fillAmount = MIN_FILL;
    }
    private int state = -1;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSetData(new RecipientData { state = ++state });
        }

        if (_currentCheckpoint != null)
        {
            m_Fill.fillAmount += Time.deltaTime * FILL_SPEED;

            if (m_Fill.fillAmount >= _currentCheckpoint.fillThreshold)
            {
                _currentCheckpoint.obj.transform.Find("Background").GetComponent<Image>().color = _currentCheckpoint.obj.GetComponent<Image>().color;
                _currentCheckpoint = null;
            }
        }
    }

    public void OnSetData(RecipientData recipient)
    {
        _currentCheckpoint = m_Checkpoints[recipient.state];
    }


    [Serializable]
    public class CheckpointData
    {
        public GameObject obj;
        public float fillThreshold;
    }
}