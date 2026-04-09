using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class KeyRebinder : MonoBehaviour
{
    public static KeyRebinder Instance { get; private set; }

    public Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>();
    private bool waitingForKey = false;
    private string currentAction = "";
    private Action onRebindComplete;
    [SerializeField] private Text keyTextPrefab;
    [SerializeField] private Text waringTextPrefab;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        SetDefaultBindings();

        // 저장된 값이 있으면 덮어쓰기
        LoadAllKeys();

        KeyBindButton[] buttons = FindObjectsOfType<KeyBindButton>();
        foreach (var b in buttons) b.UpdateKeyText();

        keyTextPrefab.gameObject.SetActive(false);
        waringTextPrefab.gameObject.SetActive(false);
    }

    void SetDefaultBindings()
    {
        // 인게임 기본 키
        keyBindings["moveForward"] = KeyCode.W;
        keyBindings["moveBackward"] = KeyCode.S;
        keyBindings["moveLeft"] = KeyCode.A;
        keyBindings["moveRight"] = KeyCode.D;
        keyBindings["Jump"] = KeyCode.Space;
        keyBindings["Block"] = KeyCode.F;
        keyBindings["Escape"] = KeyCode.Escape;
        keyBindings["Buy"] = KeyCode.Return;
        
        // 변경 가능한 키
        keyBindings["Interaction"] = KeyCode.E;
        keyBindings["Roll"] = KeyCode.Q;
        keyBindings["Attack"] = KeyCode.Mouse0;
        keyBindings["Heavy Attack"] = KeyCode.Mouse1;
        keyBindings["Inventory"] = KeyCode.I;
        keyBindings["Skill I"] = KeyCode.Alpha1;
        keyBindings["Skill II"] = KeyCode.Alpha2;
        keyBindings["Skill III"] = KeyCode.Alpha3;
    }

    public void LoadAllKeys()
    {
        // keyBindings의 키 순회하며 PlayerPrefs에 저장된 값 있으면 덮어쓰기
        List<string> keys = new List<string>(keyBindings.Keys);
        foreach (var action in keys)
        {
            keyBindings[action] = LoadKey(action, keyBindings[action]);
        }
    }

    public KeyCode GetKeyForAction(string action)
    {
        return keyBindings.ContainsKey(action) ? keyBindings[action] : KeyCode.None;
    }

    public void StartRebind(string action, Action onComplete)
    {
        currentAction = action;
        waitingForKey = true;
        onRebindComplete = onComplete;
    }

    void OnGUI()
    {
        if (waitingForKey)
        {
            keyTextPrefab.gameObject.SetActive(true);
            Event e = Event.current;
            if (e.type == EventType.KeyDown) 
            {
                KeyCode code = e.keyCode;

                // 중복 체크
                string duplicateAction = null;
                foreach (var pair in keyBindings)
                {
                    if (pair.Value == code)
                    {
                        duplicateAction = pair.Key;
                        break;
                    }
                }

                if (duplicateAction != null && duplicateAction != currentAction)
                {
                    waringTextPrefab.text = $"Already using it for '{duplicateAction}'!";
                    waringTextPrefab.gameObject.SetActive(true);
                    keyTextPrefab.gameObject.SetActive(false);
                    waitingForKey = false;
                    StartCoroutine(HideWarningText());
                    e.Use();
                    return;
                }
        
                keyBindings[currentAction] = code;
                SaveKey(currentAction, code);
                keyTextPrefab.gameObject.SetActive(false);
                waringTextPrefab.gameObject.SetActive(false);
                waitingForKey = false;
                onRebindComplete?.Invoke();
                e.Use();
            }
        }
    }

    System.Collections.IEnumerator HideWarningText()
    {
        yield return new WaitForSeconds(1f);
        waringTextPrefab.gameObject.SetActive(false);
        keyTextPrefab.gameObject.SetActive(true);
        waitingForKey = true;
    }

    public void SaveKey(string action, KeyCode key)
    {
        PlayerPrefs.SetString(action, key.ToString());
    }

    public KeyCode LoadKey(string action, KeyCode defaultKey)
    {
        string savedKey = PlayerPrefs.GetString(action, defaultKey.ToString());
        return (KeyCode)System.Enum.Parse(typeof(KeyCode), savedKey);
    }

    public void ResetToDefault()
    {
        PlayerPrefs.DeleteKey("Interaction");
        PlayerPrefs.DeleteKey("Roll");
        PlayerPrefs.DeleteKey("Attack");
        PlayerPrefs.DeleteKey("Heavy Attack");
        PlayerPrefs.DeleteKey("Inventory");
        PlayerPrefs.DeleteKey("Skill I");
        PlayerPrefs.DeleteKey("Skill II");
        PlayerPrefs.DeleteKey("Skill III");

        SetDefaultBindings();

        KeyBindButton[] buttons = FindObjectsOfType<KeyBindButton>();
        foreach (var b in buttons) b.UpdateKeyText();
    }
}