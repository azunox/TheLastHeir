using UnityEngine;
using TheLastHeir.Runtime.Entity;

namespace TheLastHeir.Runtime.States
{
    public abstract class PlayerBaseState
    {
        protected PlayerStateMachine stateMachine;
        protected Player player;

        protected PlayerBaseState(Player player, PlayerStateMachine stateMachine)
        {
            this.player = player;
            this.stateMachine = stateMachine;
        }

        public virtual void Enter() { }

        public virtual void Update() { }

        public virtual void PhysicsUpdate() { }

        public virtual void Exit() { }

        public virtual void AnimationTriggerEvent(int triggerHash) { }
    }
}