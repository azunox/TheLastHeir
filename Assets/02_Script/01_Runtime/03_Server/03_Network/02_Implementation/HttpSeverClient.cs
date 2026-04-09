using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using TheLastHeir.Runtime.Network;

public class HttpServerClient : MonoBehaviour
{
    public static HttpServerClient Instance { get; private set; }
    
    private const string BASE_URL = "https://example.com/api"; 
    
    public PlayerDataResponse CurrentPlayerData { get; private set; }

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

    /// <summary>
    /// 새 게임 생성 요청을 보냄 (외부에서 호출되는 함수)
    /// </summary>
    public void SendNewGameRequest(string nickname)
    {
        StartCoroutine(SendNewGameCoroutine(nickname));
    }

    public void SendLoadGameRequest(string nickname)
    {
        StartCoroutine(SendLoadGameCoroutine(nickname));
    }

    private IEnumerator SendNewGameCoroutine(string nickname)
    {
        string url = $"{BASE_URL}/newgame";
        
        WWWForm form = new WWWForm();
        form.AddField("nickname", nickname);
        
        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            yield return request.SendWebRequest();
            
#if UNITY_2020_1_OR_NEWER
            if (request.result == UnityWebRequest.Result.Success)
#else
            if (!request.isNetworkError && !request.isHttpError)
#endif
            {
                Debug.Log("새 게임 생성 성공");
            }
            else
            {
                Debug.LogError($"새 게임 생성 실패: {request.error}");
            }
        }
    }
    
    private IEnumerator SendLoadGameCoroutine(string nickname)
    {
        string url = $"{BASE_URL}/loadgame?nickname={UnityWebRequest.EscapeURL(nickname)}";
        
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (request.result == UnityWebRequest.Result.Success)
#else
            if (!request.isNetworkError && !request.isHttpError)
#endif
            {
                Debug.Log($"게임 불러오기 성공: {request.downloadHandler.text}");
            }
            else
            {
                Debug.LogError($"게임 불러오기 실패: {request.error}");
            }
        }
    }
    
    public void FetchPlayerData(string token, System.Action onSuccess, System.Action<string> onFail = null)
    {
        StartCoroutine(FetchPlayerDataCoroutine(token, onSuccess, onFail));
    }

    private IEnumerator FetchPlayerDataCoroutine(string token, System.Action onSuccess, System.Action<string> onFail)
    {
        string url = $"{BASE_URL}/player/data"; 

        Debug.Log($"[HttpServerClient] 데이터 요청 시작: {url}");

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (request.result == UnityWebRequest.Result.Success)
#else
            if (!request.isNetworkError && !request.isHttpError)
#endif
            {
                string json = request.downloadHandler.text;
                Debug.Log($"[HttpServerClient] 데이터 수신 성공: {json}");

                try
                {
                    CurrentPlayerData = JsonUtility.FromJson<PlayerDataResponse>(json);
                    
                    onSuccess?.Invoke(); 
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[HttpServerClient] JSON 파싱 에러: {e.Message}");
                    onFail?.Invoke("JSON Parsing Error");
                }
            }
            else
            {
                Debug.LogError($"[HttpServerClient] 요청 실패: {request.responseCode} - {request.error}");
                onFail?.Invoke(request.error);
            }
        }
    }

    /// <summary>
    /// 현재 게임 데이터를 서버로 전송하여 저장합니다.
    /// </summary>
    public void SavePlayerData(PlayerDataResponse dataToSave, System.Action onSuccess, System.Action<string> onFail = null)
    {
        string token = PlayerPrefs.GetString("jwt_token", "");
        
        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("토큰 없음");
            onFail?.Invoke("No Auth Token Found");
            return;
        }

        StartCoroutine(SavePlayerDataCoroutine(token, dataToSave, onSuccess, onFail));
    }

    private IEnumerator SavePlayerDataCoroutine(string token, PlayerDataResponse data, System.Action onSuccess, System.Action<string> onFail)
    {
        string url = $"{BASE_URL}/player/save";
        string json = JsonUtility.ToJson(data);
        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

        Debug.Log($"[HttpServerClient] 데이터 저장 시도 중 (크기: {jsonBytes.Length} bytes)");

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            
            request.SetRequestHeader("Authorization", "Bearer " + token);
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();
            
#if UNITY_2020_1_OR_NEWER
            if (request.result == UnityWebRequest.Result.Success)
#else
            if (!request.isNetworkError && !request.isHttpError)
#endif
            {
                Debug.Log("[HttpServerClient] 데이터 저장 성공");
                onSuccess?.Invoke();
            }
            else
            {
                Debug.LogError($"[HttpServerClient] 저장 실패: {request.responseCode} - {request.error}");
                onFail?.Invoke(request.error);
            }
        }
    }
}