using UnityEngine;

namespace TheLastHeir.Runtime.World
{
    public class SpawnPoint : MonoBehaviour
    {
        public int spawnID;
        
        public Transform spawnRotation; 
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
            
            Gizmos.color = Color.blue;
            Vector3 direction = spawnRotation != null ? spawnRotation.forward : transform.forward;
            Gizmos.DrawRay(transform.position, direction * 2f);
        }
    }
}