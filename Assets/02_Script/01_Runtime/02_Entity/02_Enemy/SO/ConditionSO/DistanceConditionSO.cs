using UnityEngine;

namespace TheLastHeir.Runtime.Entity.Enemy
{
    [CreateAssetMenu(menuName = "TheLastHeir/Enemy/FSM/Condition/DistanceCondition")]
    public class DistanceConditionSO : ConditionSO
    {
        public float distanceThreshold;
        [Header("[ true : 가까이 / false : 멀리 ]")]
        public bool nearby = true; // 가까이 할건지 멀리 할건지

        public override bool Evaluate(EnemyContext ctx)
        {
            if (ctx.target == null || ctx.owner == null) return false; // 예방
                
            // (내 위치 - 타겟 위치)
            Vector3 distance = ctx.owner.position - ctx.target.position;
            distance.y = 0; // x, y 만 비교

            float dist = distance.sqrMagnitude;
            float threshold = distanceThreshold * distanceThreshold;
            
            bool result = nearby ? (dist <= threshold) : (dist > threshold);
            
            // Vector3.Distance는 제곱근 계산을 하기에 Enemy 수가 많다면 성능 저하 우려, 제곱값 비교(sqrMagnitude)로 대체
            if (nearby) return dist <= (threshold);
            
            return dist > (threshold);
        }
    }
}