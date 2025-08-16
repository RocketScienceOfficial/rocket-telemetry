using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SMController : MonoBehaviour
{
    [SerializeField] private Image m_Fill;
    [SerializeField] private CheckpointData[] m_Checkpoints;

    private bool _isActive = false;
    private float _timer;
    private CheckpointInfo[] _checkpointInfos;
    private Color _inactiveColor;

    private void Start()
    {
        _checkpointInfos = new CheckpointInfo[m_Checkpoints.Length];
        _inactiveColor = m_Checkpoints[0].obj.GetComponent<Image>().color;

        SerialCommunication.Instance.OnConnected += (sender, args) =>
        {
            m_Fill.fillAmount = 0f;

            for (int i = 0; i < _checkpointInfos.Length; i++)
            {
                _checkpointInfos[i].isActive = false;

                m_Checkpoints[i].obj.GetComponent<Image>().color = _inactiveColor;

                UpdateCheckpointText(i, 0f);
            }

            SetCheckpointActive(0);

            _isActive = true;
            _timer = 0f;
        };

        SerialCommunication.Instance.OnDisconnected += (sender, args) =>
        {
            _isActive = false;
        };

        SerialCommunication.Instance.OnRead += (sender, args) =>
        {
            var msg = args.Frame;

            if (msg.msgId == DataLinkMessageType.DATALINK_MESSAGE_TELEMETRY_DATA_GCS)
            {
                var payload = BytesConverter.FromBytes<DataLinkFrameTelemetryDataGCS>(msg.payload);

                SetCheckpointActive(Mathf.Clamp(payload.state, 0, m_Checkpoints.Length - 1));
            }
        };
    }

    private void Update()
    {
        if (_isActive)
        {
            _timer += Time.deltaTime;

            for (int i = 0; i < _checkpointInfos.Length; i++)
            {
                if (!_checkpointInfos[i].isActive)
                {
                    UpdateCheckpointText(i, _timer);
                }
            }
        }
    }

    private void SetCheckpointActive(int index)
    {
        if (!_checkpointInfos[index].isActive)
        {
            m_Checkpoints[index].obj.GetComponent<Image>().color = m_Checkpoints[index].obj.transform.Find("Filling Image").GetComponent<Image>().color;

            _checkpointInfos[index].isActive = true;

            m_Fill.fillAmount = (float)index / (_checkpointInfos.Length - 1);
        }
    }

    private void UpdateCheckpointText(int index, float time)
    {
        m_Checkpoints[index].timerText.SetText("+" + string.Format("{0:0.0}", time).Replace(',', '.'));
    }


    [Serializable]
    public struct CheckpointData
    {
        public GameObject obj;
        public TextMeshProUGUI timerText;
    }

    public struct CheckpointInfo
    {
        public bool isActive;
    }
}