using System;
using UnityEngine;
using TheLastHeir.Runtime.Structs;

namespace TheLastHeir.Runtime.SO
{
    [CreateAssetMenu(fileName = "Damage Data", menuName = "TheLastHeir/SO/Damage Data")]
    public class DamageData : ScriptableObject
    {
        public Damage damage;
        public int totalDamage;

        private void OnValidate()
        {
        
        }
    }
}

