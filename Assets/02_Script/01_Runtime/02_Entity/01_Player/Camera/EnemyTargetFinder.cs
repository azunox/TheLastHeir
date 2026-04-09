using UnityEngine;

namespace TheLastHeir.Runtime.Combat
{
    public class EnemyTargetFinder : ITargetFinder
    {
        private readonly float _maxViewAngle;
        
        public EnemyTargetFinder(float maxViewAngle = 45f)
        {
            _maxViewAngle = maxViewAngle;
        }

        public Transform FindTarget(Transform origin, Transform cameraTransform, float radius, LayerMask targetLayer)
        {
            Collider[] colliders = Physics.OverlapSphere(origin.position, radius, targetLayer);
            
            Transform bestTarget = null;
            float closestAngleFromCenter = float.MaxValue;

            foreach (var col in colliders)
            {
                if (col.transform.root == origin.root) continue;
                
                Vector3 directionFromCamera = (col.transform.position - cameraTransform.position).normalized;
                
                float angle = Vector3.Angle(cameraTransform.forward, directionFromCamera);
                if (angle < _maxViewAngle)
                {
                    if (angle < closestAngleFromCenter)
                    {
                        closestAngleFromCenter = angle;
                        bestTarget = col.transform;
                    }
                }
            }

            return bestTarget;
        }
    }
}