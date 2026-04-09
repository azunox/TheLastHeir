using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IDManager : MonoBehaviour
{
    [Header("UI Components")]
    public TMP_InputField userIdInputField;
    public Toggle rememberMeToggle;

    private const string USER_ID_KEY = "UserID";
    private const string REMEMBER_ME_KEY = "RememberMe";

    void Start()
    {
        LoadUserId();
    }
    
    public void OnLoginButtonClicked()
    {
        if (rememberMeToggle.isOn)
        {
            PlayerPrefs.SetString(USER_ID_KEY, userIdInputField.text);
            PlayerPrefs.SetInt(REMEMBER_ME_KEY, 1);
        }
        else
        {
            PlayerPrefs.DeleteKey(USER_ID_KEY);
            PlayerPrefs.SetInt(REMEMBER_ME_KEY, 0);
        }
        PlayerPrefs.Save();
    }

    private void LoadUserId()
    {
        if (PlayerPrefs.GetInt(REMEMBER_ME_KEY, 0) == 1)
        {
            userIdInputField.text = PlayerPrefs.GetString(USER_ID_KEY, "");
            rememberMeToggle.isOn = true;
        }
        else
        {
            userIdInputField.text = "";
            rememberMeToggle.isOn = false;
        }
    }
}