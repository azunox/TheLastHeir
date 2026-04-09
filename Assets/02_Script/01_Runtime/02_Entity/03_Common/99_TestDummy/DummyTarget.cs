using UnityEngine;
using TheLastHeir.Runtime.Interfaces;
using TheLastHeir.Runtime.Structs;
using TheLastHeir.Runtime.Combat;

namespace TheLastHeir.Runtime.Entity
{
    public class DummyTarget : MonoBehaviour, IDamageable
    {
        [SerializeField] private float hp = 100f;
        

        public void OnTakeDamage(HitInfo hitInfo)
        {
            hp -= hitInfo.Damage.Amount;
            if (VFXManager.Instance != null)
            {
                VFXManager.Instance.PlayHitEffect(hitInfo.HitPoint, hitInfo.HitNormal);
            }
            
            if (DamageIndicatorManager.Instance != null)
            {
                DamageIndicatorManager.Instance.ShowDamage((int)hitInfo.Damage.Amount, transform.position, false, false);
            }

            Debug.Log($"{hitInfo.Damage.Amount} {hp})");

            if (hp <= 0)
            {
                Debug.Log("사망");
            }
        }
    }
}