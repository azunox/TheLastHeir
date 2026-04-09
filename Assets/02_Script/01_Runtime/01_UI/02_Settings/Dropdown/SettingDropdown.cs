using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SettingDropdown : MonoBehaviour
{
    public Dropdown dropdown;
    public SettingType settingType;

    [Header("필요에 따라 설정")]
    public Light mainLight;
    public Volume volume;
    public Camera mainCamera;
    
    private MotionBlur motionBlur;

    void Start()
    {
        if (volume != null) {
            volume.profile.TryGet<MotionBlur>(out motionBlur); }
        
        dropdown.onValueChanged.AddListener(OnDropdownChanged);

        int savedIndex = SettingManager.Instance != null
            ? SettingManager.Instance.GetSetting(settingType)
            : 0;

        dropdown.value = savedIndex;
        OnDropdownChanged(savedIndex);
    }

    public void OnDropdownChanged(int index)
    {
        if (SettingManager.Instance != null)
            SettingManager.Instance.SetSetting(settingType, index);

        
        // 설정별 처리 분기
        switch (settingType)
        {
            case SettingType.Shadow:
                ApplyShadow(index);
                break;
            case SettingType.Brightness:
                ApplyBrightness(index);
                break;
            case SettingType.Texture:
                ApplyTextureQuality(index);
                break;
            case SettingType.FPS:
                ApplyFPS(index);
                break;
            case SettingType.MotionBlur:
                ApplyVolume(index);
                break;
            case SettingType.AntiAliasing:
                ApplyAntiAliasing(index);
                break;
            case SettingType.Screen:
                ApplyScreenMode(index);
                break;
        }
    }

    void ApplyShadow(int index)
    {
        float[] shadowStrengths = { 1.0f, 0.7f, 0.5f, 0.2f };
        bool[] shadowCast = { true, true, true, false };
        bool[] shadowReceive = { true, true, true, false };

       SetShadowForAllObjects(shadowCast[index], shadowReceive[index]);
        if (mainLight != null)
            mainLight.shadowStrength = shadowStrengths[index];
    }

    void ApplyBrightness(int index)
    {
        float[] brightnessLevels = { 1.0f, 0.8f, 0.6f, 0.4f };
        if (mainLight != null)
            mainLight.intensity = brightnessLevels[index];
    }

    void ApplyTextureQuality(int index)
    {
        int[] qualityLevels = { 0, 1, 2, 3 }; // Unity QualitySetting 예시
        QualitySettings.globalTextureMipmapLimit = qualityLevels[index];
    }

    void ApplyFPS(int index)
    {
        int[] fpxLevels = { -1, 144, 120, 90, 60, 30 };
        Application.targetFrameRate = fpxLevels[index];
    }
    
    void ApplyVolume(int index)
    {
        if (index == 0)
        {
            if (volume != null)
            {
                volume.profile.TryGet<MotionBlur>(out motionBlur);
            }
            motionBlur.active = true;
        }
        else if (index == 1)
        {
            motionBlur.active = false;
        }
    }

    void ApplyAntiAliasing(int index)
    {
        if (mainCamera == null) return;

        var cameraData = mainCamera.GetComponent<UniversalAdditionalCameraData>();
        
        if (cameraData == null) return;
        switch (index) {
            case 0: cameraData.antialiasing = AntialiasingMode.None; break;
            case 1: cameraData.antialiasing = AntialiasingMode.FastApproximateAntialiasing; break;
            case 2: cameraData.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing; break;
            case 3: cameraData.antialiasing = AntialiasingMode.TemporalAntiAliasing; break;
        }
    }
    public void ApplyScreenMode(int index)
    {
        switch (index)
        {
            case 0: // FullScreen
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1: // Window Screen
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 2: // Full Window Screen (Borderless)
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
        }
    }

    void SetShadowForAllObjects(bool cast, bool receive)
    {
        MeshRenderer[] renderers = FindObjectsOfType<MeshRenderer>();
        foreach (var renderer in renderers)
        {
            renderer.shadowCastingMode =
                cast ? UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = receive;
        }
    }
    
}