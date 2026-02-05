using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    private const string PREF_DIFFICULTY = "selected_difficulty"; // 0=Easy,1=Normal,2=Hard

    [Header("Buttons")]
    public Button Easy;
    public Button Medium;
    public Button Hard;
    public Button StartButton;

    [Header("UI")]
    public RectTransform SelectionBox;

    [Header("Refs")]
    public GridManager GridManager;

    private void OnEnable()
    {
        Easy.onClick.AddListener(EasyLevel);
        Medium.onClick.AddListener(MediumLevel);
        Hard.onClick.AddListener(HardLevel);
        StartButton.onClick.AddListener(StartGame);

        LoadAndApplySavedDifficulty();
    }

    private void OnDisable()
    {
        Easy.onClick.RemoveListener(EasyLevel);
        Medium.onClick.RemoveListener(MediumLevel);
        Hard.onClick.RemoveListener(HardLevel);
        StartButton.onClick.RemoveListener(StartGame);
    }

    // -------------------- Levels --------------------

    public void EasyLevel()
    {
        ApplyDifficulty(DifficultyMode.Easy);
        SaveDifficulty(DifficultyMode.Easy);
    }

    public void MediumLevel()
    {
        ApplyDifficulty(DifficultyMode.Normal);
        SaveDifficulty(DifficultyMode.Normal);
    }

    public void HardLevel()
    {
        ApplyDifficulty(DifficultyMode.Hard);
        SaveDifficulty(DifficultyMode.Hard);
    }

    private void ApplyDifficulty(DifficultyMode mode)
    {
        if (GridManager != null)
            GridManager.mode = mode;

        // Move selection box to the correct button
        RectTransform target = GetButtonRect(mode);
        MoveSelectionBox(target);

    }

    // -------------------- Selection Box --------------------

    private void MoveSelectionBox(RectTransform target)
    {
        if (SelectionBox == null || target == null) return;
        SelectionBox.localPosition = target.localPosition;
    }

    private RectTransform GetButtonRect(DifficultyMode mode)
    {
        switch (mode)
        {
            case DifficultyMode.Easy: return Easy.GetComponent<RectTransform>();
            case DifficultyMode.Normal: return Medium.GetComponent<RectTransform>();
            case DifficultyMode.Hard: return Hard.GetComponent<RectTransform>();
            default: return Easy.GetComponent<RectTransform>();
        }
    }

    // -------------------- PlayerPrefs --------------------

    private void SaveDifficulty(DifficultyMode mode)
    {
        PlayerPrefs.SetInt(PREF_DIFFICULTY, (int)mode);
        PlayerPrefs.Save();
        Debug.Log($"[LevelSelector] Saved difficulty: {mode}");
    }

    private void LoadAndApplySavedDifficulty()
    {
        DifficultyMode saved = (DifficultyMode)PlayerPrefs.GetInt(PREF_DIFFICULTY, (int)DifficultyMode.Easy);
        ApplyDifficulty(saved);
    }

    // -------------------- Start --------------------

    public void StartGame()
    {
        if (GridManager == null) return;

        GridManager.Initialize(GridManager.mode);
    }
}
