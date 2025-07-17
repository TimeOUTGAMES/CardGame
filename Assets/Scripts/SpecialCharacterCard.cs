using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecialCharacterCard : Cards
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI rightText;
    [SerializeField] private TextMeshProUGUI leftText;
    [SerializeField] [Range(0, 2)] private float textFadeDistance = 0.5f;
    [SerializeField] private GameObject fadePanelGO;

    private Image fadeImage;
    private RectTransform fadeRect;

    [Header("Left Swipe Effects")]
    [SerializeField] private float leftMilitaryChange;
    [SerializeField] private float leftFarmChange;
    [SerializeField] private float leftPublicChange;
    [SerializeField] private float leftEconomyChange;

    [Header("Right Swipe Effects")]
    [SerializeField] private float rightMilitaryChange;
    [SerializeField] private float rightFarmChange;
    [SerializeField] private float rightPublicChange;
    [SerializeField] private float rightEconomyChange;

    [Header("Swipe-Based New Cards")]
    [SerializeField] private GameObject cardPrefabRight;
    [SerializeField] private GameObject cardPrefabLeft;
    private Transform cardsTransform;

    [SerializeField] private AudioClip cardAppearClip;
    private AudioSource audioSource;

    private const float DIRECTION_THRESHOLD = 0.1f;

    private bool isSpawned;

    private AudioManager audioManager;

    private void Awake()
    {

        audioSource = GetComponent<AudioSource>();

        if (fadePanelGO != null)
        {
            fadeImage = fadePanelGO.GetComponent<Image>();
            fadeRect = fadePanelGO.GetComponent<RectTransform>();
        }

        if (barControl == null)
        {
            barControl = FindFirstObjectByType<BarControl>();
            if (barControl == null)
            {
                Debug.LogWarning("BarControl reference not found. Bar effects won't work.");
            }
        }
    }

    private void OnEnable()
    {
        if (audioSource != null && cardAppearClip != null)
        {
            cardsTransform = GameManager.instance.cardsTransform;
            audioManager = AudioManager.instance;
            audioSource.PlayOneShot(cardAppearClip);
        }
    }

    protected override void MoveCard(Vector3 moveAmount)
    {
        leftText.gameObject.SetActive(true);
        rightText.gameObject.SetActive(true);

        base.MoveCard(moveAmount);

        if (!isTouching || barControl == null) return;

        float direction = transform.position.x - startPosX;
        float fadeDirection = Mathf.Clamp(direction, -textFadeDistance, textFadeDistance);

        float leftAlpha = Mathf.InverseLerp(0f, textFadeDistance, fadeDirection);
        float rightAlpha = Mathf.InverseLerp(0f, -textFadeDistance, fadeDirection);

        SetAlpha(leftText, leftAlpha);
        SetAlpha(rightText, rightAlpha);

        float fadeAmount = Mathf.InverseLerp(0f, textFadeDistance, Mathf.Abs(fadeDirection));
        SetAlpha(fadeImage, fadeAmount);

        if (fadeRect != null)
        {
            Vector3 originalScale = fadeRect.localScale;
            fadeRect.localScale = new Vector3(
                direction > 0 ? Mathf.Abs(originalScale.x) : -Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );
        }

        if (Mathf.Abs(direction) > DIRECTION_THRESHOLD)
        {
            if (direction > 0)
            {
                barControl.PreviewBarEffects(
                    rightEconomyChange,
                    rightFarmChange,
                    rightPublicChange,
                    rightMilitaryChange
                );
            }
            else
            {
                barControl.PreviewBarEffects(
                    leftEconomyChange,
                    leftFarmChange,
                    leftPublicChange,
                    leftMilitaryChange
                );
            }
        }
    }

    protected override void ReturnToOriginalPosition()
    {
        base.ReturnToOriginalPosition();
        SetTextVisibility(false, false);
        SetAlpha(leftText, 0);
        SetAlpha(rightText, 0);
        if (fadeImage != null) SetAlpha(fadeImage, 0);
        if (barControl != null)
        {
            barControl.ResetBarColors();
        }
    }

    protected override void ManageCard()
    {
        base.ManageCard();

        if (distance >= maxDistance)
        {
            ApplyBarEffect();

            bool isRightSwipe = transform.position.x > startPosX;

            if (!isSpawned)
            {
                SpawnNewCardBasedOnSwipe(isRightSwipe);
                isSpawned = true;
            }
        }
    }

    private void ApplyBarEffect()
    {
        if (barControl == null) return;

        float direction = transform.position.x - startPosX;

        if (direction > 0)
        {
            barControl.ApplyEffects(
                rightEconomyChange,
                rightFarmChange,
                rightPublicChange,
                rightMilitaryChange
            );
        }
        else if (direction < 0)
        {
            barControl.ApplyEffects(
                leftEconomyChange,
                leftFarmChange,
                leftPublicChange,
                leftMilitaryChange
            );
        }
    }

    private void SpawnNewCardBasedOnSwipe(bool rightSwipe)
    {
        GameObject prefabToSpawn = rightSwipe ? cardPrefabLeft : cardPrefabRight;
        if (prefabToSpawn == null || cardsTransform == null) return;

        GameObject newCard = Instantiate(prefabToSpawn, cardsTransform);
        
        int currentIndex = transform.GetSiblingIndex();
        newCard.transform.SetSiblingIndex(currentIndex + 1);
        newCard.SetActive(false);
    }

    private void SetAlpha(Graphic graphic, float alpha)
    {
        if (graphic == null) return;
        Color c = graphic.color;
        c.a = alpha;
        graphic.color = c;
    }

    private void SetTextVisibility(bool showRightText, bool showLeftText)
    {
        // Gerekiyorsa burada text aktifliklerini kontrol edebilirsin
    }
}
