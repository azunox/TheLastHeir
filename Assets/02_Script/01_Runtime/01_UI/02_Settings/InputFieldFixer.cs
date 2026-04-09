using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldFixer : MonoBehaviour
{
    // Text의 값이 채워져도 Placeholder가 사라지지 않는 문제 해결
    public TMP_InputField inputID;
    public TMP_InputField inputPW;
    
    public void OnIDValueChanged()
    {
        TogglePlaceholder(inputID, inputID.text);
    }

    public void OnPWValueChanged()
    {
        TogglePlaceholder(inputPW, inputPW.text);
    }

    private void TogglePlaceholder(TMP_InputField inputField, string text)
    {
        if (inputField != null && inputField.placeholder != null)
        {
            inputField.placeholder.gameObject.SetActive(string.IsNullOrEmpty(text));
        }
    }
}