using UnityEngine;
using TheLastHeir.Runtime.Entity;
using TheLastHeir.Runtime.Camera;

namespace TheLastHeir.Runtime.Combat
{
    public class LockOnHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float searchRadius = 20f;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private string targetPointName = "LockOnTarget"; 
        
        [Header("Smooth Settings")]
        [Tooltip("타겟 포지션을 추적할 때의 부드러움 정도 (낮을수록 부드러움)")]
        [SerializeField] private float trackingSmoothTime = 0.02f;

        [Header("UI")]
        [SerializeField] private LockOnVisualizer visualizer;

        public Animator animator;
        private ITargetFinder _targetFinder;
        public Transform _currentTarget; 
        private Player _player;
        private UnityEngine.Camera _mainCam;
        private CameraHandler _cameraHandler; 

        private Vector3 _currentVisualizerVelocity;
        private Vector3 _smoothedTargetPos;
        
        public bool IsLockedOn => _currentTarget != null;

        private void Awake()
        {
            _player = GetComponent<Player>();
            _targetFinder = new EnemyTargetFinder(60f); 
            
            _mainCam = UnityEngine.Camera.main;
            if (_mainCam == null) Debug.LogError("MainCamera Not Found");

            _cameraHandler = FindObjectOfType<CameraHandler>();
        }

        private void Update()
        {
            if (_player.PlayerInput.LockOnTriggered)
            {
                ToggleLockOn();
            }
            animator.SetBool("LockOn", _currentTarget != null);
        }

        private void LateUpdate()
        {
            HandleTargetTracking();
        }

        private void HandleTargetTracking()
        {
            if (_currentTarget == null) return;

            if (!_currentTarget.gameObject.activeInHierarchy)
            {
                ResetLockOn();
                return;
            }

            float dist = Vector3.Distance(transform.position, _currentTarget.root.position);
            if (dist > searchRadius * 1.2f)
            {
                ResetLockOn();
                return;
            }
            
            if (visualizer != null)
            {
                _smoothedTargetPos = Vector3.SmoothDamp(
                    _smoothedTargetPos, 
                    _currentTarget.position, 
                    ref _currentVisualizerVelocity, 
                    trackingSmoothTime
                );

                visualizer.UpdatePosition(_currentTarget, _mainCam);
            }
        }

        private void ToggleLockOn()
        {
            if (_currentTarget != null)
            {
                ResetLockOn();
                return;
            }
            
            Transform enemyRoot = _targetFinder.FindTarget(transform, _mainCam.transform, searchRadius, enemyLayer);
            
            if (enemyRoot != null)
            {
                _currentTarget = FindChildByName(enemyRoot, targetPointName);
                if (_currentTarget == null) _currentTarget = enemyRoot;

                // 초기 위치 초기화
                _smoothedTargetPos = _currentTarget.position;

                if (visualizer != null) visualizer.SetVisible(true);
                if (_cameraHandler != null) _cameraHandler.SetLockOnTarget(_currentTarget);
                
                Debug.Log($"LockOn : {enemyRoot.name}");
            }
        }

        private Transform FindChildByName(Transform parent, string name)
        {
            Transform[] allChildren = parent.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                if (child.name.Equals(name, System.StringComparison.OrdinalIgnoreCase))
                    return child;
            }
            return null;
        }

        public void ResetLockOn()
        {
            _currentTarget = null;
            if (visualizer != null) visualizer.SetVisible(false);
            if (_cameraHandler != null) _cameraHandler.SetLockOnTarget(null);
        }
        
        public Transform GetCurrentTarget() => _currentTarget;
    }
}