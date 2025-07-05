using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
public class GameOpening : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject fadePrefab;
    [SerializeField] private GameObject firstCard;
    [SerializeField] AudioClip cardSound;
    public Transform targetPosition;
    public float spawnDelay = 0.2f;
    public Vector3 startOffset = new Vector3(-1000, 0, 0);
    public Vector3 endOffset = new Vector3(1000, 0, 0);
    public int cardNumbers = 5;
    public static GameOpening Instance;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        StartCoroutine(DealCards());
    }

    public IEnumerator DealCards()
    {
        Vector3 offsetValue = endOffset;
        SpriteRenderer spriteRenderer = null;
        for (int i = 0; i < cardNumbers; i++)
        {
            // Kart oluştur
            GameObject card = Instantiate(cardPrefab, transform);
            card.transform.position = targetPosition.position + startOffset;
            AudioSource.PlayClipAtPoint(cardSound,Vector3.zero);
            //card.transform.localScale = Vector3.zero;

            // DOTween ile animasyon: Ölçekle büyüsün ve pozisyona gelsin
            card.transform.DOMove(targetPosition.position + offsetValue, 0.4f).SetEase(Ease.OutCubic);
            offsetValue += endOffset;
            
            // Sıradaki karta geçmeden önce bekle
            yield return new WaitForSeconds(spawnDelay);
        }
        yield return new WaitForSeconds(spawnDelay * 5);
        GameObject fade = Instantiate(fadePrefab, transform);
        fade.transform.position = targetPosition.position + offsetValue;
        spriteRenderer = fade.GetComponent<SpriteRenderer>();
        Color fadedColor = new Color(1f, 1f, 1f, 1f); // Beyaz ton
// veya daha soluk yapmak istersen
// Color fadedColor = new Color(0.85f, 0.85f, 0.85f, 1f);
        spriteRenderer.DOFade(1, 0.5f).SetEase(Ease.InQuad)
                    .OnComplete(() =>
                    {
                        GameManager.instance.CreateCards();
                        GameManager.instance.cardsTransform.gameObject.SetActive(true);
                        GameManager.instance.bgCard.SetActive(true);
                        spriteRenderer.DOFade(0f, 0.5f).SetEase(Ease.InQuad);
                            
                    });
        
    }
}