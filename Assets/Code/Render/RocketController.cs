using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : DataRecipient
{
    private Vector3 _startPos;

    private void Start()
    {
        _startPos = transform.position;
    }

    public override void OnSetData(RecipentData data)
    {
        transform.position = _startPos + new Vector3(data.positionX, data.positionY, data.positionZ);
        transform.eulerAngles = new Vector3(data.pitch, data.roll, data.yaw);
    }
}