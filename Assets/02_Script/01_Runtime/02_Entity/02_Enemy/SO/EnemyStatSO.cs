using UnityEngine;
using TheLastHeir.Runtime.Structs;

namespace TheLastHeir.Runtime.Entity.Enemy
{
    [CreateAssetMenu(menuName = "TheLastHeir/Enemy/FSM/Stat")]
    public class EnemyStatSO : ScriptableObject
    {
        [Header("[ Base Stats ]")]
        public int maxHealth;
        public float baseDamage;

        [Header("[ Defence ]")] 
        public DamageNegation damageNegation;
        // 기타 스탯들은 추후 추가
    }
}