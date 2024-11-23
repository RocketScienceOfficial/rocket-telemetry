using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationController : MonoBehaviour
{
    private void Start()
    {
        SerialCommunication.Instance.OnRead += (sender, args) =>
        {
            var msg = args.Frame;

            if (msg.msgId == DataLinkMessageType.DATALINK_MESSAGE_TELEMETRY_DATA_GCS)
            {
                var payload = BytesConverter.FromBytes<DataLinkFrameTelemetryDataGCS>(msg.payload);

                transform.localRotation = new Quaternion(payload.qx, payload.qy, payload.qz, payload.qw);
            }
        };
    }
}