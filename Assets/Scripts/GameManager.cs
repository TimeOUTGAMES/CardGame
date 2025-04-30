using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class GameManager : MonoBehaviour
{

    [SerializeField]
    public List<GameObject> modernEraCardsList, middleEraCardsList, ancientEraCardsList, modernEraAI, middleEraAI, ancientEraAI;
    [SerializeField]
    public Transform cardsTransform;
    [SerializeField]
    public TextMeshProUGUI characterString, characterName;

    public GameObject bgCard;


    public static GameManager instance;
    private void Awake()
    {
        instance = this;


    }

    public void CreateCards()
    {
        InstatiateAICards(modernEraAI);
        InstantiateCard(modernEraCardsList);
        InstatiateAICards(middleEraAI);
        InstantiateCard(middleEraCardsList);
        InstatiateAICards(ancientEraAI);
        InstantiateCard(ancientEraCardsList);


        bgCard.SetActive(true);

    }

    // public GameObject cards;

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

    //Kart� se�me sonucu di�er kart� g�sterir
    public void ShowCard()
    {
        if (cardsTransform.childCount > 0)
        {
            cardsTransform.GetChild(0).gameObject.SetActive(true);
        }
        else print("game over");
    }
    //Kartlar�n konu�ma metinlerini UI da g�sterir
    public void ShowText()
    {

        if (cardsTransform.childCount > 0)
        {
            if (cardsTransform.GetChild(0).GetComponent<CharacterCard>() != null)
            {
                characterString.text = $"{cardsTransform.GetChild(0).GetComponent<CharacterCard>().characterString}";
                characterName.text = $"{cardsTransform.GetChild(0).GetComponent<CharacterCard>().characterName}";
            }
            else if (cardsTransform.GetChild(0).GetComponent<AICard>() != null)
            {
                characterString.text = $"{cardsTransform.GetChild(0).GetComponent<AICard>().characterString}";
                characterName.text = $"{cardsTransform.GetChild(0).GetComponent<AICard>().characterName}";
            }
        }
        else
        {
            characterString.text = "";
            characterName.text = "";
        }

    }


    void Update()
    {

        ShowCard();
        ShowText();


    }
}
