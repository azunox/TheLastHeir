using UnityEngine;
using TheLastHeir.Runtime.Camera;

namespace TheLastHeir.Runtime.Cutscene
{
    public class CutsceneCameraController : MonoBehaviour
    {
        private UnityEngine.Camera _mainCamera;
        private CameraHandler _gameplayCameraHandler;

        public void Initialize(UnityEngine.Camera cam, CameraHandler handler)
        {
            _mainCamera = cam;
            _gameplayCameraHandler = handler;
        }

        public void SetCutsceneMode(bool isCutsceneActive)
        {
            if (_gameplayCameraHandler != null)
                _gameplayCameraHandler.enabled = !isCutsceneActive;
        }

        public void MoveCamera(Vector3 startPos, Quaternion startRot, Transform target, float t)
        {
            if (_mainCamera == null || target == null) return;

            float smoothT = t * t * (3f - 2f * t);

            _mainCamera.transform.position = Vector3.Lerp(startPos, target.position, smoothT);
            _mainCamera.transform.rotation = Quaternion.Slerp(startRot, target.rotation, smoothT);
        }

        public void ToTarget(Transform target)
        {
            if (_mainCamera == null || target == null) return;

            _mainCamera.transform.position = target.position;
            _mainCamera.transform.rotation = target.rotation;
        }

        public (Vector3 pos, Quaternion rot) GetCurrentState()
        {
            if (_mainCamera == null) return (Vector3.zero, Quaternion.identity);
            return (_mainCamera.transform.position, _mainCamera.transform.rotation);
        }
    }
}