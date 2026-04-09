using System;
using TheLastHeir.Runtime.Structs;
using UnityEngine;
using TheLastHeir.Runtime.Interfaces;
using TheLastHeir.Runtime.Combat;

namespace TheLastHeir.Runtime.Entity
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    public abstract class StandardEntity : MonoBehaviour, IDamageable
    {
        public CharacterController cc { get; private set; }
        public Animator animator { get; private set; }
        
        public StandardAttributeEntity Attributes { get; protected set; }

        public bool IsDead => Attributes != null && Attributes.CurHp <= 0;
        public bool IsInvincible { get; set; }
        
        [Header("Health")]
        public int CurHp => Attributes != null ? Attributes.CurHp : 0;
        public int MaxHp => Attributes != null ? Attributes.MaxHp : 0;
        
        public event Action<int> OnHpChanged = delegate { };
        public event Action OnHit = delegate { };
        public event Action OnDied = delegate { };

        private bool _isSetup;
        
        private void Start()
        {
            Setup(); 
        }

        protected virtual void Setup()
        {
            if (_isSetup) return;
            _isSetup = true;

            cc = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            Attributes = GetComponent<StandardAttributeEntity>();
        }
        
        private void Update()
        {
            Tick();
        }

        protected virtual void Tick() { }

        protected abstract void OnDeath();
        
        public virtual void OnTakeDamage(HitInfo hitInfo)
        {
            if (IsDead || IsInvincible) return;
            
            if (Attributes != null)
            {
                Attributes.TakeDamage(hitInfo.Damage.Amount);
                InvokeOnHpChanged(Attributes.CurHp);
            }
            
            if (VFXManager.Instance != null)
            {
                VFXManager.Instance.PlayHitEffect(hitInfo.HitPoint, hitInfo.HitNormal);
            }
            
            if (DamageIndicatorManager.Instance != null)
            {
                bool isPlayer = CompareTag("Player");
                
                DamageIndicatorManager.Instance.ShowDamage((int)hitInfo.Damage.Amount, transform.position + Vector3.up, isPlayer, false);
            }
            
            if (IsDead)
            {
                OnDeath();
                InvokeOnDied();
            }
            else
            {
                InvokeOnHit();
            }
        }

        public void InvokeOnHit() => OnHit?.Invoke();
        public void InvokeOnHpChanged() => OnHpChanged?.Invoke(CurHp);
        public void InvokeOnHpChanged(int newHp) => OnHpChanged?.Invoke(newHp);
        public void InvokeOnDied() => OnDied?.Invoke();
    }
}