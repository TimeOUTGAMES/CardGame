using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class GameManager : MonoBehaviour
{

    [SerializeField]
    public List<GameObject> modernEraCardsList, middleEraCardsList, ancientEraCardsList, 
    modernEraAIStart, middleEraAIStart, ancientEraAIStart,
    modernEraAIEnd,middleEraAIEnd, ancientEraAIEnd;
    [SerializeField]
    public Transform cardsTransform;

    public WhiteFlashEffect whiteFlashEffect;
    public int currentEra = 1;
    public GameObject bgCard;

    private bool isTransitioning = false;
    public bool IsTransitioning => isTransitioning;

    public Vector3 secondCardPosition; 
    public Vector3 thirdCardPosition;

    private List<GameObject> transitionCards = new List<GameObject>();

    public static GameManager instance;
    private void Awake()
    {
        instance = this;
        print("deneme");

        if (whiteFlashEffect == null)
        {
            Debug.LogError("WhiteFlashEffect bağlantısı yapılmamış! GameManager içinde null.");
        }
    }

    public void ManageEra()
    {
        if (isTransitioning) return;

        if (cardsTransform.childCount > 0)
        {
            int newEra = -1;
            if (cardsTransform.childCount > 0)
            {
                if (cardsTransform.GetChild(0).GetComponent<CharacterCard>() != null &&
                cardsTransform.GetChild(0).GetComponent<CharacterCard>().currentEra != currentEra)
                {
                    newEra = cardsTransform.GetChild(0).GetComponent<CharacterCard>().currentEra;
                    print("çağ değişti");

                }
                else if (cardsTransform.GetChild(0).GetComponent<AICard>() != null &&
                cardsTransform.GetChild(0).GetComponent<AICard>().currentEra != currentEra)
                {
                    newEra = cardsTransform.GetChild(0).GetComponent<AICard>().currentEra;
                    print("çağ değişti");

                }
            }

            if (newEra != -1)
            {
                currentEra = newEra;
                Debug.Log("Çağ değişti → Yeni çağ: " + currentEra);
                whiteFlashEffect.Play();
                GameOpening.instance.en();
                //StartCoroutine(MoveCardDown(transitionCards, secondCardPosition, thirdCardPosition, 30f));
            }
        }
    }

    /*public void RegisterTransitionCard(GameObject card)
    {
        card.SetActive(false);
        transitionCards.Add(card);
    }

    public IEnumerator MoveCardDown(List<GameObject> cards, Vector3 secondCardPosition, Vector3 thirdCardPosition, float speed)
    {
        isTransitioning = true;
        bgCard.SetActive(false);

        // Oyun kartlarını geçici olarak gizle
        SetGameCardsVisible(false);


        foreach (GameObject card in cards)
        {
            card.SetActive(true);
            card.transform.position = secondCardPosition;

            while (Vector3.Distance(card.transform.position, thirdCardPosition) > 0.05f)
            {
                card.transform.position = Vector3.MoveTowards(card.transform.position, thirdCardPosition, speed * Time.deltaTime);
                yield return null;
            }

            AudioManager.instance?.Play("HoldCard");
            card.SetActive(false);
        }


        // Oyun kartlarını tekrar görünür yap
        SetGameCardsVisible(true);
        bgCard.SetActive(true);
        isTransitioning = false;
    }



    private void SetGameCardsVisible(bool visible)
    {
        foreach (Transform child in cardsTransform)
        {
            child.gameObject.SetActive(false); // Hepsini kapat
        }

        if (visible && cardsTransform.childCount > 0)
        {
            cardsTransform.GetChild(0).gameObject.SetActive(true); // Sadece ilk kartı aç
        }
    }*/


    public void CreateCards()
    {

        InstatiateAICards(modernEraAIStart);
        InstantiateCard(modernEraCardsList);
        InstatiateAICards(modernEraAIEnd);
        InstatiateAICards(middleEraAIStart);
        InstantiateCard(middleEraCardsList);
        InstatiateAICards(middleEraAIEnd);
        InstatiateAICards(ancientEraAIStart);
        InstantiateCard(ancientEraCardsList);
        InstatiateAICards(ancientEraAIEnd);
  


        bgCard.SetActive(true);

    }

    void InstatiateAICards(List<GameObject> cardList)
    {
        foreach (GameObject cardPrefab in cardList)
        {
            GameObject card = Instantiate(cardPrefab, cardsTransform);
            card.SetActive(false);
        }

    }



    void InstantiateCard(List<GameObject> cardList)
    {
        List<GameObject> allCards = cardList.OrderBy(x => Random.value).ToList();
        foreach (GameObject cardPrefab in allCards)
        {
            GameObject card = Instantiate(cardPrefab, cardsTransform);
            card.SetActive(false);
        }


    }


    public void ShowCard()
    {
        //if (isTransitioning) return;

        if (cardsTransform.childCount > 0)
        {
            Transform firstCard = cardsTransform.GetChild(0);
            if (!firstCard.gameObject.activeSelf) // Eğer zaten görünüyorsa tekrar açma
            {
                firstCard.gameObject.SetActive(true);
            }
        }
        else
        {
            print("game over");
        }
    }


    void Update()
    {
        //if (isTransitioning) return;
        ShowCard();
        ManageEra();

    }

}
