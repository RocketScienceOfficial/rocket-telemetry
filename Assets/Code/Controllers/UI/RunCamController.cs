using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunCamController : MonoBehaviour, IDataRecipient
{
    private const string RUN_CAM_NAME = "HD Pro Webcam C920";

    [SerializeField] private RawImage m_Image;

    private bool _init = false;
    private WebCamTexture _texture;

    public void OnSetData(RecipientData data)
    {
        if (!_init)
        {
            if (CameraExists())
            {
                _texture = new WebCamTexture(RUN_CAM_NAME);
                _texture.Play();

                m_Image.color = Color.white;
                m_Image.texture = _texture;
                m_Image.rectTransform.sizeDelta = new Vector2(m_Image.rectTransform.sizeDelta.y * _texture.width / _texture.height, m_Image.rectTransform.sizeDelta.y);
            }

            _init = true;
        }
    }

    private bool CameraExists()
    {
        var devices = WebCamTexture.devices;

        foreach (var device in devices)
        {
            if (device.name == RUN_CAM_NAME)
            {
                return true;
            }
        }

        return false;
    }
}