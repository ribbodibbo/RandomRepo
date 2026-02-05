using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("UI Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private const string MUSIC_KEY = "MusicVolume";
    private const string SFX_KEY = "SfxVolume";

    private void Start()
    {
        LoadSettings();
        BindUI();
    }

    // -------------------- Init --------------------

    private void LoadSettings()
    {
        float music = PlayerPrefs.GetFloat(MUSIC_KEY, 0.6f);
        float sfx = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        if (musicSlider) musicSlider.value = music;
        if (sfxSlider) sfxSlider.value = sfx;

        AudioManager.Instance.SetMusicVolume(music);
        AudioManager.Instance.SetSfxVolume(sfx);
    }

    private void BindUI()
    {
        if (musicSlider)
            musicSlider.onValueChanged.AddListener(OnMusicChanged);

        if (sfxSlider)
            sfxSlider.onValueChanged.AddListener(OnSfxChanged);
    }

    // -------------------- Callbacks --------------------

    private void OnMusicChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
        PlayerPrefs.SetFloat(MUSIC_KEY, value);
    }

    private void OnSfxChanged(float value)
    {
        AudioManager.Instance.SetSfxVolume(value);
        PlayerPrefs.SetFloat(SFX_KEY, value);
    }

    private void OnDisable()
    {
        if (musicSlider)
            musicSlider.onValueChanged.RemoveListener(OnMusicChanged);

        if (sfxSlider)
            sfxSlider.onValueChanged.RemoveListener(OnSfxChanged);

        PlayerPrefs.Save();
    }
}
