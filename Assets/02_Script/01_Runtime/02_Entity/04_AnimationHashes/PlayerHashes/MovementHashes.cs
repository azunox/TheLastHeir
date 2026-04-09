using UnityEngine;

namespace TheLastHeir.Runtime.AnimationHashes
{
    public static class MovementHashes
    {
        public static readonly int JumpStartState = Animator.StringToHash("JumpStart");
        public static readonly int RollState = Animator.StringToHash("Roll");
        public static readonly int RespawnTrigger = Animator.StringToHash("Respawn");
    }
}