using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunCamController : MonoBehaviour, ITelemetryDataRecipient, IReplayDataRecipient, ISimulationDataRecipient
{
    private const string RUN_CAM_NAME = "";

    [SerializeField] private RawImage m_Image;

    private bool _init = false;
    private WebCamTexture _texture;

    public void OnSetData(RecipientData data)
    {
        if (!_init)
        {
            var devices = WebCamTexture.devices;
            var valid = false;

            foreach (var device in devices)
            {
                if (device.name == RUN_CAM_NAME)
                {
                    valid = true;

                    break;
                }
            }

            if (!valid)
            {
                return;
            }

            _texture = new WebCamTexture(devices[0].name);
            _texture.Play();
            
            m_Image.texture = _texture;
            m_Image.rectTransform.sizeDelta = new Vector2(m_Image.rectTransform.sizeDelta.x, m_Image.rectTransform.sizeDelta.y * _texture.height / _texture.width);

            _init = true;
        }
    }
}