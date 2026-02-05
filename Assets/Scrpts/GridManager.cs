using UnityEngine;
using UnityEngine.UI;

public enum DifficultyMode
{
    Easy,   // 6 cards  (3x2)
    Normal, // 12 cards (4x3)
    Hard    // 16 cards (4x4)
}

[RequireComponent(typeof(GridLayoutGroup))]
public class GridManager : MonoBehaviour
{
    [Header("Mode")]
    public DifficultyMode mode;

    [Header("References")]
    [SerializeField] private RectTransform container;
    [SerializeField] private GridLayoutGroup grid;
    [SerializeField] private CardManager cardManager;

    [Header("Spawn")]
    [SerializeField] private GameObject cardPrefab;

    public DifficultyMode CurrentMode { get; private set; }
    public int CurrentCardCount { get; private set; }

    private void Reset()
    {
        grid = GetComponent<GridLayoutGroup>();
        container = GetComponent<RectTransform>();
        Debug.Log("[GridManager] Reset: Auto-assigned grid + container.");
    }

    private void Awake()
    {
        if (!grid) grid = GetComponent<GridLayoutGroup>();
        if (!container) container = GetComponent<RectTransform>();
    }

    [ContextMenu("Initialize")]
    public void InitializeInInspector()
    {
        ClearCardsEditor();
        Initialize(mode);
    }

    public void Initialize(DifficultyMode selectedMode)
    {
        CurrentMode = selectedMode;

        switch (selectedMode)
        {
            case DifficultyMode.Easy:
                ApplyPresetEasy();
                break;

            case DifficultyMode.Normal:
                ApplyPresetNormal();
                break;

            case DifficultyMode.Hard:
                ApplyPresetHard();
                break;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(container);

        SpawnCards(CurrentCardCount);

        Debug.Log($"[GridManager] Initialize: Mode={CurrentMode}, Cards={CurrentCardCount}");
    }

    private void SpawnCards(int count)
    {
        if (!cardPrefab)
        {
            Debug.LogError("[GridManager] SpawnCards: cardPrefab is not assigned.");
            return;
        }

        ClearCardsRuntime();

        for (int i = 0; i < count; i++)
        {
            var cardGO = Instantiate(cardPrefab, container);
            cardGO.name = $"Card_{i:00}";
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(container);

        if (cardManager != null)
            cardManager.InitializePairs(count);
        else
            Debug.LogWarning("[GridManager] SpawnCards: cardManager is not assigned.");
    }

    // -------------------- Presets --------------------

    private void ApplyPresetEasy()
    {
        CurrentCardCount = 6;

        container.sizeDelta = new Vector2(1050, 700);

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 3;

        grid.cellSize = new Vector2(300, 300);
        grid.spacing = new Vector2(35, 20);
        grid.padding = new RectOffset(40, 40, 40, 50);

        Debug.Log("[GridManager] Preset: Easy (3x2)");
    }

    private void ApplyPresetNormal()
    {
        CurrentCardCount = 12;

        container.sizeDelta = new Vector2(1150, 850);

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 4;

        grid.cellSize = new Vector2(250, 250);
        grid.spacing = new Vector2(20, 10);
        grid.padding = new RectOffset(40, 40, 40, 40);

        Debug.Log("[GridManager] Preset: Normal (4x3)");
    }

    private void ApplyPresetHard()
    {
        CurrentCardCount = 16;

        container.sizeDelta = new Vector2(1050, 1000);

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 4;

        grid.cellSize = new Vector2(200, 200);
        grid.spacing = new Vector2(50, 30);
        grid.padding = new RectOffset(50, 40, 50, 50);

        Debug.Log("[GridManager] Preset: Hard (4x4)");
    }

    // -------------------- Clearing --------------------

    private void ClearCardsRuntime()
    {
        if (!container) return;

        for (int i = container.childCount - 1; i >= 0; i--)
            Destroy(container.GetChild(i).gameObject);

        Debug.Log("[GridManager] ClearCardsRuntime: Cleared children.");
    }

    private void ClearCardsEditor()
    {
        if (!container) return;

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            for (int i = container.childCount - 1; i >= 0; i--)
                DestroyImmediate(container.GetChild(i).gameObject);

            Debug.Log("[GridManager] ClearCardsEditor: Cleared children (Editor).");
            return;
        }
#endif

        // fallback if in play mode
        ClearCardsRuntime();
    }
}
