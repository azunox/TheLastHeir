using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheLastHeir.Runtime.Structs
{
    public enum StatusEffectType
    {
        None,
        Bleed,
        Poison,
    }

    public enum StatusStackingMode
    {
        None,
        Additive, // 단순 합산
        Multiplicative // 곱연산으로 배율 강화
    }

    [Serializable]
    public struct StatusEffectSpec
    {
        public StatusEffectType Type;
        public float TickDamage;
        public float Duration;
        public float Interval;
        // public bool CanStack;
        // public int MaxStacks;
        public StatusStackingMode StackingMode;
        // public float PerStackAdd;
        // public float PerStackMul;
    }

    [Serializable]
    public struct StatusResistances
    {
        [Range(0f, 1f)] public float Bleed;
        [Range(0f, 1f)] public float Poison;

        public static StatusResistances operator *(StatusResistances a, StatusResistances b)
        {
            // Description : 더 강한 저항치를 반환하기 위해 Max() 사용
            return new StatusResistances
            {
                Bleed = Mathf.Max(a.Bleed, b.Bleed), 
                Poison = Mathf.Max(a.Poison, b.Poison),
            };
        }
    }

    [Serializable]
    public struct StatusRuntime
    {
        public StatusEffectType Type;
        public float Remaining;
        public float Interval;
        public float NextTickPoint; // 다음 틱 발생 시점
        // public int Stacks;

        public static StatusRuntime operator +(StatusRuntime a, StatusRuntime b)
        {
            if (a.Type != b.Type) return a;
            return new StatusRuntime
            {
                Type = a.Type,
                // Description : 더 긴 지속시간을 우선시하기 위해 Max() 사용
                Remaining = Mathf.Max(a.Remaining, b.Remaining), 
                Interval = a.Interval,
                // Description : 더 빠른 틱 발생 시점을 우선시하기 위해 Min() 사용
                NextTickPoint = Mathf.Min(a.NextTickPoint, b.NextTickPoint),
                // Stacks = a.Stacks + b.Stacks
            };
        }
    }

    [CreateAssetMenu(menuName = "Status/StatusEffectLibrary", fileName = "StatusEffectLibrary")]
    public class StatusEffectLibrary : ScriptableObject
    {
        [SerializeField] private StatusEffectSpec[] specs = Array.Empty<StatusEffectSpec>();
        private Dictionary<StatusEffectType, StatusEffectSpec> _map;

        public bool TryGet(StatusEffectType type, out StatusEffectSpec spec)
        {
            EnsureMap();
            return _map.TryGetValue(type, out spec);
        }

        private void OnEnable() => Rebuild();
#if UNITY_EDITOR
        private void OnValidate() => Rebuild();
#endif
        private void EnsureMap()
        {
            if (_map == null) Rebuild();
        }

        private void Rebuild()
        {
            _map = new Dictionary<StatusEffectType, StatusEffectSpec>(specs?.Length ?? 0);
            if (specs == null) return;

            for (int i = 0; i < specs.Length; i++)
            {
                var s = specs[i];
                _map[s.Type] = s;
            }
        }
    }
}
