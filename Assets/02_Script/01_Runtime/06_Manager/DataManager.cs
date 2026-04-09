using UnityEngine;

namespace TheLastHeir.Runtime.Managers
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance { get; private set; }
        
        public int LastSpawnID { get; set; } = 0;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}