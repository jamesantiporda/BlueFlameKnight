using Cinemachine;
using itsSALT.FinalCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private PlayerLocomotionInput _playerLocomotionInput;
    [SerializeField]
    private PlayerState _playerState;
    [SerializeField]
    private CinemachineVirtualCamera cinemachineCam;
    [SerializeField]
    private FieldOfView fov;

    private Camera _playerCamera;
    private Enemy enemy;

    [Header("Camera Settings")]
    public float lookSenseH = 0.1f;
    public float lookSenseV = 0.1f;
    public float lookLimitV = 70f;
    public float lockSpeed = 5f;

    [Header("Lock On Settings")]
    public float cameraLockRotationSpeed = 10f;
    public float RotationMismatch { get; private set; } = 0f;

    private Vector2 _cameraRotation = Vector2.zero;

    private bool IsRotatingToTarget = false;
    private float _rotatingToTargetTimer = 0f;
    public float rotateToTargetTime = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        _playerCamera = GetComponent<Camera>();
        enemy = null;
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

        float rotationTolerance = 90f;

        IsRotatingToTarget = _rotatingToTargetTimer > 0;

        if (!_playerLocomotionInput.LockToggledOn)
        {
            //enemy = null;

            enemy = null;

            _playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0f);
        }
        else if(fov.targetObject != null && !player.IsDead) // && _rotatingToTargetTimer > 0)
        {
            //enemy = fov.targetObject;

            ComponentToEnemy compToEnemy = fov.targetObject.GetComponent<ComponentToEnemy>();

            enemy = compToEnemy.enemy;

            if(!enemy.IsDead)
            {
                Vector3 lockDirection = enemy.LockOnPoint.position - _playerCamera.transform.position;

                lockDirection.Normalize();

                Quaternion cameraFinalRotation = Quaternion.LookRotation(lockDirection);

                _playerCamera.transform.rotation = cameraFinalRotation;

                Vector3 euler = _playerCamera.transform.rotation.eulerAngles;

                _cameraRotation = new Vector2(euler.y, euler.x);

                //if (Mathf.Abs(RotationMismatch) > rotationTolerance)
                //{
                //    _rotatingToTargetTimer = rotateToTargetTime;
                //}
                //_rotatingToTargetTimer -= Time.deltaTime;
            }
        }
    }
}
