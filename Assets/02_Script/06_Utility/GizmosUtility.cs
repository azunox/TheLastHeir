using UnityEngine;

namespace TheLastHeir.Utility
{
    public static class GizmosUtility
    {
        private static readonly Color DefaultColor = new Color(0.1f, 1f, 0.1f, 0.65f);

        /// <summary>
        /// 지정한 Collider 타입에 따라 자동으로 Gizmo를 그림.
        /// </summary>
        public static void DrawColliderGizmo(Collider collider, Color? color = null)
        {
#if UNITY_EDITOR
            if (collider == null) return;

            Gizmos.color = color ?? DefaultColor;

            switch (collider)
            {
                case BoxCollider box:
                    DrawBoxCollider(box);
                    break;
                case SphereCollider sphere:
                    DrawSphereCollider(sphere);
                    break;
                case CapsuleCollider capsule:
                    DrawCapsuleCollider(capsule);
                    break;
                case CharacterController cc:
                    DrawCharacterController(cc);
                    break;
            }

            Gizmos.color = Color.white;
#endif
        }

#if UNITY_EDITOR
        private static void DrawBoxCollider(BoxCollider box)
        {
            Gizmos.matrix = box.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(box.center, box.size);
        }

        private static void DrawSphereCollider(SphereCollider sphere)
        {
            Gizmos.matrix = sphere.transform.localToWorldMatrix;
            Gizmos.DrawWireSphere(sphere.center, sphere.radius);
        }

        private static void DrawCapsuleCollider(CapsuleCollider capsule)
        {
            Gizmos.matrix = capsule.transform.localToWorldMatrix;

            Vector3 up, down;
            // Set Direction
            switch (capsule.direction)
            {
                case 0: // X축 
                    up = Vector3.left;
                    down = Vector3.right;
                    break;
                case 1: // Y축 
                    up = Vector3.up;
                    down = Vector3.down;
                    break;
                case 2: // Z축
                    up = Vector3.forward;
                    down = Vector3.back;
                    break;
                default:
                    up = Vector3.up;
                    down = Vector3.down;
                    break;
            }
            
            // 상단 하단 위치 계산
            float halfHeight = Mathf.Max(0, capsule.height / 2f - capsule.radius);
            Vector3 top = capsule.center + up * halfHeight;
            Vector3 bottom = capsule.center + down * halfHeight;

            Gizmos.DrawWireSphere(top, capsule.radius);
            Gizmos.DrawWireSphere(bottom, capsule.radius);

            Color faded = Gizmos.color; faded.a *= 0.5f;
            Gizmos.color = faded;

            int steps = Mathf.Max(4, Mathf.RoundToInt(capsule.height / capsule.radius));
            
            // 중간 원통 부분
            for (int i = 1; i < steps; i++)
            {
                Vector3 pos = Vector3.Lerp(top, bottom, i / (float)steps);
                Gizmos.DrawWireSphere(pos, capsule.radius);
            }

            Gizmos.color = Color.white;
            Gizmos.DrawLine(top, bottom);
        }


        private static void DrawCharacterController(CharacterController cc)
        {
            Gizmos.matrix = cc.transform.localToWorldMatrix;

            float halfHeight = Mathf.Max(0, cc.height / 2f - cc.radius);
            Vector3 top = cc.center + Vector3.up * halfHeight;
            Vector3 bottom = cc.center + Vector3.down * halfHeight;

            Gizmos.DrawWireSphere(top, cc.radius);
            Gizmos.DrawWireSphere(bottom, cc.radius);

            Color faded = Gizmos.color; faded.a *= 0.5f;
            Gizmos.color = faded;

            int steps = Mathf.Max(4, Mathf.RoundToInt(cc.height / cc.radius));
            for (int i = 1; i < steps; i++)
            {
                Vector3 pos = Vector3.Lerp(top, bottom, i / (float)steps);
                Gizmos.DrawWireSphere(pos, cc.radius);
            }

            Gizmos.color = Color.white;
            Gizmos.DrawLine(top, bottom);
        }
#endif
    }
}
