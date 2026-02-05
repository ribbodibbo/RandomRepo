using UnityEngine;

public class MobileRuntimeSettings : MonoBehaviour
{
    [Header("FPS")]
    [Tooltip("Target FPS for the game. Use 60 for stable mobile feel. Use 120 only if you can maintain it.")]
    [SerializeField] private int targetFps = 60;

    [Header("Screen Awake")]
    [Tooltip("Prevents the phone from dimming / sleeping while the game is running.")]
    [SerializeField] private bool keepScreenAwake = true;

    [Header("Optional")]
    [SerializeField] private bool dontDestroyOnLoad = true;
    [SerializeField] private bool lockOrientationPortrait = false;
    [SerializeField] private bool lockOrientationLandscape = false;

    private void Awake()
    {
        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
            Debug.Log("[MobileRuntimeSettings] DontDestroyOnLoad enabled. (ADDED)");
        }

        ApplySettings();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        // Re-apply settings when app returns to foreground
        if (hasFocus)
            ApplySettings();
    }

    private void OnApplicationPause(bool isPaused)
    {
        // Re-apply after pause/resume (some devices reset sleep settings)
        if (!isPaused)
            ApplySettings();
    }

    private void ApplySettings()
    {
        // --- FPS ---
        QualitySettings.vSyncCount = 0; // Important: VSync overrides targetFrameRate on many platforms
        Application.targetFrameRate = targetFps;

        Debug.Log($"[MobileRuntimeSettings] Target FPS set to {targetFps}. VSync disabled. (MODIFIED)");

        // --- Screen Awake ---
        if (keepScreenAwake)
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Debug.Log("[MobileRuntimeSettings] Screen sleep disabled (NeverSleep). (ADDED)");
        }
        else
        {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
            Debug.Log("[MobileRuntimeSettings] Screen sleep uses SystemSetting. (ADDED)");
        }

        // --- Orientation (optional) ---
        if (lockOrientationPortrait)
        {
            Screen.orientation = ScreenOrientation.Portrait;
            Debug.Log("[MobileRuntimeSettings] Orientation locked to Portrait. (ADDED)");
        }
        else if (lockOrientationLandscape)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft; // or LandscapeRight
            Debug.Log("[MobileRuntimeSettings] Orientation locked to Landscape. (ADDED)");
        }
    }

    // Call this from gameplay if you want to temporarily allow screen sleep in menus:
    public void SetKeepScreenAwake(bool awake)
    {
        keepScreenAwake = awake;
        ApplySettings();
    }

    public void SetTargetFps(int fps)
    {
        targetFps = Mathf.Clamp(fps, 30, 240);
        ApplySettings();
    }
}
