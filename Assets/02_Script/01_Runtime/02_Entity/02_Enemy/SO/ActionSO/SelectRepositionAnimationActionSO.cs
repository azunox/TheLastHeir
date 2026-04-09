using UnityEngine;
using System.Collections.Generic;

namespace TheLastHeir.Runtime.Entity.Enemy
{
    [CreateAssetMenu(menuName = "TheLastHeir/Enemy/FSM/Action/SelectRepositionAnimation")]
    public class SelectRepositionAnimationActionSO : ActionSO
    {
        [Header("Animation Names")]
        public string backstep;
        public string strafeLeft;
        public string strafeRight;

        [Header("Rule")]
        public float backstepIfCloserThan = 2.0f; // 너무 붙으면 뒤로
        [Range(0f, 1f)] public float backstepChance = 0.35f; // 거리 애매하면 확률로 backstep

        [Header("CrossFade")]
        public float crossFadeTime = 0.08f;

        [Header("Alternate Strafe")]
        public bool alternateStrafeDir = true;

        private class PerEnemy
        {
            public int lastStrafeDir = 1; // 1=Right, -1=Left
        }

        private readonly Dictionary<int, PerEnemy> _mem = new();

        public override void Execute(EnemyContext ctx)
        {
            if (ctx?.anim == null || ctx.owner == null) return;

            int id = ctx.owner.GetInstanceID();
            if (!_mem.TryGetValue(id, out var mem))
            {
                mem = new PerEnemy();
                _mem[id] = mem;
            }

            float dist = float.MaxValue;
            if (ctx.target != null)
            {
                Vector3 d = ctx.target.position - ctx.owner.position;
                d.y = 0f;
                dist = d.magnitude;
            }

            string chosen;

            bool forceBackstep = dist <= backstepIfCloserThan;
            if (forceBackstep)
            {
                chosen = backstep;
            }
            else
            {
                bool doBackstep = Random.value < backstepChance;
                if (doBackstep) chosen = backstep;
                else
                {
                    int dir = mem.lastStrafeDir;
                    if (alternateStrafeDir) dir *= -1;
                    else dir = Random.value < 0.5f ? -1 : 1;

                    mem.lastStrafeDir = dir;
                    chosen = (dir > 0) ? strafeRight : strafeLeft;
                }
            }

            ctx.anim.CrossFadeInFixedTime(chosen, crossFadeTime);
            ctx.anim.SetFloat("Vertical", 0f);
            ctx.anim.SetFloat("Horizontal", 0f);
        }
    }
}
