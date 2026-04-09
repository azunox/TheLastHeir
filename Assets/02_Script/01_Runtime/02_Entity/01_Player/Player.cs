using System.Collections;
using UnityEngine;
using TheLastHeir.Runtime.States;
using TheLastHeir.Runtime.UI;

namespace TheLastHeir.Runtime.Entity
{
    public class Player : StandardEntity
    {
        public PlayerStats playerStats;
        public Transform GroundCheck;
        
        [Header("Handlers")]
        public PlayerInputHandler PlayerInput;
        public PlayerMovementHandler Movement;
        public PlayerAnimationHandler AnimationHandler;
        public PlayerCombatHandler CombatHandler;
        public bool CanMove = true;
        public bool CanRotate = true;

        [Header("State Machine")]
        public PlayerStateMachine StateMachine { get; private set; }
        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerAttackState AttackState { get; private set; }
        public PlayerRollState RollState { get; private set; }
        public PlayerHitState HitState { get; private set; }
        public PlayerUseItemState UseItemState { get; private set; }

        protected void Awake()
        {
            StateMachine = new PlayerStateMachine();
            IdleState = new PlayerIdleState(this, StateMachine);
            MoveState = new PlayerMoveState(this, StateMachine);
            AttackState = new PlayerAttackState(this, StateMachine);
            HitState = new PlayerHitState(this, StateMachine);
            UseItemState = new PlayerUseItemState(this, StateMachine);
            RollState = new PlayerRollState(this, StateMachine);
        }

        protected override void Setup()
        {
            base.Setup();
            Movement.Initialize(this);
            AnimationHandler.Initialize(this);
            CombatHandler.Initialize(this);
            
            StateMachine.Initialize(IdleState);
        }

        protected override void Tick()
        {
            base.Tick();

            if (IsDead) return;

            StateMachine.CurrentState.Update();
        }

        private void FixedUpdate()
        {
            if (IsDead) return;
            
            StateMachine.CurrentState.PhysicsUpdate();
        }
        
        protected override void OnDeath()
        { 
            AnimationHandler.PlayDeathAnimation();
            if (PlayerInputHandler.Instance != null)
            {
                PlayerInputHandler.Instance.canInput = false;
                PlayerInputHandler.Instance.move = Vector2.zero;
            }

            Movement.StopMovement();

            if (DeathUI.Instance != null)
            {
                DeathUI.Instance.ShowDeathUI();
            }
        }
        
        public void OnRespawn()
        {
            StartCoroutine(RespawnRoutine());
        }

        private IEnumerator RespawnRoutine()
        {
            if (PlayerInputHandler.Instance != null) PlayerInputHandler.Instance.canInput = false;
            Movement.StopMovement();
            
            AnimationHandler.PlayRespawnAnimation();

            yield return new WaitForSeconds(1.3f);
            
            if (PlayerInputHandler.Instance != null) PlayerInputHandler.Instance.canInput = true;
            
            StateMachine.ChangeState(IdleState);
        }
        
        public void EnableWeaponCollider()
        {
            CombatHandler.EnableWeaponCollider();
        }

        public void DisableWeaponCollider()
        {
            CombatHandler.DisableWeaponCollider();
        }
    }
}