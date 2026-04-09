using System.Linq;
using UnityEngine;
using TheLastHeir.Runtime.Entity;

namespace TheLastHeir.Utility
{
    // Rigidbody는 Collider 위에 붙는 물리 컴포넌트이므로, Rigidbody 유무와 상관없이 Collider로만 충돌 무시 처리를 합니다.
    public static class PhysicsUtility
    {
        /// <summary>
        /// 지정한 엔티티의 자식 콜라이더들과 타겟 콜라이더 간의 충돌을 무시합니다.
        /// </summary>
        public static void IgnoreCollisionUtil(StandardEntity entity, Collider target)
        {
            Collider[] cols = entity.GetComponentsInChildren<Collider>(true);
            foreach (var col in cols)
                Physics.IgnoreCollision(col, target);

            // Player의 방어 콜라이더가 있을 경우 추가 예정
        }

        /// <summary>
        /// 지정한 엔티티의 자식 콜라이더들 간의 충돌을 무시합니다.
        /// </summary>
        public static void SetUpIgnoreBodyCollision(StandardEntity entity)
        {
            Collider[] cols = entity.GetComponentsInChildren<Collider>(true);
            foreach (var c1 in cols)
            foreach (var c2 in cols)
                Physics.IgnoreCollision(c1, c2);
        }

        
        /// <summary>
        /// 지정한 루트 자식 콜라이더들의 활성화 상태를 설정합니다.
        /// </summary>
        public static void SetActiveChildrenColliders(Transform root, bool active, int layerMask = ~0, bool includeInactive = false)
        {
            var cols = root.GetComponentsInChildren<Collider>(includeInactive) // 배열을 반환
                // Description : 배열을 반환하는 타입이기에, LINQ 사용 가능, 앞에 "."은 메서드 체이닝
                .Where(c => layerMask == (layerMask | (1 << c.gameObject.layer))) // layerMask 안에 포함된 콜라이더만 선택
                .ToArray();

            foreach (var col in cols)
                col.enabled = active;
        }
    }
}