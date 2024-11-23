using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionWheelController : MonoBehaviour
{
    private const float MIN_FILL = 0.346f;
    private const float FILL_SPEED = 0.2f;
    private const float MAX_DELTA_TIME = 0.05f;

    [SerializeField] private Image m_Fill;
    [SerializeField] private CheckpointData[] m_Checkpoints;

    private int _checkingIndex = 0;
    private int _currentCheckpointIndex = -1;


    private void Start()
    {
        m_Fill.fillAmount = MIN_FILL;

        SerialCommunication.Instance.OnRead += (sender, args) =>
        {
            var msg = args.Frame;

            if (msg.msgId == DataLinkMessageType.DATALINK_MESSAGE_TELEMETRY_DATA_GCS)
            {
                var payload = BytesConverter.FromBytes<DataLinkFrameTelemetryDataGCS>(msg.payload);

                _currentCheckpointIndex = Mathf.Clamp(payload.state, 0, m_Checkpoints.Length - 1);
            }
        };
    }

    private void Update()
    {
        if (_currentCheckpointIndex != -1)
        {
            if (m_Fill.fillAmount < m_Checkpoints[_currentCheckpointIndex].fillThreshold && Time.deltaTime < MAX_DELTA_TIME)
            {
                m_Fill.fillAmount += Time.deltaTime * FILL_SPEED;
            }

            if (_checkingIndex < m_Checkpoints.Length)
            {
                var current = m_Checkpoints[_checkingIndex];

                if (m_Fill.fillAmount >= current.fillThreshold)
                {
                    current.obj.transform.Find("Background").GetComponent<Image>().color = current.obj.GetComponent<Image>().color;

                    _checkingIndex++;
                }
            }
        }
    }


    [Serializable]
    public class CheckpointData
    {
        public GameObject obj;
        public float fillThreshold;
    }
}