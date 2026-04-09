using UnityEngine;
using UnityEngine.AI;

namespace TheLastHeir.Runtime.Entity.Enemy
{
    /// <summary>
    /// Enemy의 정보들을 담을 보따리 역할
    /// </summary>
    public class EnemyContext 
    {
        public Transform owner;
        public Transform target;
        public NavMeshAgent agent;
        public Animator anim;
        public Enemy controller;
        public float stateTimer;
        // 나중에 필요한 것들(Stats, Combat 등)을 여기에 추가
    }
}