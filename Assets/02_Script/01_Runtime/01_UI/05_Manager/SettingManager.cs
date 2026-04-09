using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.Rendering;

public enum SettingType
{
    Shadow,
    Brightness,
    Texture,
    FPS,
    MotionBlur,
    AntiAliasing,
    Screen,
}

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance { get; private set; }
    private Dictionary<SettingType, int> settings = new Dictionary<SettingType, int>();
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // 설정값 가져오기
    public int GetSetting(SettingType type)
    {
        if (settings.TryGetValue(type, out int value))
            return value;
        return 0; // 기본값
    }

    // 설정값 저장
    public void SetSetting(SettingType type, int value)
    {
        settings[type] = value;
    }
    
    // 세팅 전체 불러오기
    private void LoadSettings()
    {
        foreach (SettingType type in System.Enum.GetValues(typeof(SettingType)))
        {
            int value = PlayerPrefs.GetInt(type.ToString(), 0);
            settings[type] = value;
        }
    }
}