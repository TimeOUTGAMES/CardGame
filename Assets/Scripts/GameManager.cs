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

    public int currentEra = 1;


    public GameObject bgCard;


    public static GameManager instance;
    private void Awake()
    {
        instance = this;
        print("deneme");
    }


    public void ManageEra()
    {
        if (cardsTransform.childCount > 0)
        {
            if (cardsTransform.GetChild(1).GetComponent<CharacterCard>() != null && 
            cardsTransform.GetChild(1).GetComponent<CharacterCard>().currentEra!=currentEra)
            {
                currentEra = cardsTransform.GetChild(1).GetComponent<CharacterCard>().currentEra;
                print("çağ değişti");
            }
            else if (cardsTransform.GetChild(1).GetComponent<AICard>() != null && 
            cardsTransform.GetChild(1).GetComponent<AICard>().currentEra!=currentEra)
            {
                currentEra = cardsTransform.GetChild(1).GetComponent<AICard>().currentEra;
                print("çağ değişti");
            }
        }
    }

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
        if (cardsTransform.childCount > 0)
        {
            cardsTransform.GetChild(0).gameObject.SetActive(true);
        }
        else print("game over");
    }




    void Update()
    {

        ShowCard();
        ManageEra();


    }
}
