using UnityEngine;
using TheLastHeir.Runtime.Entity;

namespace TheLastHeir.Runtime.States
{
    public class PlayerHitState : PlayerBaseState
    {
        private float _stunTime = 0.5f; 
        private float _timer;

        public PlayerHitState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

        public override void Enter()
        {
            _timer = 0f;
            
            player.Movement.StopMovement();
            player.AnimationHandler.PlayHitAnimation();
        }

        public override void Update()
        {
            _timer += Time.deltaTime;
            
            if (_timer > _stunTime)
            {
                AnimatorStateInfo info = player.animator.GetCurrentAnimatorStateInfo(0);
                
                if (info.IsTag("Hit") && info.normalizedTime >= 0.9f)
                {
                    stateMachine.ChangeState(player.IdleState);
                }
                else if (!info.IsTag("Hit") && _timer > _stunTime + 0.1f)
                {
                    stateMachine.ChangeState(player.IdleState);
                }
            }
        }

        public override void Exit()
        {
        }
    }
}