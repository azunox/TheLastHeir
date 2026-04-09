using UnityEngine;
using TheLastHeir.Runtime.Structs;

namespace TheLastHeir.Runtime.Entity.Enemy
{
    public class EnemyAttributeEntity : StandardAttributeEntity
    {
        private int _maxHp;
        private int _curHp;

        public override int MaxHp { get => _maxHp; set => _maxHp = value; }
        public override int CurHp { get => _curHp; set => _curHp = value; }
        
        public override DamageNegation DamageNegation { get; protected set; }
        public override ElementType Type { get; protected set; }
        public override StatusResistances StatusResistances { get; protected set; }

        // Enemy.cs에서 호출
        public void Initialize(int maxHp)
        {
            _maxHp = maxHp;
            _curHp = maxHp; // 시작 시 풀피
        }

        public override void TakeDamage(float amount)
        {
            if (CurHp <= 0) return;

            CurHp -= Mathf.RoundToInt(amount);
            CurHp = Mathf.Clamp(CurHp, 0, MaxHp);

            Debug.Log($"[{owner.name}] 체력 감소: {CurHp}/{MaxHp}");
        }
    }
}