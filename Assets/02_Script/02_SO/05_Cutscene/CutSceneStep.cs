using UnityEngine;
using System.Collections.Generic;

namespace TheLastHeir.Runtime.Cutscene
{
    [System.Serializable]
    public class CutsceneStep
    {
        [Header("Info")]
        public string stepName; 

        [Header("Camera & Time")]
        public float moveDuration = 2.0f;
        public float waitDuration = 1.5f;

        [Header("Lights")]
        [Tooltip("해당 컷신의 밝기")]
        public float targetLightIntensity = 1.0f; 

        [Header("Fade Effect")]
        public bool fadeInOnStart = false;
        public bool fadeOutOnEnd = false;
        public float fadeDuration = 1.0f;
        public float fadeOutHold = 0.0f;

        [Header("Glitch Effect")]
        public bool enableGlitch = false;
        [Range(0f, 1f)] public float glitchIntensity = 1.0f;

        [Header("Audio")]
        public AudioClip stepSound;
        public float soundDelay = 0.0f; 
        [Range(0f, 1f)] public float volume = 1.0f;

        [Header("Dialogue")]
        public string speakerName;
        
        [TextArea(2, 5)] 
        public List<string> dialogueLines = new List<string>();

        [Header("Animation")]
        public string animationTrigger;
    }
}