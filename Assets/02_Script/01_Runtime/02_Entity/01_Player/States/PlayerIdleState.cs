using UnityEngine;
using TheLastHeir.Runtime.Entity;

namespace TheLastHeir.Runtime.States
{
    public class PlayerIdleState : PlayerGroundedState
    {
        public PlayerIdleState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }
        

        public override void Update()
        {
            base.Update(); 

            if (player.PlayerInput.move != Vector2.zero)
            {
                stateMachine.ChangeState(player.MoveState);

            }
            player.AnimationHandler.HandleLocomotion();
        }
    }
}