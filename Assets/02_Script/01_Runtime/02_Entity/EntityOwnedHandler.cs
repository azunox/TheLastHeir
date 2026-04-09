using UnityEngine;

namespace TheLastHeir.Runtime.Entity
{
    public abstract class EntityOwnedHandler<T> : MonoBehaviour where T : StandardEntity
    {
        protected T owner{ get; private set; }

        protected virtual void Awake()
        {
            owner = GetComponent<T>();
            
            if (owner == null)
            {
                Debug.LogError("owner is null");
            }
        }
    }
}
