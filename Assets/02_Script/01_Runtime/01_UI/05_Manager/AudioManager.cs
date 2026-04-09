using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("UI 슬라이더")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider uiSlider;
    [SerializeField] private Slider voiceSlider;

    [Header("Audio Mixer 파라미터 이름")]
    private const string MASTER_PARAM = "Master";
    const string MUSIC_PARAM = "Music";
    private const string SFX_PARAM = "SFX";
    private const string UI_PARAM = "UI";
    private const string VOICE_PARAM = "Voicer";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // 슬라이더 이벤트 연결
        if (masterSlider != null) masterSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        if (musicSlider != null) musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        if (sfxSlider != null) sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        if (uiSlider != null) uiSlider.onValueChanged.AddListener(OnUIVolumeChanged);
        if (voiceSlider != null) voiceSlider.onValueChanged.AddListener(OnVoiceVolumeChanged);

        // 초기값 적용
        ApplyAll();
    }
    
    private float SliderToDecibel(float value)
    {
        return Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
    }

    private void ApplyAll()
    {
        if (masterSlider != null) OnMasterVolumeChanged(masterSlider.value);
        if (musicSlider != null) OnMusicVolumeChanged(musicSlider.value);
        if (sfxSlider != null) OnSFXVolumeChanged(sfxSlider.value);
        if (uiSlider != null) OnUIVolumeChanged(uiSlider.value);
        if (voiceSlider != null) OnVoiceVolumeChanged(voiceSlider.value);
    }

    public void OnMasterVolumeChanged(float value)
    {
        Debug.Log(value);
        audioMixer.SetFloat(MASTER_PARAM, SliderToDecibel(value));
    }
    public void OnMusicVolumeChanged(float value)
    {
        audioMixer.SetFloat(MUSIC_PARAM, SliderToDecibel(value));
    }
    public void OnSFXVolumeChanged(float value)
    {
        Debug.Log(value);
        audioMixer.SetFloat(SFX_PARAM, SliderToDecibel(value));
    }
    public void OnUIVolumeChanged(float value)
    {
        Debug.Log(value);
        audioMixer.SetFloat(UI_PARAM, SliderToDecibel(value));
    }
    public void OnVoiceVolumeChanged(float value)
    {
        Debug.Log(value);
        audioMixer.SetFloat(VOICE_PARAM, SliderToDecibel(value));
    }
}
