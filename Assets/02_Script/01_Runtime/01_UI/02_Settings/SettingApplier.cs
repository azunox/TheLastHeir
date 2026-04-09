using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class SettingApplier : MonoBehaviour
{
    [Header("필요에 따라 설정")]
    public Light mainLight; // Inspector에서 씬의 Directional Light 연결
    public Volume volume;
    
    private MotionBlur motionBlur;

    public Camera mainCamera;

    void Start()
    {
        if (volume != null)
        {
            volume.profile.TryGet<MotionBlur>(out motionBlur);
        }
        ApplyAllSettings();
    }

    public void ApplyAllSettings()
    {
        int shadowIndex = SettingManager.Instance.GetSetting(SettingType.Shadow);
        ApplyShadow(shadowIndex);

        int brightnessIndex = SettingManager.Instance.GetSetting(SettingType.Brightness);
        ApplyBrightness(brightnessIndex);
        
        int textureQualityIndex = SettingManager.Instance.GetSetting(SettingType.Texture);
        ApplyTextureQuality(textureQualityIndex);
        
        int fpsIndex = SettingManager.Instance.GetSetting(SettingType.FPS);
        ApplyFPS(fpsIndex);

        int motionBlurIndex = SettingManager.Instance.GetSetting(SettingType.MotionBlur);
        ApplyMotionBlur(motionBlurIndex);
        
        int antiAliasingIndex = SettingManager.Instance.GetSetting(SettingType.AntiAliasing);
        ApplyAntiAliasing(antiAliasingIndex);
        
        int screenModeIndex = SettingManager.Instance.GetSetting(SettingType.Screen);
        SetScreenMode(screenModeIndex);
    }

    void ApplyShadow(int index)
    {
        float[] shadowStrengths = { 1.0f, 0.7f, 0.5f, 0.2f };
        bool[] shadowCast = { true, true, true, false };
        bool[] shadowReceive = { true, true, true, false };

        MeshRenderer[] renderers = FindObjectsOfType<MeshRenderer>();
        foreach (var renderer in renderers)
        {
            renderer.shadowCastingMode = shadowCast[index] ? ShadowCastingMode.On : ShadowCastingMode.Off;
            renderer.receiveShadows = shadowReceive[index];
        }
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

    void ApplyMotionBlur(int index)
    {
        if (index == 0)
        {
            motionBlur.active = true;
        }
        else if (index == 1)
        {
            motionBlur.active = false;
        }
    }

    void ApplyAntiAliasing(int index)
    {
        if (mainCamera != null) return;
        
        var cameraData = mainCamera.GetComponent<UniversalAdditionalCameraData>();
        
        if (cameraData == null) return;
        switch (index) {
            case 0: cameraData.antialiasing = AntialiasingMode.None; break;
            case 1: cameraData.antialiasing = AntialiasingMode.FastApproximateAntialiasing; break;
            case 2: cameraData.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing; break;
            case 3: cameraData.antialiasing = AntialiasingMode.TemporalAntiAliasing; break;
        }
    }
    
    void SetScreenMode(int index)
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
}