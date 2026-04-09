using UnityEngine;
using TheLastHeir.Runtime.Entity;
using TheLastHeir.Runtime.AnimationHashes;

namespace TheLastHeir.Runtime.States
{
    public class PlayerUseItemState : PlayerBaseState
    {
        private float _timer;
        private bool _isEffectApplied;
        private bool _hasTriggeredEnd; 
        private PotionItem _currentPotion;
        
        private float _drinkDuration = 1.0f; 
        private float _safetyTimeout = 5.0f;

        public PlayerUseItemState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
        {
        }

        public override void Enter()
        {
            _timer = 0f;
            _isEffectApplied = false;
            _hasTriggeredEnd = false;

            _currentPotion = player.GetComponent<PlayerInventoryHandler>().GetEquippedPotion();
            
            if (_currentPotion == null)
            {
                stateMachine.ChangeState(player.IdleState);
                return;
            }

            player.Movement.StopMovement();

            player.AnimationHandler.SetTriggerByHash(AttackHashes.UseItemTrigger);
        }

        public override void Update()
        {
            _timer += Time.deltaTime;

            if (_timer > _safetyTimeout)
            {
                stateMachine.ChangeState(player.IdleState);
                return;
            }

            AnimatorStateInfo info = player.animator.GetCurrentAnimatorStateInfo(0);

            if (info.IsTag("ItemUseLoop"))
            {
                if (!_isEffectApplied)
                {
                    ApplyPotionEffect();
                    _isEffectApplied = true;
                }

                if (!_hasTriggeredEnd && _timer >= _drinkDuration)
                {
                    player.AnimationHandler.PlayUseItemEndAnimation();
                    _hasTriggeredEnd = true;
                }
            }
            else if (info.IsTag("ItemUseEnd"))
            {
                if (info.normalizedTime >= 0.9f)
                {
                    stateMachine.ChangeState(player.IdleState);
                }
            }
        }

        private void ApplyPotionEffect()
        {
            if (_currentPotion == null) return;

            PlayerAttributeHandler stats = player.GetComponent<PlayerAttributeHandler>();
            if (stats != null)
            {
                if (_currentPotion.healthRecoveryAmount > 0)
                {
                    stats.CurHp += _currentPotion.healthRecoveryAmount;
                    if (DamageIndicatorManager.Instance != null)
                        DamageIndicatorManager.Instance.ShowDamage(-_currentPotion.healthRecoveryAmount, player.transform.position + Vector3.up, true, false);
                }
                
                if (_currentPotion.staminaRecoveryAmount > 0)
                {
                    stats.RestoreStamina(_currentPotion.staminaRecoveryAmount);
                }
            }
            
            player.GetComponent<PlayerInventoryHandler>().ConsumeCurrentPotion();
        }
    }
}