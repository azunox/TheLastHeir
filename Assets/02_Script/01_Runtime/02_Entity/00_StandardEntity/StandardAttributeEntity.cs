using UnityEngine;
using TheLastHeir.Runtime.Structs;

namespace TheLastHeir.Runtime.Entity
{
    public abstract class StandardAttributeEntity : EntityOwnedHandler<StandardEntity>
    {
        public abstract int MaxHp { get; set; }
        
        public abstract int CurHp { get; set; }
        
        public abstract DamageNegation DamageNegation { get; protected set; }
        
        public abstract ElementType Type { get; protected set; }
        
        public abstract StatusResistances StatusResistances { get; protected set; }

        public abstract void TakeDamage(float amount);
    }
}