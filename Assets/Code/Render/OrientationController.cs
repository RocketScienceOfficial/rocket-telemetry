using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationController : DataRecipient
{
    public override void OnSetData(RecipentData data)
    {
        transform.eulerAngles = new Vector3(data.pitch, data.roll, data.yaw);
    }
}