using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour, ITelemetryDataRecipient, IReplayDataRecipient, ISimulationDataRecipient
{
    private const float MAX_Y_ANGLE = 80f;

    [SerializeField] private float m_CameraSpeed;
    [SerializeField] private float m_CameraSensitivity;
    [SerializeField] private float m_ShiftSpeedMultiplier;
    [SerializeField] private float m_SpeedRate;

    private bool _movementEnabled;
    private Vector3 _currentSpeed;
    private Vector3 _rotatedSpeed;
    private bool _isPressingKey;
    private bool _isShiftPressed;
    private Vector3 _speedOffset;
    private Vector3 _dataOffset;
    private Vector2 _currentRotation;
    private Quaternion _currentRotationQuaternion;
    private Vector3 _startPos;

    private void Start()
    {
        _startPos = transform.position;
    }

    private void Update()
    {
        if (!_movementEnabled)
        {
            //return;
        }

        _isPressingKey = false;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _isShiftPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isShiftPressed = false;
        }

        if (Input.GetKey(KeyCode.W))
        {
            _currentSpeed.z += m_SpeedRate * Time.deltaTime;
            _currentSpeed.z = Mathf.Clamp(_currentSpeed.z, -GetMaxSpeed(), GetMaxSpeed());
            _isPressingKey = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _currentSpeed.z -= m_SpeedRate * Time.deltaTime;
            _currentSpeed.z = Mathf.Clamp(_currentSpeed.z, -GetMaxSpeed(), GetMaxSpeed());
            _isPressingKey = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _currentSpeed.x -= m_SpeedRate * Time.deltaTime;
            _currentSpeed.x = Mathf.Clamp(_currentSpeed.x, -GetMaxSpeed(), GetMaxSpeed());
            _isPressingKey = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _currentSpeed.x += m_SpeedRate * Time.deltaTime;
            _currentSpeed.x = Mathf.Clamp(_currentSpeed.x, -GetMaxSpeed(), GetMaxSpeed());
            _isPressingKey = true;
        }
        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Space))
        {
            _currentSpeed.y += m_SpeedRate * Time.deltaTime;
            _currentSpeed.y = Mathf.Clamp(_currentSpeed.y, -GetMaxSpeed(), GetMaxSpeed());
            _isPressingKey = true;
        }
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftControl))
        {
            _currentSpeed.y -= m_SpeedRate * Time.deltaTime;
            _currentSpeed.y = Mathf.Clamp(_currentSpeed.y, -GetMaxSpeed(), GetMaxSpeed());
            _isPressingKey = true;
        }

        _currentSpeed.x -= !_isPressingKey && _currentSpeed.x != 0f ? Mathf.Sign(_currentSpeed.x) * m_SpeedRate * Time.deltaTime : 0f;
        _currentSpeed.x = Mathf.Clamp(_currentSpeed.x, -GetMaxSpeed(), GetMaxSpeed());
        _currentSpeed.y -= !_isPressingKey && _currentSpeed.y != 0f ? Mathf.Sign(_currentSpeed.y) * m_SpeedRate * Time.deltaTime : 0f;
        _currentSpeed.y = Mathf.Clamp(_currentSpeed.y, -GetMaxSpeed(), GetMaxSpeed());
        _currentSpeed.z -= !_isPressingKey && _currentSpeed.z != 0f ? Mathf.Sign(_currentSpeed.z) * m_SpeedRate * Time.deltaTime : 0f;
        _currentSpeed.z = Mathf.Clamp(_currentSpeed.z, -GetMaxSpeed(), GetMaxSpeed());

        if (Mathf.Abs(_currentSpeed.x) < 0.001f)
        {
            _currentSpeed.x = 0f;
        }
        if (Mathf.Abs(_currentSpeed.y) < 0.001f)
        {
            _currentSpeed.y = 0f;
        }
        if (Mathf.Abs(_currentSpeed.z) < 0.001f)
        {
            _currentSpeed.z = 0f;
        }

        _rotatedSpeed = _currentSpeed;

        if (Input.GetKey(KeyCode.R))
        {
            _currentSpeed = Vector3.zero;
            _speedOffset = Vector3.zero;
            _currentRotation = Vector2.zero;

            transform.position = _startPos + _speedOffset + _dataOffset;
            transform.eulerAngles = new Vector3(_currentRotation.y, _currentRotation.x, 0);
        }

        if (Input.GetMouseButton(0))
        {
            _currentRotation.x += Input.GetAxis("Mouse X") * m_CameraSensitivity;
            _currentRotation.y -= Input.GetAxis("Mouse Y") * m_CameraSensitivity;
            _currentRotation.x = Mathf.Repeat(_currentRotation.x, 360);
            _currentRotation.y = Mathf.Clamp(_currentRotation.y, -MAX_Y_ANGLE, MAX_Y_ANGLE);

            _currentRotationQuaternion = Quaternion.Euler(_currentRotation.y, _currentRotation.x, 0f);
            _rotatedSpeed = _currentRotationQuaternion * _currentSpeed;

            transform.rotation = _currentRotationQuaternion;
        }

        _speedOffset += _rotatedSpeed * Time.deltaTime;

        transform.position = _startPos + _speedOffset + _dataOffset;
    }

    public void OnSetData(RecipientData data)
    {
        _movementEnabled = true;
        _dataOffset = new Vector3(data.positionX, data.positionY, data.positionZ);

        transform.position = _startPos + _speedOffset + _dataOffset;
    }

    private float GetMaxSpeed()
    {
        return m_CameraSpeed * (_isShiftPressed ? m_ShiftSpeedMultiplier : 1f);
    }
}