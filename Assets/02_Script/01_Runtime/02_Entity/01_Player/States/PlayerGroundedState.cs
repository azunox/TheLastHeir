using UnityEngine;
using TheLastHeir.Runtime.Entity;

namespace TheLastHeir.Runtime.States
{
    public class PlayerGroundedState : PlayerBaseState
    {
        protected PlayerAttributeHandler attributes;

        public PlayerGroundedState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
        {
            attributes = player.GetComponent<PlayerAttributeHandler>();
        }

        public override void Update()
        {
            base.Update();

            if (PlayerInputHandler.Instance.AttackTriggered)
            {
                if (attributes.UseStamina(player.playerStats.AttackStaminaCost))
                {
                    stateMachine.ChangeState(player.AttackState);
                    return;
                }
            }
            
            if (PlayerInputHandler.Instance.PotionTriggered)
            {
                stateMachine.ChangeState(player.UseItemState);
                return;
            }

            if (PlayerInputHandler.Instance.JumpTriggered)
            {
                if (attributes.UseStamina(player.playerStats.JumpStaminaCost))
                {
                    player.Movement.HandleJump();
                }
            }

            if (PlayerInputHandler.Instance.RollTriggered)
            {
                if (player.Movement.CanRoll && attributes.UseStamina(player.playerStats.RollStaminaCost))
                {
                    stateMachine.ChangeState(player.RollState);
                    return;
                }
            }

            player.Movement.Tick();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}