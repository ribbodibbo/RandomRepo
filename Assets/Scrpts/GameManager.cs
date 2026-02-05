using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Preview Settings")]
    [SerializeField] private float previewDelay = 0.5f;
    [SerializeField] private float previewDuration = 5f;

    [Header("Game State")]
    [SerializeField] private int totalPairs;
    [SerializeField] private float mismatchDelay = 0.7f;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI MatchCount;
    [SerializeField] TextMeshProUGUI FlipCount;

    public UnityEvent GamWon;

    private int matchCount;
    private int flipCount;

    private Card firstCard;
    private Card secondCard;

    private int matchedPairs;
    private bool isChecking;
    private bool inputLocked;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }

    public void InitializeGame(int cardCount)
    {
        StopAllCoroutines(); // safety

        totalPairs = cardCount / 2;
        matchedPairs = 0;
        matchCount = 0;
        flipCount = 0;

        firstCard = null;
        secondCard = null;

        isChecking = false;
        inputLocked = true; // locked during preview

        // Reset UI
        if (MatchCount) MatchCount.text = "0";
        if (FlipCount) FlipCount.text = "0";

        StartCoroutine(PreviewCards());

       // Debug.Log($"[GameManager] Game Initialized. Total Pairs: {totalPairs}");
    }

    public void ResetGame()
    {
        StopAllCoroutines();

        firstCard = null;
        secondCard = null;

        matchCount = 0;
        flipCount = 0;
        matchedPairs = 0;

        isChecking = false;
        inputLocked = true; // locked until InitializeGame is called

        // Reset UI
        if (MatchCount) MatchCount.text = "0";
        if (FlipCount) FlipCount.text = "0";

        // Reset all cards visually
        Card[] cards = FindObjectsOfType<Card>();
        foreach (var card in cards)
        {
            card.ResetCard();
        }

    }

    private IEnumerator PreviewCards()
    {
        yield return new WaitForSeconds(previewDelay);

        Card[] cards = FindObjectsOfType<Card>();

        // Reveal all
        foreach (var card in cards)
        {
            card.Reveal();
        }

        yield return new WaitForSeconds(previewDuration);

        // Hide all
        foreach (var card in cards)
        {
            card.Hide();
        }

        inputLocked = false;
    }

    public void OnCardSelected(Card card)
    {
        if (inputLocked) return;
        if (isChecking) return;
        if (card.IsMatched) return;
        if (card.IsFaceUp) return;

        card.Reveal();
        AudioManager.Instance.PlayCardTap();

        if (firstCard == null)
        {
            firstCard = card;
            return;
        }

        secondCard = card;
        StartCoroutine(CheckMatch());
    }

    private IEnumerator CheckMatch()
    {
        isChecking = true;

        yield return new WaitForSeconds(0.1f); // small buffer for animation start

        if (firstCard.PairIndex == secondCard.PairIndex)
        {
            HandleMatch();
        }
        else
        {
            yield return new WaitForSeconds(mismatchDelay);
            HandleMismatch();
        }

        flipCount++;
        FlipCount.text = flipCount.ToString();

        firstCard = null;
        secondCard = null;
        isChecking = false;
    }

    private void HandleMatch()
    {
        AudioManager.Instance.PlayMatch();

        firstCard.SetMatched();
        secondCard.SetMatched();

        matchCount++;
        MatchCount.text = matchCount.ToString();    

        matchedPairs++;
       // Debug.Log($"[GameManager] Match found. ({matchedPairs}/{totalPairs})");

        if (matchedPairs >= totalPairs)
        {
            Invoke("OnGameWon", 2);
        }
    }

    private void HandleMismatch()
    {
        AudioManager.Instance.PlayMisMatch();

        firstCard.Hide();
        secondCard.Hide();

        //Debug.Log("[GameManager] Mismatch.");
    }

    private void OnGameWon()
    {
        AudioManager.Instance.PlayWin();
        GamWon.Invoke();
        //Debug.Log("🎉 [GameManager] YOU WIN!");
        // later:
        // score update
        // save progress
        // show win UI
    }
}
