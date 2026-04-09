using UnityEngine;
using TheLastHeir.Runtime.Entity;

namespace TheLastHeir.Runtime.States
{
    public class PlayerAttackState : PlayerBaseState
    {
        private float _exitTimer;
        private PlayerAttributeHandler _attributes;
        
        private int _lastAnimationHash;
        private bool _hasRegisteredCombo;

        public PlayerAttackState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
        {
            _attributes = player.GetComponent<PlayerAttributeHandler>();
        }

        public override void Enter()
        {
            _exitTimer = 0.1f;
            player.Movement.StopMovement();
            
            player.CombatHandler.Attack();
            
            _lastAnimationHash = 0;
            _hasRegisteredCombo = false;
        }

        public override void Update()
        {
            AnimatorStateInfo info = player.animator.GetCurrentAnimatorStateInfo(0);
            
            if (info.shortNameHash != _lastAnimationHash)
            {
                _lastAnimationHash = info.shortNameHash;
                _hasRegisteredCombo = false;
            }
            
            if (PlayerInputHandler.Instance.AttackTriggered)
            {
                if (!_hasRegisteredCombo && info.IsTag("Attack") && info.normalizedTime >= 0.3f) 
                {
                    if (_attributes.UseStamina(player.playerStats.AttackStaminaCost))
                    {
                        player.CombatHandler.Attack(); 
                        _hasRegisteredCombo = true;
                    }
                }
            }

            if (_exitTimer > 0)
            {
                _exitTimer -= Time.deltaTime;
                return;
            }

            if (info.IsTag("Attack") && info.normalizedTime >= 0.7f && !player.animator.IsInTransition(0))
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }
}