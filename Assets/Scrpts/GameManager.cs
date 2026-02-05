using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Preview Settings")]
    [SerializeField] private float previewDelay = 0.5f;
    [SerializeField] private float previewDuration = 5f;

    [Header("Game State")]
    [SerializeField] private int totalPairs;
    [SerializeField] private float mismatchDelay = 0.7f;

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
        totalPairs = cardCount / 2;
        matchedPairs = 0;

        firstCard = null;
        secondCard = null;
        isChecking = false;

        StartCoroutine(PreviewCards());

        Debug.Log($"[GameManager] Game Initialized. Total Pairs: {totalPairs}");
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

        Debug.Log("[GameManager] Preview finished. Player can play.");
    }

    public void OnCardSelected(Card card)
    {
        if (inputLocked) return;
        if (isChecking) return;
        if (card.IsMatched) return;
        if (card.IsFaceUp) return;

        card.Reveal();

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

        firstCard = null;
        secondCard = null;
        isChecking = false;
    }

    private void HandleMatch()
    {
        firstCard.SetMatched();
        secondCard.SetMatched();

        matchedPairs++;
        Debug.Log($"[GameManager] Match found. ({matchedPairs}/{totalPairs})");

        if (matchedPairs >= totalPairs)
        {
            OnGameWon();
        }
    }

    private void HandleMismatch()
    {
        firstCard.Hide();
        secondCard.Hide();

        Debug.Log("[GameManager] Mismatch.");
    }

    private void OnGameWon()
    {
        Debug.Log("🎉 [GameManager] YOU WIN!");
        // later:
        // score update
        // save progress
        // show win UI
    }
}
