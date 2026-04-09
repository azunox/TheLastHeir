using System.Collections.Generic;
using UnityEngine;
using TheLastHeir.Runtime.Interfaces;
using TheLastHeir.Runtime.Structs;

namespace TheLastHeir.Runtime.Combat
{
    [RequireComponent(typeof(Collider))]
    public class WeaponDamageCollider : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Collider damageCollider;
        [SerializeField] private LayerMask targetLayer; 

        private readonly HashSet<GameObject> _alreadyHitObjects = new HashSet<GameObject>();
        
        private Damage _currentDamage;
        private GameObject _owner;

        private void Awake()
        {
            if (damageCollider == null) 
                damageCollider = GetComponent<Collider>();
            
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        public void EnableDamageCollider(Damage damage, GameObject owner)
        {
            _currentDamage = damage;
            _owner = owner;
            _alreadyHitObjects.Clear();
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
            _alreadyHitObjects.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _owner) return;
            if (((1 << other.gameObject.layer) & targetLayer) == 0) return;
            if (_alreadyHitObjects.Contains(other.gameObject)) return;

            IDamageable target = other.GetComponentInParent<IDamageable>() ?? 
                                 other.GetComponent<IDamageable>() ?? 
                                 other.GetComponentInChildren<IDamageable>();

            if (target != null)
            {
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = (transform.position - other.transform.position).normalized;

                HitInfo hitInfo = new HitInfo(_currentDamage, ElementType.Physical, hitPoint, hitNormal);
                
                target.OnTakeDamage(hitInfo);
                
                _alreadyHitObjects.Add(other.gameObject);
            }
        }
    }
}