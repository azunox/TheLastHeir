using UnityEngine;
using TheLastHeir.Runtime.Structs;

namespace TheLastHeir.Runtime.Entity
{
    public abstract class StandardEntityDamageHandler : EntityOwnedHandler<StandardEntity>
    {
        /// <summary>
        /// 히트 데이터를 받아 최종 피해를 계산하고 HP에 반영 ( 파생 클래스에서 구현 )
        /// </summary>
        public abstract void ApplyHit(in HitInfo hit, ElementMultiplier elementMultiplier);
    }
}