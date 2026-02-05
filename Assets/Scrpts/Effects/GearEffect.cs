using UnityEngine;
using DG.Tweening;

public class GearEffect : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Transform settingsIcon;

    [Header("Rotation Settings")]
    [SerializeField] private float openRotation = -180f; // clockwise
    [SerializeField] private float rotateDuration = 0.4f;

    private bool isOpen;
    private Quaternion initialRotation;
    private Tween rotateTween;

    private void Awake()
    {
        initialRotation = settingsIcon.rotation;

        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    // 🔘 Call this from Button OnClick
    public void ToggleSettings()
    {
        if (isOpen)
            Close();
        else
            Open();
    }

    private void Open()
    {
        isOpen = true;

        rotateTween?.Kill();
        rotateTween = settingsIcon
            .DORotate(new Vector3(0, 0, openRotation), rotateDuration, RotateMode.LocalAxisAdd)
            .SetEase(Ease.Linear);

        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    private void Close()
    {
        isOpen = false;

        rotateTween?.Kill();
        rotateTween = settingsIcon
            .DORotateQuaternion(initialRotation, rotateDuration)
            .SetEase(Ease.Linear);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }
}
