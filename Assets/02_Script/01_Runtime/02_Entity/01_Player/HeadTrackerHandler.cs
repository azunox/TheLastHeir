using TheLastHeir.Runtime.Combat;
using UnityEngine;


public class HeadTrackHandler : MonoBehaviour
{
    private Animator animator;
    private LockOnHandler lockOnManager;

    [Header("추적 설정")]
    [Range(0, 1)] public float weight = 1.0f;    
    [Range(0, 1)] public float bodyWeight = 0.2f;  
    [Range(0, 1)] public float headWeight = 0.8f;   
    [Range(0, 1)] public float eyesWeight = 1.0f; 
    [Range(0, 1)] public float clampWeight = 0.5f; 

    [Header("스무딩")]
    public float transitionSpeed = 5f;         
    private float currentWeight = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        lockOnManager = GetComponent<LockOnHandler>();
    }
    
    void OnAnimatorIK(int layerIndex)
    {
        if (animator == null) return;
        
        bool isLockedOn = lockOnManager != null && lockOnManager._currentTarget != null;
        
        float targetWeight = isLockedOn ? weight : 0f;
        currentWeight = Mathf.Lerp(currentWeight, targetWeight, Time.deltaTime * transitionSpeed);

        if (currentWeight > 0.001f)
        {
            Transform target = lockOnManager._currentTarget;
            
            Vector3 targetPosition = target.position;
            animator.SetLookAtWeight(currentWeight, bodyWeight, headWeight, eyesWeight, clampWeight);
            animator.SetLookAtPosition(targetPosition);
        }
        else
        {
            animator.SetLookAtWeight(0);
        }
    }
}