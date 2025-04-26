using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{






    [SerializeField]
    public List<GameObject> modernEraCardsList, middleEraCardsList, ancientEraCardsList;//Kartlar�n listeleri
    [SerializeField]
    public Transform cardsTransform;//Modern �a� kartlar�n�n olu�turuldu�u bo� objenin referans�
    [SerializeField]
    public TextMeshProUGUI characterString, characterName;//Kartlar�n UI da g�r�nen konu�ma metinleri


    public static GameManager instance;
    private void Awake()
    {
        instance = this;


    }
    void Start()
    {

    }



    //Kartlar� rastgele s�ralar ve olu�turur
    public void CreateCards()
    {
        InstantiateCard(modernEraCardsList);
        InstantiateCard(middleEraCardsList);
        InstantiateCard(ancientEraCardsList);

    }


    void InstantiateCard(List<GameObject> cardList)
    {
       List<GameObject> allCards = cardList.OrderBy(x => Random.value).ToList();
       foreach (GameObject cards in allCards)
        {
            GameObject card = Instantiate(cards, cardsTransform);
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
            characterString.text = $"{cardsTransform.GetChild(0).GetComponent<Card>().characterString}";
            characterName.text = $"{cardsTransform.GetChild(0).GetComponent<Card>().characterName}";
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
