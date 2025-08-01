using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class EndOfEraCardData
{
    public string cardName;
    public Image cardImage;
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> modernEraCardsList, middleEraCardsList, ancientEraCardsList,
    modernEraAIStart, middleEraAIStart, ancientEraAIStart,
    modernEraAIEnd, middleEraAIEnd, ancientEraAIEnd;
    [SerializeField]
    public Transform cardsTransform;

    public WhiteFlashEffect whiteFlashEffect;
    public int currentEra = 1;
    public GameObject bgCard;

    private bool isTransitioning = false;
    public bool IsTransitioning => isTransitioning;

    public Vector3 secondCardPosition;
    public Vector3 thirdCardPosition;
    private bool allCardsInstantiate = false;


    public Color ancientColor;
    public Color middleColor;
    public Color modenrColor;
    
    public Age currentAge = Age.MODERN_ERA;
    public List<Image> barsToChange;
    public EndOfEraCardData militaryEnd, farmEnd, healthEnd, publicEnd;
    
    private UIManager uiManager;

    public enum Age
    {
        ANCIENT_ERA,
        MIDDLE_ERA,
        MODERN_ERA,
    }

    public static GameManager instance;
    private void Awake()
    {
        instance = this;

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
                //if (cardsTransform.GetChild(0).GetComponent<CharacterCard>() != null &&
                //cardsTransform.GetChild(0).GetComponent<CharacterCard>().currentEra != currentEra)
                //{
                //    newEra = cardsTransform.GetChild(0).GetComponent<CharacterCard>().currentEra;                    

                //}
                if (cardsTransform.GetChild(0).GetComponent<AICard>() != null &&
                cardsTransform.GetChild(0).GetComponent<AICard>().currentEra != currentEra)
                {
                    newEra = cardsTransform.GetChild(0).GetComponent<AICard>().currentEra;
                    StartCoroutine(GameOpening.Instance.DealCards());
                    whiteFlashEffect.Play();
                }
            }

            if (newEra != -1)
            {
                currentEra = newEra;
                switch (currentEra)
                {
                    case 1:
                        currentAge = Age.MODERN_ERA;
                        break;
                    case 2:
                        currentAge = Age.MIDDLE_ERA;
                        break;
                    case 3:
                        currentAge = Age.ANCIENT_ERA;
                        break;
                    default:
                        print("gay");
                        break;

                        
                }
                ChangeAgeColor(currentAge);
                Debug.Log(currentEra);

            }
        }
    }

    private void ChangeAgeColor(Age age)
    {
        switch (age)
        {
            case Age.ANCIENT_ERA:
                foreach (Image img in barsToChange)
                {
                    img.color = ancientColor;
                }
                break;
            case Age.MIDDLE_ERA:
                foreach (Image img in barsToChange)
                {
                    img.color = middleColor;
                }
                break;
            case Age.MODERN_ERA:
                foreach (Image img in barsToChange)
                {
                    img.color = modenrColor;
                }
                break;
        }
    }


    public void CreateCards()
    {
        if (allCardsInstantiate) return;
        allCardsInstantiate = true;
        Instantiate(cardsTransform);
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


        if (cardsTransform.childCount > 0)
        {
            Transform firstCard = cardsTransform.GetChild(0);
            if (!firstCard.gameObject.activeSelf)
            {
                firstCard.gameObject.SetActive(true);
                uiManager.ShowText();
                
            }
        }
    }

    public void EndOfEraCardManager()
    {
        switch (BarControl.Instance.CheckBarValue())
        {
            case "Military":
                militaryEnd.cardImage.gameObject.SetActive(true);                
                break;
            case "Farm":
                farmEnd.cardImage.gameObject.SetActive(true);                
                break;
            case "Public":
                publicEnd.cardImage.gameObject.SetActive(true);                
                break;
            case "Economy":
                healthEnd.cardImage.gameObject.SetActive(true);                
                break;
            default:
                Debug.LogError("Geçersiz bar adı: " + BarControl.Instance.CheckBarValue());
                break;

        }
    }

    public void RestartGame()
    {
        if (EndOfEraCard.Instance.isSelected)
        {
            Destroy(cardsTransform);
            allCardsInstantiate = false;
            whiteFlashEffect.Play();
            bgCard.SetActive(false);
            currentEra = 1;
            BarControl.Instance.InitAllvars();
            CreateCards();


        }
    }


    void Update()
    {
        ShowCard();
        ManageEra();
        EndOfEraCardManager();
    }

    private void Start()
    {
        uiManager = UIManager.instance;
    }
}
