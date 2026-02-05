using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [Header("UI")]
    public Image spriteRenderer;   // drag the Front Image here in prefab

    [Header("Data")]
    public int pairIndex;          // 1..N (same for two cards)

    public void Setup(int newPairIndex, Sprite newSprite)
    {
        pairIndex = newPairIndex;

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = newSprite;
            spriteRenderer.preserveAspect = true;
        }

        Debug.Log($"[Card] Setup(): pairIndex={pairIndex}, sprite={(newSprite ? newSprite.name : "null")}");
    }
}
