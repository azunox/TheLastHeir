using UnityEngine;

namespace TheLastHeir.Runtime.Combat
{
    public interface ITargetFinder
    {
        Transform FindTarget(Transform origin, Transform cameraTransform, float radius, LayerMask targetLayer);
    }
}