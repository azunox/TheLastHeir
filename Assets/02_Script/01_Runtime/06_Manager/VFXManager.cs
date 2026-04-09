using UnityEngine;
using TheLastHeir.Runtime.Structs;

namespace TheLastHeir.Runtime.Combat
{
    public class VFXManager : MonoBehaviour
    {
        public static VFXManager Instance { get; private set; }

        [Header("Common VFX")]
        [SerializeField] private GameObject defaultBloodVFX; // 기본 피 이펙트
        [SerializeField] private GameObject metalSparkVFX;   // 금속 타격 이펙트 (나중을 위해)
        [SerializeField] private float destroyDelay = 2f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void PlayHitEffect(Vector3 hitPoint, Vector3 hitNormal)
        {
            if (defaultBloodVFX == null) return;

            // 1. 이펙트 생성
            GameObject vfx = Instantiate(defaultBloodVFX, hitPoint, Quaternion.identity);

            // 2. 방향 설정 (튀는 방향)
            if (hitNormal != Vector3.zero)
            {
                vfx.transform.rotation = Quaternion.LookRotation(hitNormal);
            }

            // 3. 삭제 
            Destroy(vfx, destroyDelay);
        }
    }
}