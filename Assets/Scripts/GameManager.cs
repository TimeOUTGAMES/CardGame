using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{






    [SerializeField]
    public List<GameObject> modernEraCardsList;//Modern �a� kartlar�n�n tutuldu�u liste
    [SerializeField]
    public Transform modernEraTransform;//Modern �a� kartlar�n�n olu�turuldu�u bo� objenin referans�
    [SerializeField]
    public TextMeshProUGUI cardText;//Kartlar�n UI da g�r�nen konu�ma metinleri


    public static GameManager instance;
    private void Awake()
    {
        instance = this;
        RandomAlign();

    }
    void Start()
    {
       
    }

  

    //Kartlar� rastgele s�ralar ve olu�turur
    public void RandomAlign()
    {

        List<GameObject> randomCards = modernEraCardsList.OrderBy(x => Random.value).ToList();
        foreach (GameObject cards in randomCards)
        {
            GameObject card = Instantiate(cards, modernEraTransform);
            card.SetActive(false);
        }

    }

    //Kart� se�me sonucu di�er kart� g�sterir
    public void ShowCard()
    {
        if (modernEraTransform.childCount > 0)
        {
            modernEraTransform.GetChild(0).gameObject.SetActive(true);
            // bool isSelected = modernEraTransform.GetChild(0).GetComponent<Card>().isSelected;

            // if (isSelected) Destroy(modernEraTransform.GetChild(0).gameObject);
        }
        else print("game over");



    }
    //Kartlar�n konu�ma metinlerini UI da g�sterir
    public void ShowText()
    {

        if (modernEraTransform.childCount > 0)
        {
            string text = modernEraTransform.GetChild(0).GetComponent<Card>().text;
            cardText.text = $"{text}";
        }
        else
            cardText.text = "";
    }


    void Update()
    {

        ShowCard();
        ShowText();


    }
}
