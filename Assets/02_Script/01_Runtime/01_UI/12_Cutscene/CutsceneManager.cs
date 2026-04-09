using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TheLastHeir.Runtime.Entity;
using TheLastHeir.Runtime.Camera;
using TheLastHeir.Runtime.UI;

namespace TheLastHeir.Runtime.Cutscene
{
    [RequireComponent(typeof(CutsceneCameraController))]
    [RequireComponent(typeof(CutsceneEffectController))]
    public class CutsceneManager : MonoBehaviour
    {
        public static CutsceneManager Instance { get; private set; }

        [SerializeField] private UnityEngine.Camera mainCamera;
        [SerializeField] private CameraHandler cameraHandler;

        private bool _isPlaying = false;
        private Coroutine _currentCutsceneRoutine;
        private Coroutine _dialogueRoutine;

        private CutsceneCameraController _camController;
        private CutsceneEffectController _effectController;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            _camController = GetComponent<CutsceneCameraController>();
            _effectController = GetComponent<CutsceneEffectController>();

            if (mainCamera == null) mainCamera = UnityEngine.Camera.main;
            if (cameraHandler == null) cameraHandler = FindObjectOfType<CameraHandler>();

            if (_camController != null)
                _camController.Initialize(mainCamera, cameraHandler);
        }

        public bool PlayCutscene(CutsceneData data, List<Transform> waypoints, Animator actor = null)
        {
            if (!IsDataValid(data, waypoints)) return false;
            
            if (_isPlaying) 
            {
                Debug.LogWarning("Already playing");
                return false; 
            }
            
            _currentCutsceneRoutine = StartCoroutine(ProcessCutsceneSequence(data, waypoints, actor));
            return true;
        }

        private IEnumerator ProcessCutsceneSequence(CutsceneData data, List<Transform> waypoints, Animator actor)
        {
            if (!IsDataValid(data, waypoints)) yield break;

            InitializeCutscene();

            int count = Mathf.Min(data.steps.Count, waypoints.Count);

            for (int i = 0; i < count; i++)
            {
                yield return StartCoroutine(ExecuteStep(data.steps[i], waypoints[i], actor));
            }

            CleanupCutscene(data);
        }

        private IEnumerator ExecuteStep(CutsceneStep step, Transform targetPoint, Animator actor)
        {
            HandleStepStart(step);

            if (_dialogueRoutine != null) StopCoroutine(_dialogueRoutine);
            _dialogueRoutine = StartCoroutine(PlayStepDialogue(step));

            if (actor != null && !string.IsNullOrEmpty(step.animationTrigger))
                actor.SetTrigger(step.animationTrigger);

            float timer = 0f;
            Vector3 startPos = mainCamera.transform.position;
            Quaternion startRot = mainCamera.transform.rotation;
            
            float startLight = _effectController.GetCurrentLightIntensity();
            bool hasPlayedSound = false;

            if (step.moveDuration <= 0.01f)
            {
                if (_camController != null) _camController.ToTarget(targetPoint);
                else
                {
                    mainCamera.transform.position = targetPoint.position;
                    mainCamera.transform.rotation = targetPoint.rotation;
                }
            }
            else
            {
                while (timer < step.moveDuration)
                {
                    timer += Time.deltaTime;
                    float t = timer / step.moveDuration;

                    if (!hasPlayedSound && step.stepSound != null && timer >= step.soundDelay)
                    {
                        _effectController.PlayStepSound(step.stepSound, step.volume);
                        hasPlayedSound = true;
                    }

                    if (_camController != null)
                        _camController.MoveCamera(startPos, startRot, targetPoint, t);
                    
                    _effectController.UpdateLighting(startLight, step.targetLightIntensity, t);

                    yield return null;
                }
            }

            if (!hasPlayedSound) 
                _effectController.PlayStepSound(step.stepSound, step.volume);

            if (_camController != null) _camController.ToTarget(targetPoint);
            _effectController.SetLightIntensity(step.targetLightIntensity);

            if (step.waitDuration > 0)
                yield return new WaitForSeconds(step.waitDuration);

            if (step.fadeOutOnEnd && CutsceneUI.Instance != null)
            {
                yield return StartCoroutine(CutsceneUI.Instance.FadeScreen(1f, step.fadeDuration));
                if (step.fadeOutHold > 0) yield return new WaitForSeconds(step.fadeOutHold);
            }

            if (_dialogueRoutine != null) StopCoroutine(_dialogueRoutine);
        }

        private void InitializeCutscene()
        {
            _isPlaying = true;
            SetPlayerInput(false);
            
            if (_camController != null) _camController.SetCutsceneMode(true);
            
            if (CutsceneUI.Instance != null) 
                CutsceneUI.Instance.EnterCutsceneMode();
        }

        private void CleanupCutscene(CutsceneData data)
        {
            _effectController.ResetEffects(data.returnLightingOnEnd);
            
            if (CutsceneUI.Instance != null) 
                CutsceneUI.Instance.ExitCutsceneMode();

            if (data.returnCameraOnEnd && _camController != null)
                _camController.SetCutsceneMode(false);
            
            SetPlayerInput(true);
            _isPlaying = false;
            _currentCutsceneRoutine = null;
        }

        private void HandleStepStart(CutsceneStep step)
        {
            if (PlayerInputHandler.Instance != null && PlayerInputHandler.Instance.enabled)
                SetPlayerInput(false);

            _effectController.HandleGlitchEffect(step);

            if (step.fadeInOnStart && CutsceneUI.Instance != null)
                StartCoroutine(SequenceFadeIn(step.fadeDuration));
        }

        private void SetPlayerInput(bool enabled)
        {
            if (PlayerInputHandler.Instance != null)
                PlayerInputHandler.Instance.enabled = enabled;
        }

        private bool IsDataValid(CutsceneData data, List<Transform> waypoints)
        {
            if (data == null || data.steps.Count == 0 || waypoints == null || waypoints.Count == 0)
            {
                _isPlaying = false;
                return false;
            }
            return true;
        }

        private IEnumerator SequenceFadeIn(float duration)
        {
            yield return StartCoroutine(CutsceneUI.Instance.FadeScreen(1f, 0f));
            StartCoroutine(CutsceneUI.Instance.FadeScreen(0f, duration));
        }

        private IEnumerator PlayStepDialogue(CutsceneStep step)
        {
            if (CutsceneUI.Instance == null) yield break;

            if (step.dialogueLines == null || step.dialogueLines.Count == 0)
            {
                CutsceneUI.Instance.HideSubtitle();
                yield break;
            }

            float totalDuration = step.moveDuration + step.waitDuration;
            float timePerLine = (step.dialogueLines.Count > 0) ? (totalDuration / step.dialogueLines.Count) : 0;

            foreach (var line in step.dialogueLines)
            {
                if (!string.IsNullOrEmpty(line))
                    CutsceneUI.Instance.ShowSubtitle(step.speakerName, line);
                else
                    CutsceneUI.Instance.HideSubtitle();

                if (timePerLine > 0) yield return new WaitForSeconds(timePerLine);
            }
        }
    }
}