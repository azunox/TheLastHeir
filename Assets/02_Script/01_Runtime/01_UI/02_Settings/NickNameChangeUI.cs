using UnityEngine;
using TMPro;  
using UnityEngine.UI;

public class NicknameChangeUI : MonoBehaviour
{
    public TMP_InputField nicknameInputField; 
    public ApiClient apiClient;

    public void OnChangeNicknameButtonClicked()
    {
        string input = nicknameInputField.text.Trim();
        if (string.IsNullOrEmpty(input))
        {
            return;
        }
        apiClient.newNickname = input;
    }
}