using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using TheLastHeir.Runtime.Effects; 

namespace TheLastHeir.Runtime.Cutscene
{
    [RequireComponent(typeof(AudioSource))]
    public class CutsceneEffectController : MonoBehaviour
    {
        [SerializeField] private Light mainDirectionalLight;
        
        [SerializeField] private Volume glitchVolume; 
        [SerializeField] private AudioClip glitchSound;

        private AudioSource _sfxSource;
        private float _originalLightIntensity;
        
        private Coroutine _glitchFadeCoroutine; 
        private Coroutine _shaderGlitchCoroutine;

        private void Awake()
        {
            _sfxSource = GetComponent<AudioSource>();
            
            if (glitchVolume != null) glitchVolume.weight = 0f;
            
            if (mainDirectionalLight != null)
                _originalLightIntensity = mainDirectionalLight.intensity;
        }

        public void UpdateLighting(float startIntensity, float targetIntensity, float t)
        {
            if (mainDirectionalLight != null)
            {
                float smoothT = t * t * (3f - 2f * t);
                mainDirectionalLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, smoothT);
            }
        }

        public void HandleGlitchEffect(CutsceneStep step)
        {
            float targetIntensity = step.enableGlitch ? step.glitchIntensity : 0f;
            
            // 효과가 켜질 때 소리 재생 (나중에)
            bool isTurningOn = targetIntensity > 0.01f;
            bool wasVolumeOff = glitchVolume != null && glitchVolume.weight < 0.1f;
            bool wasShaderOff = GlitchEffectController.Instance != null && GlitchEffectController.Instance.currentIntensity < 0.1f;

            if (step.enableGlitch && isTurningOn && (wasVolumeOff || wasShaderOff) && glitchSound != null)
                _sfxSource.PlayOneShot(glitchSound);

            // 쉐이더 글리치 (화면 효과)
            if (GlitchEffectController.Instance != null)
            {
                if (_shaderGlitchCoroutine != null) StopCoroutine(_shaderGlitchCoroutine);
                StartCoroutine(GlitchEffectController.Instance.FadeGlitch(targetIntensity, 0.2f));
            }

            if (glitchVolume != null)
            {
                if (_glitchFadeCoroutine != null) StopCoroutine(_glitchFadeCoroutine);
                _glitchFadeCoroutine = StartCoroutine(FadeVolumeWeight(targetIntensity, 0.5f));
            }
        }

        public void PlayStepSound(AudioClip clip, float volume)
        {
            if (clip != null && _sfxSource != null)
                _sfxSource.PlayOneShot(clip, volume);
        }

        public void ResetEffects(bool resetLight)
        {
            if (GlitchEffectController.Instance != null)
            {
                if (_shaderGlitchCoroutine != null) StopCoroutine(_shaderGlitchCoroutine);
                StartCoroutine(GlitchEffectController.Instance.FadeGlitch(0f, 0.5f));
            }

            if (glitchVolume != null)
            {
                if (_glitchFadeCoroutine != null) StopCoroutine(_glitchFadeCoroutine);
                StartCoroutine(FadeVolumeWeight(0f, 1.0f));
            }

            if (resetLight && mainDirectionalLight != null)
                mainDirectionalLight.intensity = _originalLightIntensity;
        }
        
        public float GetCurrentLightIntensity()
        {
            return mainDirectionalLight != null ? mainDirectionalLight.intensity : 1f;
        }

        public void SetLightIntensity(float intensity)
        {
            if (mainDirectionalLight != null) mainDirectionalLight.intensity = intensity;
        }

        private IEnumerator FadeVolumeWeight(float target, float duration)
        {
            if (glitchVolume == null) yield break;

            float start = glitchVolume.weight;
            float timer = 0f;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                glitchVolume.weight = Mathf.Lerp(start, target, timer / duration);
                yield return null;
            }
            glitchVolume.weight = target;
        }
    }
}