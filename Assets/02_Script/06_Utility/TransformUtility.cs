using UnityEngine;

namespace TheLastHeir.Utility
{                                                                           
    public static class TransformUtility
    {
        /// <summary>
        /// 특정 타겟을 바라보되, Y축 회전만 유지.
        /// </summary>
        public static void LookAtIgnoreY(Transform self, Vector3 targetPos)
        {
            Vector3 dir = targetPos - self.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.0001f)
                self.rotation = Quaternion.LookRotation(dir);
        }

        /// <summary>
        /// 두 지점 사이의 거리 (Y축 무시)
        /// </summary>
        public static float DistanceXZ(Vector3 a, Vector3 b)
        {
            a.y = b.y;
            return Vector3.Distance(a, b);
        }

        /// <summary>
        /// 두 위치 간의 XZ 평면 방향 벡터 (정규화)
        /// </summary>
        public static Vector3 DirectionXZ(Vector3 from, Vector3 to)
        {
            Vector3 dir = to - from;
            dir.y = 0f;
            return dir.sqrMagnitude > 0f ? dir.normalized : Vector3.zero;
        }

        /// <summary>
        /// 월드 기준 좌표를 로컬 좌표로 변환 (Transform이 null이면 자기 자신 반환)
        /// </summary>
        public static Vector3 WorldToLocal(Transform reference, Vector3 worldPos)
        {
            if (reference == null) return worldPos;
            return reference.InverseTransformPoint(worldPos);
        }

        /// <summary>
        /// 로컬 기준 좌표를 월드 좌표로 변환
        /// </summary>
        public static Vector3 LocalToWorld(Transform reference, Vector3 localPos)
        {
            if (reference == null) return localPos;
            return reference.TransformPoint(localPos);
        }

        /// <summary>
        /// 월드 좌표 기준으로 Transform이 특정 방향을 바라보게 회전시킴.
        /// </summary>
        public static void RotateTowards(Transform self, Vector3 direction, float rotationSpeed)
        {
            if (direction.sqrMagnitude < 0.0001f) return;
            Quaternion targetRot = Quaternion.LookRotation(direction);
            self.rotation = Quaternion.Slerp(self.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        /// <summary>
        /// 오브젝트를 일정한 높이(Y)로 스냅.
        /// </summary>
        public static void SnapHeight(Transform self, float height)
        {
            Vector3 pos = self.position;
            pos.y = height;
            self.position = pos;
        }

        /// <summary>
        /// 다른 Transform의 위치, 회전을 복사.
        /// </summary>
        public static void CopyTransform(Transform target, Transform source)
        {
            target.position = source.position;
            target.rotation = source.rotation;
            target.localScale = source.localScale;
        }
    }
}
