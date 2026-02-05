using UnityEngine;
using DG.Tweening;

public class PopAndShrinkEffect : MonoBehaviour
{

    [Header("Scale Values")]
    [SerializeField] private float initialShrinkScale = 0.85f;
    [SerializeField] private float popScale = 1.15f;

    [Header("Timings")]
    [SerializeField] private float initialShrinkDuration = 0.08f;
    [SerializeField] private float popDuration = 0.10f;
    [SerializeField] private float finalShrinkDuration = 0.18f;



 
    public void Play(Transform target, bool disableOnComplete = true)
    {
        if (target == null)
        {
            Debug.LogWarning("[PopAndShrinkEffect] Play called with null target.");
            return;
        }

        // Ensure clean state
        target.DOKill();
        target.localScale = Vector3.one;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(
            target.DOScale(initialShrinkScale, initialShrinkDuration)
                  .SetEase(Ease.InQuad)
        );

        sequence.Append(
            target.DOScale(popScale, popDuration)
                  .SetEase(Ease.OutBack)
        );

        sequence.Append(
            target.DOScale(0f, finalShrinkDuration)
                  .SetEase(Ease.InBack)
        );

        sequence.OnComplete(() =>
        {
            if (disableOnComplete)
                target.gameObject.SetActive(false);
        });
    }
}
