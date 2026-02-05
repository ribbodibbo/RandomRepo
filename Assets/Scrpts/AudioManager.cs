using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SfxVolume";
    private const string MUSIC_MUTED_KEY = "MusicMuted";

    [Header("Audio Sources (Drag from Hierarchy)")]
    [SerializeField] private AudioSource bgMusic;
    [SerializeField] private AudioSource cardTap;
    [SerializeField] private AudioSource match;
    [SerializeField] private AudioSource misMatch;
    [SerializeField] private AudioSource win;

    [Header("Settings")]
    [Range(0f, 1f)][SerializeField] private float musicVolume = 0.6f;
    [Range(0f, 1f)][SerializeField] private float sfxVolume = 1.0f;

    private bool isMusicMuted;

    // -------------------- Unity --------------------

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadSettings();
        ApplyVolumes();

        if (bgMusic != null && !isMusicMuted)
            bgMusic.Play();
    }

    // -------------------- Public API --------------------

    public void PlayCardTap() => PlaySfx(cardTap);
    public void PlayMatch() => PlaySfx(match);
    public void PlayMisMatch() => PlaySfx(misMatch);
    public void PlayWin() => PlaySfx(win);

    public void PlayMusic()
    {
        if (bgMusic == null || isMusicMuted) return;
        if (!bgMusic.isPlaying) bgMusic.Play();
    }

    public void StopMusic()
    {
        if (bgMusic == null) return;
        bgMusic.Stop();
    }

    public void SetMusicVolume(float value01)
    {
        musicVolume = Mathf.Clamp01(value01);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, musicVolume);
        PlayerPrefs.Save();
        ApplyVolumes();
    }

    public void SetSfxVolume(float value01)
    {
        sfxVolume = Mathf.Clamp01(value01);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, sfxVolume);
        PlayerPrefs.Save();
        ApplyVolumes();
    }

    public void ToggleMusicMute()
    {
        isMusicMuted = !isMusicMuted;
        PlayerPrefs.SetInt(MUSIC_MUTED_KEY, isMusicMuted ? 1 : 0);
        PlayerPrefs.Save();

        if (isMusicMuted)
            StopMusic();
        else
            PlayMusic();
    }

    // -------------------- Internals --------------------

    private void PlaySfx(AudioSource src)
    {
        if (src == null) return;

        src.volume = sfxVolume;
        src.Play();
    }

    private void ApplyVolumes()
    {
        if (bgMusic != null)
            bgMusic.volume = isMusicMuted ? 0f : musicVolume;

        if (cardTap != null) cardTap.volume = sfxVolume;
        if (match != null) match.volume = sfxVolume;
        if (misMatch != null) misMatch.volume = sfxVolume;
        if (win != null) win.volume = sfxVolume;
    }

    private void LoadSettings()
    {
        musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.6f);
        sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1.0f);
        isMusicMuted = PlayerPrefs.GetInt(MUSIC_MUTED_KEY, 0) == 1;
    }
}
