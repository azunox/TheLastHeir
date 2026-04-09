using UnityEngine;
using System.Collections.Generic;
using TheLastHeir.Runtime.Entity;
using TheLastHeir.Runtime.Managers;

namespace TheLastHeir.Runtime.World
{
    public class SpawnManager : MonoBehaviour
    {
        public static SpawnManager Instance { get; private set; }

        [SerializeField] private List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
            
            SpawnPoint[] points = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
            _spawnPoints.AddRange(points);
        }

        private void Start()
        {
            RespawnPlayerAtLastPoint();
        }

        public void RespawnPlayerAtLastPoint()
        {
            Player player = FindObjectOfType<Player>();
            if (player == null) return;
            
            int lastID = DataManager.Instance != null ? DataManager.Instance.LastSpawnID : 0;
            
            SpawnPoint targetPoint = _spawnPoints.Find(p => p.spawnID == lastID);
            
            if (targetPoint == null)
            {
                targetPoint = _spawnPoints.Find(p => p.spawnID == 0);
            }
            
            if (targetPoint != null)
            {
                if (player.cc != null) player.cc.enabled = false;

                player.transform.position = targetPoint.transform.position;
                
                Vector3 forward = targetPoint.spawnRotation != null ? targetPoint.spawnRotation.forward : targetPoint.transform.forward;
                player.transform.rotation = Quaternion.LookRotation(forward);

                if (player.cc != null) player.cc.enabled = true;
            }
            
            player.OnRespawn();
            
            ResetPlayerState(player);
        }

        public void SetSpawnPoint(int id)
        {
            PlayerPrefs.SetInt("LastSpawnID", id);
            PlayerPrefs.Save();
        }

        private void ResetPlayerState(Player player)
        {
            if (PlayerInputHandler.Instance != null)
            {
                PlayerInputHandler.Instance.canInput = true;
            }
            
            if (player.Attributes != null)
            {
                player.Attributes.CurHp = player.Attributes.MaxHp;
            }
            
            player.StateMachine.Initialize(player.IdleState);
            
        }
    }
}