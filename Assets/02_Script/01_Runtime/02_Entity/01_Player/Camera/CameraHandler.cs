using UnityEngine;
using TheLastHeir.Runtime.Entity;

namespace TheLastHeir.Runtime.Camera
{
    public class CameraHandler : MonoBehaviour
    {
        [Header("References")]
        public GameObject cameraPivot;
        public Transform playerTarget;
        public UnityEngine.Camera mainCamera;

        [Header("Follow Settings")]
        [SerializeField] private float followSmoothTime = 0.03f;
        
        [Header("Lock On")] 
        public Transform currentLockOnTarget;
        [SerializeField] private float lockOnSpeed = 15f;

        [Header("Collision Settings")]
        [SerializeField] private bool enableCollision = true;
        [SerializeField] private LayerMask obstacleLayers = (1 << 0) | (1 << 8);
        [SerializeField] private float cameraCollisionRadius = 0.2f;
        [SerializeField] private float minCollisionDistance = 0.5f;
        [SerializeField] private float cameraSmoothSpeed = 15f;

        private Vector3 _currentFollowVelocity;
        private float upAndDownRotationSensitivity = 220;
        private float leftAndRightRotationSensitivity = 220;

        private float _defaultDistance;
        private float _targetDistance;
        private float _currentDistance;
        private Vector3 _cameraDirection;

        [SerializeField] private float upAndDownLookAngle;
        [SerializeField] private float leftAndRightLookAngle;

        private void Start()
        {
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false; 

            if (mainCamera == null) mainCamera = UnityEngine.Camera.main;
            
            _cameraDirection = mainCamera.transform.localPosition.normalized;
            _defaultDistance = mainCamera.transform.localPosition.magnitude;
            _currentDistance = _defaultDistance;
        }
        
        private void OnEnable()
        {
            ResetCameraRotation();
        }

        private void ResetCameraRotation()
        {
            if (mainCamera != null)
            {
                mainCamera.transform.localRotation = Quaternion.identity;
            }
        } 
        private void LateUpdate()
        {
            if (playerTarget == null) return;
            
            HandleFollowTarget();
            HandleRotations();
            
            if (enableCollision)
            {
                HandleCameraCollision();
            }
        }

        private void HandleFollowTarget()
        {
            cameraPivot.transform.position = Vector3.SmoothDamp(
                cameraPivot.transform.position, 
                playerTarget.position, 
                ref _currentFollowVelocity, 
                followSmoothTime
            );
        }

        private void HandleRotations()
        {
            if (currentLockOnTarget != null)
            {
                Vector3 dir = currentLockOnTarget.position - cameraPivot.transform.position;
                
                if (dir.sqrMagnitude < 0.001f) return;

                Quaternion targetRotation = Quaternion.LookRotation(dir);
                
                cameraPivot.transform.rotation = Quaternion.Slerp(
                    cameraPivot.transform.rotation,
                    targetRotation,
                    lockOnSpeed * Time.deltaTime
                );
                
                Vector3 currentEuler = cameraPivot.transform.eulerAngles;
                upAndDownLookAngle = currentEuler.y;
                leftAndRightLookAngle = currentEuler.x;
            }
            else
            {
                upAndDownLookAngle += (PlayerInputHandler.Instance.cameraHorizontalInput * upAndDownRotationSensitivity * Time.deltaTime);
                leftAndRightLookAngle -= (PlayerInputHandler.Instance.cameraVerticalInput * leftAndRightRotationSensitivity * Time.deltaTime); // 마우스 반전 방지 위해 - 연산
                
                leftAndRightLookAngle = Mathf.Clamp(leftAndRightLookAngle, -35f, 35f);

                cameraPivot.transform.rotation = Quaternion.Euler(leftAndRightLookAngle, upAndDownLookAngle, 0);
            }
        }

        private void HandleCameraCollision()
        {
            _targetDistance = _defaultDistance;
            RaycastHit hit;
            Vector3 origin = cameraPivot.transform.position;
            Vector3 targetPos = cameraPivot.transform.TransformPoint(_cameraDirection * _defaultDistance);
            Vector3 direction = (targetPos - origin).normalized;

            if (Physics.SphereCast(origin, cameraCollisionRadius, direction, out hit, _defaultDistance, obstacleLayers))
            {
                float distance = Vector3.Distance(origin, hit.point);
                _targetDistance = Mathf.Clamp(distance - 0.2f, minCollisionDistance, _defaultDistance);
            }
            
            _currentDistance = Mathf.Lerp(_currentDistance, _targetDistance, Time.deltaTime * cameraSmoothSpeed);
            mainCamera.transform.localPosition = _cameraDirection * _currentDistance;
        }
        
        public void SetLockOnTarget(Transform target)
        {
            currentLockOnTarget = target;
        }
    }
}