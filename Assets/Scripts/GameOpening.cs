using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameOpening : MonoBehaviour
{
    [Header("Card Setup")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private int cardNumbers = 5;
    [SerializeField] private float spawnDelay = 0.2f;
    [SerializeField] private Vector3 startOffset = new Vector3(-1000, 0, 0);
    [SerializeField] private Vector3 endOffset = new Vector3(1000, 0, 0);
    [SerializeField] private Transform targetPosition;

    [Header("Fade Setup")]
    [SerializeField] private GameObject fadePrefab;

    [Header("Audio")]
    [SerializeField] private AudioClip cardSound;
    [SerializeField] private AudioSource audioSource;

    public static GameOpening Instance;

    private List<GameObject> cardPool = new List<GameObject>();

    private void Awake()
    {
        Instance = this;

        // Object Pool oluştur
        for (int i = 0; i < cardNumbers; i++)
        {
            GameObject card = Instantiate(cardPrefab, transform);
            card.SetActive(false);
            cardPool.Add(card);
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        StartCoroutine(DealCards());
    }

    public IEnumerator DealCards()
    {
        Vector3 offsetValue = endOffset;

        for (int i = 0; i < cardNumbers; i++)
        {
            GameObject card = cardPool[i];
            ResetCard(card);

            card.SetActive(true);
            card.transform.position = targetPosition.position + startOffset;

            // 💡 Kartın öne gelmesi için sortingOrder'ı ayarla
            SpriteRenderer renderer = card.GetComponent<SpriteRenderer>();
            if (renderer != null)
                renderer.sortingOrder = i;

            // Kart hareket animasyonu
            card.transform.DOMove(targetPosition.position + offsetValue, 0.4f)
                .SetEase(Ease.OutCubic)
                .SetAutoKill(true);

            offsetValue += endOffset;

            // Kart sesi
            if (audioSource != null && cardSound != null)
                audioSource.PlayOneShot(cardSound);

            yield return new WaitForSeconds(spawnDelay);
        }

        // Fade animasyonu
        yield return new WaitForSeconds(spawnDelay * 5);

        GameObject fade = Instantiate(fadePrefab, transform);
        fade.transform.position = targetPosition.position + offsetValue;

        SpriteRenderer fadeRenderer = fade.GetComponent<SpriteRenderer>();
        if (fadeRenderer != null)
        {
            fadeRenderer.color = new Color(1f, 1f, 1f, 0f);
            fadeRenderer.sortingOrder = cardNumbers + 1;

            fadeRenderer.DOFade(1, 0.5f).SetEase(Ease.InQuad).OnComplete(() =>
            {
                GameManager.instance.CreateCards();
                GameManager.instance.cardsTransform.gameObject.SetActive(true);
                GameManager.instance.bgCard.SetActive(true);

                fadeRenderer.DOFade(0f, 0.5f).SetEase(Ease.OutBack);
            });
        }
    }

    private void ResetCard(GameObject card)
    {
        card.transform.DOKill(); // Tween'leri temizle
        card.transform.localScale = Vector3.one * 2;
        card.transform.rotation = Quaternion.identity;
        card.transform.position = Vector3.zero;
        card.SetActive(false);
    }
}
