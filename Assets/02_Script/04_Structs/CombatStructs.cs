using System;
using System.Collections.Generic;
using UnityEngine;
using TheLastHeir.Utility;

namespace TheLastHeir.Runtime.Structs
{
    public enum ElementType
    {
        Physical,
        Magic,
        Holy,
        Dark
    }

    [Serializable]
    public struct Damage
    {
        [Header("Damage Data")] public ElementType Type;
        public float Amount;

        /// <summary>
        /// 동일 속성의 데미지 합산(Type이 다르면 'param, a' 반환)
        /// </summary>
        public static Damage operator +(Damage a, Damage b)
        {
            if (a.Type != b.Type)
            {
#if UNITY_EDITOR
                DebugUtility.LogWarning($"[Damage.+] Type mismatch: {a.Type} + {b.Type}");
#endif
                return a;
            }

            return new Damage
            {
                Type = a.Type,
                Amount = a.Amount + b.Amount
            };
        }

        // Description : 음수 데미지 보정, 안전장치
        public Damage ClampedNonNegative()
            => new Damage { Type = Type, Amount = Mathf.Max(0f, Amount) };
    }

    [Serializable]
    public struct DamageNegation
    {
        [Range(0f, 1f)] public float Physical;
        [Range(0f, 1f)] public float Magic;

        public float GetNegation(ElementType type)
        {
            return type switch
            {
                ElementType.Physical => Physical,
                ElementType.Magic => Magic,
                _ => 0f 
            };
        }

        public static DamageNegation operator *(DamageNegation a, DamageNegation b)
        {
            return new DamageNegation
            {
                Physical = a.Physical + (1f - a.Physical) * b.Physical,
                Magic = a.Magic + (1f - a.Magic) * b.Magic
            };
        }

        public static DamageNegation operator /(DamageNegation a, DamageNegation b)
        {
            float Inv(float combined, float remove)
            {
                if (remove >= 0.999999f) return 0f;
                float num = 1f - combined;
                float den = 1f - remove;
                float x = 1f - (num / Mathf.Max(den, 1e-6f));
                return Mathf.Clamp01(x);
            }

            return new DamageNegation
            {
                Physical = Inv(a.Physical, b.Physical),
                Magic = Inv(a.Magic, b.Magic)
            };
        }
    }

    [Serializable]
    public struct ElementMultiplierEntry
    {
        public ElementType Element;
        [Range(0.1f, 5f)] public float Multiplier;
    }

    [CreateAssetMenu(menuName = "Combat/ElementMultiplier", fileName = "ElementMultiplier")]
    public class ElementMultiplier : ScriptableObject
    {
        [SerializeField] private ElementMultiplierEntry[] entries = Array.Empty<ElementMultiplierEntry>();
        [SerializeField] private float defaultMultiplier = 1f;

        private Dictionary<ElementType, float> _cache;

        public float GetMultiplier(ElementType element)
        {
            EnsureCache();
            return _cache.TryGetValue(element, out var mul) ? mul : defaultMultiplier;
        }

        private void OnEnable() => RebuildCache();

#if UNITY_EDITOR
        private void OnValidate() => RebuildCache();
#endif

        private void EnsureCache()
        {
            if (_cache == null)
                RebuildCache();
        }

        private void RebuildCache()
        {
            _cache = new Dictionary<ElementType, float>(entries.Length);
            foreach (var e in entries)
                if (!_cache.ContainsKey(e.Element))
                    _cache[e.Element] = e.Multiplier;
        }
    }
}
