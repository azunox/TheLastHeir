using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class ApiClient : MonoBehaviour
{
    [SerializeField] private string url;
    public string newNickname;
    
    public void OnChangeNicknameButtonClicked()
    {
        StartCoroutine(UpdateNickname(newNickname)); ;
    }
    
    public IEnumerator UpdateNickname(string newNickname)
    {
        string token = PlayerPrefs.GetString("jwt_token", "");
        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("저장된 토큰이 없습니다. 로그인 후 시도하세요.");
            yield break;
        }

        // 안전하게 JSON 만들기
        var payload = new UsernameUpdateRequest { username = newNickname };
        string jsonBody = JsonUtility.ToJson(payload);

        UnityWebRequest request = new UnityWebRequest(url, "PATCH");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + token);
        
        yield return request.SendWebRequest();

        if (request.responseCode == 200)
            Debug.Log("닉네임 변경 성공: " + request.downloadHandler.text);
        else
            Debug.LogWarning($"닉네임 변경 실패: {request.responseCode} - {request.downloadHandler.text}");
    }
}

[System.Serializable]
public class UsernameUpdateRequest
{
    public string username;
}