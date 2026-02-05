using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class WinPopEffect : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup canvasGroup; // optional but recommended

    [Header("Pop Settings")]
    [SerializeField] private float startScale = 0f;
    [SerializeField] private float popScale = 1.15f;
    [SerializeField] private float finalScale = 1f;

    [SerializeField] private float popDuration = 0.35f;
    [SerializeField] private float settleDuration = 0.15f;

    private void Awake()
    {
        // Prepare hidden state
        transform.localScale = Vector3.one * startScale;

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

    }
    private void OnEnable()
    {
        Show();
    }

    [ContextMenu("POP")]
    public void Show()
    {
        gameObject.SetActive(true);

        transform.DOKill();
        canvasGroup?.DOKill();

        // Fade in
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.DOFade(1f, popDuration * 0.8f);
        }

        // Pop animation
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOScale(popScale, popDuration).SetEase(Ease.InSine));
        seq.Append(transform.DOScale(finalScale, settleDuration).SetEase(Ease.OutSine));

        // Optional: play win sound
        AudioManager.Instance?.PlayWin();
    }

    // Optional hide (for replay)
    public void Hide()
    {
        transform.DOKill();
        canvasGroup?.DOKill();

        gameObject.SetActive(false);
        transform.localScale = Vector3.one * startScale;

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }
}
