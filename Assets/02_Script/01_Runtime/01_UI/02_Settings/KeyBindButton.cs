using UnityEngine;
using UnityEngine.UI;

public class KeyBindButton : MonoBehaviour
{
    public string actionName; 
    public Text keyText; 
    public Button button;

    private KeyRebinder keyRebinder;

    void Start()
    {
        keyRebinder = KeyRebinder.Instance;
        UpdateKeyText();
        button.onClick.AddListener(OnClick);
    }

    public void UpdateKeyText()
    {
        if (keyRebinder == null)
        {
            keyRebinder = KeyRebinder.Instance;
            if (keyRebinder == null)
            {
                keyText.text = "None";
                return;
            }
        }
        KeyCode key = keyRebinder.GetKeyForAction(actionName);
        keyText.text = key.ToString();
    }

    public void OnClick()
    {
        keyRebinder.StartRebind(actionName, UpdateKeyText);
    }
}

