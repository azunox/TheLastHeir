using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;
using System.Text;
using System.Collections;

public class LoginManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    
    [Header("Scene Settings")]
    public string nextSceneName;

    [Header("API URL")]
    public string loginUrl;
    
    public void OnLoginButtonClicked()
    {
        GameObject idObj = GameObject.Find("InputID");
        GameObject pwObj = GameObject.Find("InputPW");

        TMP_InputField idInput = idObj?.GetComponent<TMP_InputField>();
        TMP_InputField pwInput = pwObj?.GetComponent<TMP_InputField>();

        if (idInput == null || pwInput == null)
        {
            Debug.LogError("[LoginManager] ID 또는 PW InputField를 찾을 수 없습니다. 이름을 확인해주세요. (InputID, InputPW)");
            return;
        }

        string id = idInput.text;
        string password = pwInput.text;

        StartCoroutine(TryLogin(id, password));
    }

    private IEnumerator TryLogin(string id, string password)
    {
        LoginRequest requestData = new LoginRequest
        {
            username = id,
            password = password
        };
        
        string json = JsonUtility.ToJson(requestData);
        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request = new UnityWebRequest(loginUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
            {

                // 토큰 파싱
                TokenResponse tokenData = JsonUtility.FromJson<TokenResponse>(request.downloadHandler.text);
                
                if (tokenData != null && !string.IsNullOrEmpty(tokenData.token))
                {
                    string jwtToken = tokenData.token;

                    PlayerPrefs.SetString("jwt_token", jwtToken);
                    PlayerPrefs.SetString("username", id);
                    PlayerPrefs.Save();

                    if (HttpServerClient.Instance != null)
                    {
                        HttpServerClient.Instance.FetchPlayerData(
                            jwtToken,
                            onSuccess: () => 
                            {
                                SceneManager.LoadScene(nextSceneName);
                            },
                            onFail: (errorMsg) =>
                            {
                                Debug.LogError($"[LoginManager] 데이터 로드 실패: {errorMsg}");
                            }
                        );
                    }
                    else
                    {
                        Debug.LogError("[LoginManager] HttpServerClient 인스턴스가 씬에 없습니다! 데이터 로드를 건너뛰고 씬을 이동합니다.");
                        SceneManager.LoadScene(nextSceneName);
                    }
                }
                else
                {
                    Debug.LogError("[LoginManager] 토큰 파싱 실패: 응답에 토큰이 없습니다.");
                }
            }
            else
            {
                Debug.LogWarning($"[LoginManager] 로그인 실패: {request.responseCode} - {request.error}");
                Debug.LogWarning($"[LoginManager] 서버 응답: {request.downloadHandler.text}");
            }
        }
    }
}
