using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationController : MonoBehaviour
{
    private void Start()
    {
        SerialCommunication.Instance.OnConnected += (sender, args) =>
        {
            transform.localRotation = new Quaternion(0f, 0f, 0f, 1f);
        };

        SerialCommunication.Instance.OnRead += (sender, args) =>
        {
            var msg = args.Frame;

            if (msg.msgId == DataLinkMessageType.DATALINK_MESSAGE_TELEMETRY_DATA_GCS)
            {
                var payload = BytesConverter.FromBytes<DataLinkFrameTelemetryDataGCS>(msg.payload);
                var qw = (float)payload.qw / short.MaxValue;
                var qx = (float)payload.qx / short.MaxValue;
                var qy = (float)payload.qy / short.MaxValue;
                var qz = (float)payload.qz / short.MaxValue;
                
                transform.localRotation = new Quaternion(payload.qx, payload.qy, payload.qz, payload.qw);
            }
        };
    }
}