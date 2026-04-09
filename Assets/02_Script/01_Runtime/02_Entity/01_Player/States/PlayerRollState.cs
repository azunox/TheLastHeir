using UnityEngine;
using TheLastHeir.Runtime.Entity;

namespace TheLastHeir.Runtime.States
{
    public class PlayerRollState : PlayerBaseState
    {
        private float _rollTimer;

        public PlayerRollState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
        {
        }

        public override void Enter()
        {
            _rollTimer = 0f;
            player.IsInvincible = true; 
            
            player.Movement.HandleRollInput(); 
        }

        public override void Update()
        {
            _rollTimer += Time.deltaTime;

            if (player.IsInvincible && _rollTimer >= player.playerStats.RollInvincibilityDuration)
            {
                player.IsInvincible = false;
            }

            if (_rollTimer >= player.playerStats.RollDuration)
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }

        public override void Exit()
        {
            player.IsInvincible = false;
            player.Movement.StopMovement();
        }
        
        public override void PhysicsUpdate()
        {
            player.Movement.Tick();
        }
    }
}