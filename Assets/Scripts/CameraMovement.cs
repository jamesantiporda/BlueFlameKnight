using itsSALT.FinalCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Enemy enemy;
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private PlayerLocomotionInput _playerLocomotionInput;
    
    private Camera _playerCamera;

    [Header("Camera Settings")]
    public float lookSenseH = 0.1f;
    public float lookSenseV = 0.1f;
    public float lookLimitV = 70f;
    public float lockSpeed = 5f;

    private Vector2 _cameraRotation = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        _playerCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        UpdateCameraRotation();
    }

    private void UpdateCameraRotation()
    {
        _cameraRotation.x += lookSenseH * _playerLocomotionInput.LookInput.x;
        _cameraRotation.y = Mathf.Clamp(_cameraRotation.y - lookSenseV * _playerLocomotionInput.LookInput.y, -lookLimitV, lookLimitV);

        if(!_playerLocomotionInput.LockToggledOn)
        {
            _playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0f);
        }
        else if(!enemy.IsDead && !player.IsDead)
        {
            Vector3 lockDirection = enemy.transform.position - _playerCamera.transform.position;

            lockDirection.Normalize();

            Quaternion cameraFinalRotation = Quaternion.LookRotation(lockDirection);

            _playerCamera.transform.rotation = cameraFinalRotation; //Quaternion.Lerp(_playerCamera.transform.rotation, cameraFinalRotation, playerModelRotationSpeed * Time.deltaTime);

            Vector3 euler = _playerCamera.transform.rotation.eulerAngles;

            _cameraRotation = new Vector2(euler.y, euler.x);
        }
    }
}
