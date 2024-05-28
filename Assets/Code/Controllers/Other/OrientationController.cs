using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationController : MonoBehaviour, IDataRecipient
{
    public void OnSetData(RecipientData data)
    {
        transform.eulerAngles = new Vector3(data.pitch, data.roll, data.yaw);
    }
}