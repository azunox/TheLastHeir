using UnityEngine;
using System.Collections.Generic;

namespace TheLastHeir.Runtime.Cutscene
{
    [RequireComponent(typeof(Collider))]
    public class CutsceneTrigger : MonoBehaviour
    {
        [SerializeField] private CutsceneManager cutsceneManager;
        
        [SerializeField] private CutsceneData cutsceneData;
        [SerializeField] private List<Transform> waypoints;
        [SerializeField] private Animator actorAnimator;
        
        [SerializeField] private bool playOnce = true;
        
        private bool _hasPlayed = false;

        private void Awake()
        {
            if (cutsceneManager == null)
                cutsceneManager = FindObjectOfType<CutsceneManager>();

            Collider col = GetComponent<Collider>();
            if (col != null) col.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_hasPlayed && playOnce) return;
            if (cutsceneManager == null || cutsceneData == null) return;

            if (other.CompareTag("Player"))
            {
                bool success = cutsceneManager.PlayCutscene(cutsceneData, waypoints, actorAnimator);
                
                if (success)
                    _hasPlayed = true;
            }
        }
    }
}