using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionWheelController : MonoBehaviour, IDataRecipient
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

    private void Update()
    {
        if (_currentCheckpoint != null)
        {
            if (m_Fill.fillAmount >= _currentCheckpoint.fillThreshold)
            {
                _currentCheckpoint.obj.transform.Find("Background").GetComponent<Image>().color = _currentCheckpoint.obj.GetComponent<Image>().color;
                _currentCheckpoint = null;
            }
            else
            {
                m_Fill.fillAmount += Time.deltaTime * FILL_SPEED;
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