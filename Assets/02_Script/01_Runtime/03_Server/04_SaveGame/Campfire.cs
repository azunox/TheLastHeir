using UnityEngine;
using TheLastHeir.Runtime.Network;
using TheLastHeir.Runtime.World;

namespace TheLastHeir.Runtime.World
{
    public class Campfire : MonoBehaviour
    {
        [Header("Campfire Settings")]
        public int campfireID = 0;
        public Transform sitPosition;

        private bool isSitting = false;

        public void Interact()
        {
            if (isSitting) return;
            SitDown();
        }

        private void SitDown()
        {
            isSitting = true;
            
            if (SpawnManager.Instance != null) SpawnManager.Instance.SetSpawnPoint(campfireID);
            
            if (GameDataCollector.Instance != null)
            {
                PlayerDataResponse snapshot = GameDataCollector.Instance.CollectCurrentData();
                snapshot.location.campfireNumber = campfireID;

                if (HttpServerClient.Instance != null)
                {
                    HttpServerClient.Instance.SavePlayerData(
                        snapshot,
                        onSuccess: () => Debug.Log("서버 저장 완료"),
                        onFail: (err) => Debug.LogError("서버 저장 실패: " + err)
                    );
                }
            }
        }
    }
}