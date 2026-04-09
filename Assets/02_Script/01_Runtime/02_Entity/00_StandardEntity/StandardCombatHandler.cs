using System.Collections.Generic;
using UnityEngine;
using TheLastHeir.Runtime.Structs;
using TheLastHeir.Runtime.Utility;

namespace TheLastHeir.Runtime.Entity
{
    public class StandardCombatHandler : EntityOwnedHandler<StandardEntity>
    {
        [Header("References")]
        [SerializeField] private StatusEffectLibrary statusLibrary;
        [SerializeField] private ElementMultiplier elementMultiplier;
        
        private readonly List<StatusRuntime> activeStatuses = new();
        public void ApplyHit(in HitInfo hit)
        {
            if (owner.IsDead || owner.IsInvincible) return;
            
            float final = DamageCalculator.CalculateFinal(hit.Damage, owner.Attributes.DamageNegation, elementMultiplier);
            
            owner.Attributes.TakeDamage(final);
            
            owner.InvokeOnHit();
            owner.InvokeOnHpChanged(owner.Attributes.CurHp);

            if (owner.IsDead)
                owner.InvokeOnDied();
        }
        
        public void ApplyStatus(StatusEffectType type)
        {
            if (!statusLibrary || !statusLibrary.TryGet(type, out var spec))
                return;

            var runtime = new StatusRuntime
            {
                Type = spec.Type,
                Remaining = spec.Duration,
                Interval = spec.Interval,
                NextTickPoint = Time.time + spec.Interval
            };
            
            for (int i = 0; i < activeStatuses.Count; i++)
            {
                if (activeStatuses[i].Type == runtime.Type)
                {
                    activeStatuses[i] += runtime;
                    return;
                }
            }

            activeStatuses.Add(runtime);
        }
        
        private void Update()
        {
            if (owner.IsDead) return;

            for (int i = 0; i < activeStatuses.Count; i++)
            {
                var s = activeStatuses[i];
                s.Remaining -= Time.deltaTime;

                if (Time.time >= s.NextTickPoint)
                {
                    if (statusLibrary.TryGet(s.Type, out var spec))
                    {
                        var tickHit = new HitInfo(
                            new Damage { Type = ElementType.Physical, Amount = spec.TickDamage },
                            owner.Attributes.Type,
                            owner.transform.position + Vector3.up,
                            Vector3.zero
                        );
                        ApplyHit(tickHit);
                    }
                    s.NextTickPoint = Time.time + s.Interval;
                }

                if (s.Remaining <= 0f)
                {
                    activeStatuses.RemoveAt(i--);
                    continue;
                }

                activeStatuses[i] = s;
            }
        }
    }
}