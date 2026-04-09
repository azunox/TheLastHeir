using UnityEngine;

namespace TheLastHeir.Runtime.AnimationHashes
{
    public static class AttackHashes
    {
        public static readonly int AttackTrigger = Animator.StringToHash("Attack");
        public static readonly int AttackCount = Animator.StringToHash("AttackCount");
        public static readonly int HitTrigger = Animator.StringToHash("Hit");
        public static readonly int DieTrigger = Animator.StringToHash("Die");
        
        public static readonly int UseItemTrigger = Animator.StringToHash("UseItem");
        public static readonly int UseItemEndTrigger = Animator.StringToHash("UseItemEnd");
    }
}