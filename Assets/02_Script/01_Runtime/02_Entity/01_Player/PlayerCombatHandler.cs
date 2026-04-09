using UnityEngine;
using TheLastHeir.Runtime.AnimationHashes;
using TheLastHeir.Runtime.Combat;
using TheLastHeir.Runtime.Structs;
using TheLastHeir.Runtime.Enums;

namespace TheLastHeir.Runtime.Entity
{
    public class PlayerCombatHandler : MonoBehaviour
    {
        private Player player;
        private PlayerAttributeHandler attributes;
        
        [Header("Weapon Settings")]
        [SerializeField] private WeaponDamageCollider currentWeaponCollider;
        [SerializeField] private float baseDamage = 15f; 
        [SerializeField] private float strengthScaling = 2.0f;

        public void Initialize(Player owner)
        {
            player = owner;
            attributes = player.GetComponent<PlayerAttributeHandler>();
        }

        public void Attack()
        {
            player.AnimationHandler.SetTriggerByHash(AttackHashes.AttackTrigger);
            player.AnimationHandler.SetIntegerByHash(AttackHashes.AttackCount, 0);
        }

        // 일반 공격
        // 
        public void EnableWeaponCollider()
        {
            if (currentWeaponCollider != null)
            {
                float damageAmount = baseDamage;

                if (attributes != null)
                {
                    damageAmount += attributes.Strength * strengthScaling;
                }

                Damage dmg = new Damage { Amount = damageAmount, Type = ElementType.Physical };
                
                currentWeaponCollider.EnableDamageCollider(dmg, player.gameObject);
            }
        }

        public void DisableWeaponCollider()
        {
            if (currentWeaponCollider != null)
            {
                currentWeaponCollider.DisableDamageCollider();
            }
        }
    }
}