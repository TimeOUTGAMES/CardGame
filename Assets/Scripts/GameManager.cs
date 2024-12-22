using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{






    [SerializeField]
    public List<GameObject> modernEraCardsList;//Modern Çað kartlarýnýn tutulduðu liste
    [SerializeField]
    public Transform modernEraTransform;//Modern Çað kartlarýnýn oluþturulduðu boþ objenin referansý
    [SerializeField]
    public TextMeshProUGUI cardText;//Kartlarýn UI da görünen konuþma metinleri


    public static GameManager instance;
    private void Awake()
    {
        instance = this;
        RandomAlign();

    }
    void Start()
    {

    }


    //Kartlarý rastgele sýralar ve oluþturur
    public void RandomAlign()
    {

        List<GameObject> randomCards = modernEraCardsList.OrderBy(x => Random.value).ToList();
        foreach (GameObject cards in randomCards)
        {
            GameObject card = Instantiate(cards, modernEraTransform);
            card.SetActive(false);
        }

    }

    //Kartý seçme sonucu diðer kartý gösterir
    public void ShowCard()
    {
        if (modernEraTransform.childCount > 0)
        {
            modernEraTransform.GetChild(0).gameObject.SetActive(true);
            bool isSelected = modernEraTransform.GetChild(0).GetComponent<Card>().isSelected;

            if (isSelected) Destroy(modernEraTransform.GetChild(0).gameObject);
        }
        else print("game over");



    }
    //Kartlarýn konuþma metinlerini UI da gösterir
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
