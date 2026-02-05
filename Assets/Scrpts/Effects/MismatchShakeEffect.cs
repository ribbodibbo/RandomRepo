using UnityEngine;
using DG.Tweening;


public class MismatchShakeEffect : MonoBehaviour
{

    [Header("Shake Settings")]
    [SerializeField] private float duration = 0.25f;
    [SerializeField] private float strength = 18f;     // pixels for UI cards
    [SerializeField] private int vibrato = 18;
    [SerializeField] private float randomness = 90f;

    public void Play(Transform target)
    {
        if (target == null)
        {
            Debug.LogWarning("[MismatchShakeEffect] Play called with null target.");
            return;
        }

        // For UI, shake position gives a "vibrate" feel
        target.DOShakePosition(
            duration: duration,
            strength: new Vector3(strength, 0f, 0f),
            vibrato: vibrato,
            randomness: randomness,
            snapping: false,
            fadeOut: true
        );
    }
}
