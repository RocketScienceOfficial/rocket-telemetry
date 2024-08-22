using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationController : MonoBehaviour, IDataRecipient
{
    public void OnSetData(RecipientData data)
    {
        transform.localRotation = new Quaternion(data.qx, data.qy, data.qz, data.qw);
    }
}