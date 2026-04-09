using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

namespace TheLastHeir.Runtime.Effects
{
    public class GlitchEffectController : MonoBehaviour
    {
        public static GlitchEffectController Instance { get; private set; }

        [Header("Material Settings")]
        [SerializeField] private Material glitchMaterial;
        
        [Header("Runtime")]
        [Range(0, 1)] public float currentIntensity = 0f;

        private int _intensityID;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            _intensityID = Shader.PropertyToID("_Intensity");
        }

        private void Start()
        {
            SetGlitchIntensity(0f);
        }

        public void SetGlitchIntensity(float intensity)
        {
            currentIntensity = Mathf.Clamp01(intensity);
            if (glitchMaterial != null)
            {
                glitchMaterial.SetFloat(_intensityID, currentIntensity);
            }
        }
        
        public IEnumerator FadeGlitch(float targetIntensity, float duration)
        {
            float startIntensity = currentIntensity;
            float timer = 0f;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float t = timer / duration;
                SetGlitchIntensity(Mathf.Lerp(startIntensity, targetIntensity, t));
                yield return null;
            }
            SetGlitchIntensity(targetIntensity);
        }
    }
}