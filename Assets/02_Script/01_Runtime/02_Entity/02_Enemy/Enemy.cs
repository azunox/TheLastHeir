using UnityEngine;
using UnityEngine.AI; 
using TheLastHeir.Runtime.Structs; 
using TheLastHeir.Runtime.Combat; 

namespace TheLastHeir.Runtime.Entity.Enemy
{
    public class Enemy : StandardEntity
    {
        [Header("[ Data & Settings ]")]
        public EnemyDefinitionSO definition; 
        [SerializeField] private WeaponDamageCollider weaponCollider; 

        [Header("[ FSM Runtime ]")]
        private EnemyContext _context;
        [SerializeField] private StateSO _currentState; 
        
        [SerializeField] private StandardCombatHandler _combatHandler; 
        
        protected override void Setup()
        {
            base.Setup();
            _combatHandler = GetComponent<StandardCombatHandler>();
            
            if (Attributes != null && definition != null)
            {
                var enemyAttr = Attributes as EnemyAttributeEntity;
                if (enemyAttr != null)
                {
                    enemyAttr.Initialize(definition.enemyStat.maxHealth);
                }
            }
            
            GameObject player = GameObject.FindWithTag("Player");
            
            _context = new EnemyContext {
                owner = transform,
                agent = GetComponent<NavMeshAgent>(), 
                anim = animator, 
                controller = this,
                target = player != null ? player.transform : null
            };
            
            if (_context.agent != null)
            {
                _context.agent.updateRotation = false;
                _context.agent.autoBraking = false;
            }
            
            if (weaponCollider != null) weaponCollider.DisableDamageCollider();

            
            OnHit += HandleHit;
            
            if (definition != null && definition.initialState != null)
            {
                ChangeState(definition.initialState);
            }
        }
        
        protected override void Tick()
        {
            base.Tick();

            if (_currentState == null) return;
            
            _context.stateTimer += Time.deltaTime;
            
            if (!IsDead) CheckTransitions();
            
            _currentState.OnTick(_context);
        }
        
        public override void OnTakeDamage(HitInfo hitInfo)
        {
            if (IsDead || IsInvincible) return;

            if (_combatHandler != null)
            {
                _combatHandler.ApplyHit(hitInfo);

                if (IsDead)
                {
                    OnDeath();
                }
            }
            else
            {
                base.OnTakeDamage(hitInfo);
            }
        }
        
        private void CheckTransitions()
        {
            if (_currentState == null) return;
            foreach (var transition in _currentState.transitions)
            {
                if (transition.CanTransition(_context))
                {
                    ChangeState(transition.targetState);
                    break;
                }
            }
        }

        public void ChangeState(StateSO newState)
        {
            if (newState == null) return;
            if (_currentState == newState) return;
            
            var prevState = _currentState;

            if (_currentState != null) _currentState.OnExit(_context);
            
            _currentState = newState;
            _context.stateTimer = 0;
            
            animator.ResetTrigger("Attack"); 
            animator.ResetTrigger("Hit");
            
            Debug.Log($"Enemy State Changed to: {prevState} => {_currentState.name}");
            
            _currentState.OnEnter(_context);
        }
        
        private void HandleHit()
        {
            if (definition != null && definition.hitState != null)
            {
                ChangeState(definition.hitState);
            }
        }

        protected override void OnDeath()
        {
            if (definition != null && definition.dieState != null)
            {
                ChangeState(definition.dieState);
            }
            
            // 정지
            if (_context.agent != null) _context.agent.isStopped = true;
            DisableWeaponCollider(); // 죽으면서 공격 판정 끄기
            
            InvokeOnDied();
        }
        
        public void EnableWeaponCollider()
        {
            if (weaponCollider != null)
            {
                float damageAmount = definition.enemyStat.baseDamage;
                Damage dmg = new Damage { 
                    Amount = damageAmount, 
                    Type = ElementType.Physical 
                };
                weaponCollider.EnableDamageCollider(dmg, gameObject);
            }
        }

        public void DisableWeaponCollider()
        {
            if (weaponCollider != null)
            {
                weaponCollider.DisableDamageCollider();
            }
        }
    }
}