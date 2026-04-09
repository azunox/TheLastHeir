using UnityEngine;
using TheLastHeir.Runtime.Entity;

namespace TheLastHeir.Runtime.States
{
    public class PlayerMoveState : PlayerGroundedState
    {
        public PlayerMoveState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

        public override void Update()
        {
            base.Update();
            if (player.PlayerInput.move == Vector2.zero)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            if (player.PlayerInput.sprint)
            {
                float cost = player.playerStats.SprintStaminaCost * Time.deltaTime;
                
                if (!attributes.UseStamina(cost))
                {
                    player.PlayerInput.sprint = false;
                }
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate(); 
            player.AnimationHandler.HandleLocomotion();
        }
    }
}