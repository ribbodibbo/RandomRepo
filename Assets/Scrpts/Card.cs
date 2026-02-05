using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler
{
    [Header("Visuals")]
    [SerializeField] private Image mainSprite;
    [SerializeField] private GameObject frontFace;
    [SerializeField] private GameObject backFace;

    [Header("Flip Settings")]
    [SerializeField] private float flipDuration = 0.25f;

    public int PairIndex { get; private set; }
    public bool IsFaceUp { get; private set; }
    public bool IsMatched { get; private set; }

    private bool isAnimating;

    // -------------------- Setup --------------------

    public void Setup(int pairIndex, Sprite sprite)
    {
        PairIndex = pairIndex;
        mainSprite.sprite = sprite;
        IsFaceUp = false;
        IsMatched = false;

        frontFace.SetActive(false);
        backFace.SetActive(true);
    }

    // -------------------- Flip Logic --------------------

    public void Reveal()
    {
        //if (IsFaceUp || isAnimating) return;
        Flip(true);
    }

    public void Hide()
    {
        // if (!IsFaceUp || isAnimating) return;
        Flip(false);
    }

    //----------------------- Effects ----------------------

    public void SetMatched()
    {
        if (IsMatched) return;

        IsMatched = true;
        isAnimating = true;

        // block input instantly
        var cg = GetComponent<CanvasGroup>();
        if (cg == null) cg = gameObject.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = false;
        cg.interactable = false;

        GetComponent<PopAndShrinkEffect>().Play(transform);

        Debug.Log($"[Card] Matched -> PopAndShrink: {name}");
    }
    private void Flip(bool showFront)
    {
        if (isAnimating) return;
        isAnimating = true;

        Debug.Log($"[Card] Flip start -> showFront={showFront}");

        transform.DOKill();

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DORotate(new Vector3(0, 90, 0), flipDuration / 2f).SetEase(Ease.InQuad));

        seq.AppendCallback(() =>
        {
            frontFace.SetActive(showFront);
            backFace.SetActive(!showFront);

            Debug.Log($"[Card] Mid flip -> Front={frontFace.activeSelf}, Back={backFace.activeSelf}");
        });

        seq.Append(transform.DORotate(Vector3.zero, flipDuration / 2f).SetEase(Ease.OutQuad));

        seq.OnComplete(() =>
        {
            IsFaceUp = showFront;
            isAnimating = false;

            Debug.Log($"[Card] Flip done -> IsFaceUp={IsFaceUp}");
        });
    }


    // -------------------- Input -------------------

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsMatched || isAnimating) return;
        GameManager.Instance.OnCardSelected(this);
    }

 

}
