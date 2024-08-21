using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationController : MonoBehaviour, IDataRecipient
{
    private Quaternion _baseQuat;

    private void Start()
    {
        _baseQuat = Quaternion.Euler(90f, 0f, -90f);
    }

    public void OnSetData(RecipientData data)
    {
        transform.rotation = _baseQuat * new Quaternion(data.qx, data.qy, data.qz, data.qw);
    }
}