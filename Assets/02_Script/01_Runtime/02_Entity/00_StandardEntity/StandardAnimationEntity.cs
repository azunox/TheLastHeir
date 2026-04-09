using TheLastHeir.Runtime.AnimationHashes;
using UnityEngine;

namespace TheLastHeir.Runtime.Entity {

    public abstract class StandardEntityAnimationHandler : EntityOwnedHandler<StandardEntity> {
        
        private float moveDirectionLerpSpeed = 6;
        private Vector2 MoveDirectionParam;
    
        public void LerpMoveDirectionParameter(float horizontal, float vertical) 
        {
            MoveDirectionParam = Vector2.Lerp(MoveDirectionParam, new Vector2(horizontal, vertical), Time.deltaTime * moveDirectionLerpSpeed);
            
            owner.animator.SetFloat(CommonHashes.Horizontal, MoveDirectionParam.x);
            owner.animator.SetFloat(CommonHashes.Vertical, MoveDirectionParam.y);
        }
    
    }

}