using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour, ITelemetryDataRecipient, IReplayDataRecipient, ISimulationDataRecipient
{
    private Vector3 _startPos;

    private void Start()
    {
        _startPos = transform.position;
    }

    public void OnSetData(RecipientData data)
    {
        transform.position = _startPos + new Vector3(data.positionX, data.positionY, data.positionZ);
        transform.eulerAngles = new Vector3(data.pitch, data.roll, data.yaw);
    }
}