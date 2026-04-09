using UnityEngine;

namespace TheLastHeir.Runtime.Entity.Enemy
{
    [CreateAssetMenu(menuName = "TheLastHeir/Enemy/FSM/Action/PlayAnimation")]
    public class PlayAnimationActionSO : ActionSO
    {
        public string animationName;

        public override void Execute(EnemyContext ctx)
        {
            if (ctx.anim == null) return;
            
            if (ctx.target != null)
            {
                Vector3 direction = (ctx.target.position - ctx.owner.position).normalized;
                direction.y = 0;
                
                if (direction != Vector3.zero)
                {
                    ctx.owner.rotation = Quaternion.LookRotation(direction);
                }
            }
            
            // CrossFade로 애니메이션 전환
            ctx.anim.CrossFadeInFixedTime(animationName, 0.1f);
            
            ctx.anim.SetFloat("Vertical", 0f);
            ctx.anim.SetFloat("Horizontal", 0f);
        }
    }
}