using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cardsParent; // GridHolder transform

    [Header("Sprites Pool (pair sprites)")]
    [SerializeField] private List<Sprite> pairSprites;
    // Must have at least number of pairs needed (Easy=3, Hard=8)

    [Header("Debug")]
    [SerializeField] private bool logAssignments = true;

    public void InitializePairs(int totalCards)
    {
        if (totalCards <= 0)
        {
            Debug.LogWarning("[CardManager] InitializePairs(): totalCards <= 0");
            return;
        }

        if (totalCards % 2 != 0)
        {
            Debug.LogError("[CardManager] InitializePairs(): totalCards must be EVEN for pairs. (6/16 ok, 9 not ok)");
            return;
        }

        int pairsCount = totalCards / 2;

        if (pairSprites == null || pairSprites.Count < pairsCount)
        {
            Debug.LogError($"[CardManager] Not enough sprites. Need {pairsCount}, have {(pairSprites == null ? 0 : pairSprites.Count)}");
            return;
        }

        // 1) Get spawned cards
        List<Card> cards = new List<Card>(cardsParent.GetComponentsInChildren<Card>());
        if (cards.Count != totalCards)
        {
            Debug.LogWarning($"[CardManager] Expected {totalCards} cards, found {cards.Count}. Using found count.");
            totalCards = cards.Count;
            if (totalCards % 2 != 0)
            {
                Debug.LogError("[CardManager] Found odd number of cards, cannot assign pairs.");
                return;
            }
            pairsCount = totalCards / 2;
        }

        // 2) Build pair id list: 1,1,2,2,3,3...
        List<int> pairIds = new List<int>(totalCards);
        for (int id = 1; id <= pairsCount; id++)
        {
            pairIds.Add(id);
            pairIds.Add(id);
        }

        // 3) Shuffle pair ids (so pairs are random positions)
        Shuffle(pairIds);

        // 4) Assign to cards
        for (int i = 0; i < totalCards; i++)
        {
            int pairId = pairIds[i];
            Sprite spriteForPair = pairSprites[pairId - 1]; // pairId starts at 1

            cards[i].Setup(pairId, spriteForPair);

            if (logAssignments)
                Debug.Log($"[CardManager] Assigned Card[{i}] => pairId={pairId}, sprite={spriteForPair.name}");
        }

        Debug.Log($"[CardManager] InitializePairs(): Done. pairs={pairsCount}, cards={totalCards}");
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
