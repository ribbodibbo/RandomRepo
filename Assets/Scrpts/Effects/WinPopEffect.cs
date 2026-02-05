using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class WinPopEffect : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup canvasGroup; 

    [Header("Pop Settings")]
    [SerializeField] private float startScale = 0f;
    [SerializeField] private float popScale = 1.15f;
    [SerializeField] private float finalScale = 1f;

    [SerializeField] private float popDuration = 0.35f;
    [SerializeField] private float settleDuration = 0.15f;

    private void Awake()
    {
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

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.DOFade(1f, popDuration * 0.8f);
        }


        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOScale(popScale, popDuration).SetEase(Ease.InSine));
        seq.Append(transform.DOScale(finalScale, settleDuration).SetEase(Ease.OutSine));

        AudioManager.Instance?.PlayWin();
    }
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
