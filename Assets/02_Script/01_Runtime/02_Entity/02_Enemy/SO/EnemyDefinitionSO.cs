using UnityEngine;

namespace TheLastHeir.Runtime.Entity.Enemy
{
    [CreateAssetMenu(menuName = "TheLastHeir/Enemy/FSM/Definition")]
    public class EnemyDefinitionSO : ScriptableObject
    {
        [Header("[ Enemy's Stat ]")] 
        public EnemyStatSO enemyStat;
        
        [Header("[ Enemy's State ]")] 
        [Tooltip("Enemy가 처음 시작하는 State, 그 이후의 State는 Chain 형식으로 이어짐")]
        public StateSO initialState;
        public StateSO hitState;
        public StateSO dieState;
        
        [Header("[ Enemy's FSM Graph ]")]
        public FSMGraphSO fsmGraph;
    }
}