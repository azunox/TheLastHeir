using UnityEngine;
using TheLastHeir.Runtime.AnimationHashes;

namespace TheLastHeir.Runtime.Entity
{
    public class PlayerAnimationHandler : StandardEntityAnimationHandler
    {
        private Player player;

        [Header("Animation Smoothing")]
        [SerializeField] private float animationLerpSpeed = 10f;
        
        [Header("Animation CrossFade Durations")]
        [SerializeField] private float actionCrossFade = 0.2f;

        private const float InputThreshold = 0.1f;
        private const float WalkMagnitude = 1.0f;
        private const float SprintMagnitude = 2.0f;
        
        private Vector2 animationBlendVector;

        public void Initialize(Player owner)
        {
            player = owner;
        }

        public void HandleLocomotion()
        {
            Vector2 input = player.PlayerInput.move; 
            float targetMagnitude = 0f;
            
            if (input.magnitude > InputThreshold)
                targetMagnitude = player.PlayerInput.sprint ? SprintMagnitude : WalkMagnitude; 
            
            Vector2 targetVector = input.normalized * targetMagnitude;
            
            if (targetVector.sqrMagnitude < 0.001f)
            {
                animationBlendVector = Vector2.zero;
            }
            else
            {
                animationBlendVector = Vector2.Lerp(
                    animationBlendVector, 
                    targetVector, 
                    Time.deltaTime * animationLerpSpeed
                );
            }

            LerpMoveDirectionParameter(animationBlendVector.x, animationBlendVector.y);
            
            owner.animator.SetBool(CommonHashes.Grounded, player.Movement.IsGrounded);
        }

        public void PlayJumpAnimation()
        {
            owner.animator.CrossFadeInFixedTime(MovementHashes.JumpStartState, actionCrossFade);
            owner.animator.applyRootMotion = true;
        }
        
        public void PlayRollAnimation()
        {
            owner.animator.CrossFadeInFixedTime(MovementHashes.RollState, actionCrossFade);
        }

        public void PlayHitAnimation()
        {
            owner.animator.SetTrigger(AttackHashes.HitTrigger);
        }
        
        public void PlayUseItemAnimation()
        {
            owner.animator.SetTrigger(AttackHashes.UseItemTrigger);
        }
        
        public void PlayUseItemEndAnimation()
        {
            owner.animator.SetTrigger(AttackHashes.UseItemEndTrigger);
        }
        
        public void PlayRespawnAnimation()
        {
            owner.animator.SetTrigger(MovementHashes.RespawnTrigger);
        }
        
        public void PlayDeathAnimation()
        {
            owner.animator.SetTrigger(AttackHashes.DieTrigger);
        }
        
        public void PlayTargetActionByHash(int stateHash, bool applyRootMotion = false)
        {
            owner.animator.CrossFadeInFixedTime(stateHash, actionCrossFade);
            owner.animator.applyRootMotion = applyRootMotion;
        }

        public void SetTriggerByHash(int triggerHash)
        {
            owner.animator.SetTrigger(triggerHash);
        }

        public void SetIntegerByHash(int intHash, int value)
        {
            owner.animator.SetInteger(intHash, value);
        }
    }
}