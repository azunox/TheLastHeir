using System;
using UnityEngine;

namespace TheLastHeir.Runtime.Structs
{
    [Serializable]
    public struct HitInfo
    {
        public Damage Damage;
        public ElementType DefenderType;
        
        public Vector3 HitPoint; 
        public Vector3 HitNormal; 

        public HitInfo(Damage dmg, ElementType defType, Vector3 point, Vector3 normal)
        {
            Damage = dmg;
            DefenderType = defType;
            HitPoint = point;
            HitNormal = normal;
        }
    }
}