using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> modernEraCardsList; // Modern Çað kartlarýnýn tutulduðu liste
    [SerializeField]
    public List<GameObject> middleEraCardslist; // Orta Çað kartlarýnýn tutulduðu liste
    [SerializeField]
    public Transform modernEraTransform; // Modern Çað kartlarýnýn oluþturulduðu boþ objenin referansý
    [SerializeField]
    public Transform middleEraTransform; // Orta Çað kartlarýnýn oluþturulduðu boþ objenin referansý
    [SerializeField]
    public TextMeshProUGUI cardText; // Kartlarýn UI'da görünen konuþma metinleri

    private bool isMiddleEraActive = false; // Orta çaðýn aktif olup olmadýðýný kontrol eder

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
        RandomAlign();
    }

    void Start()
    {
    }

    // Kartlarý rastgele sýralar ve oluþturur
    public void RandomAlign()
    {
        // Modern Çað kartlarýný rastgele sýrala ve oluþtur
        List<GameObject> randomModernCards = modernEraCardsList.OrderBy(x => Random.value).ToList();
        foreach (GameObject cards in randomModernCards)
        {
            GameObject card = Instantiate(cards, modernEraTransform);
            card.SetActive(false);
        }

        // Orta Çað kartlarýný rastgele sýrala ve oluþtur
        List<GameObject> randomMiddleCards = middleEraCardslist.OrderBy(x => Random.value).ToList();
        foreach (GameObject cards in randomMiddleCards)
        {
            GameObject card = Instantiate(cards, middleEraTransform);
            card.SetActive(false);
        }
    }

    // Kartý seçme sonucu diðer kartý gösterir
    public void ShowCard()
    {
        if (!isMiddleEraActive) // Modern Çað Kartlarý
        {
            if (modernEraTransform.childCount > 0)
            {
                Transform card = modernEraTransform.GetChild(0);
                card.gameObject.SetActive(true);

                bool isSelected = card.GetComponent<Card>().isSelected;

                if (isSelected) Destroy(card.gameObject);

                // Modern Çað kartlarý bittiðinde orta çaða geç
                if (modernEraTransform.childCount == 0)
                {
                    isMiddleEraActive = true;
                }
            }
        }
        else // Orta Çað Kartlarý
        {
            if (middleEraTransform.childCount > 0)
            {
                Transform card = middleEraTransform.GetChild(0);
                card.gameObject.SetActive(true);

                bool isSelected = card.GetComponent<Card>().isSelected;

                if (isSelected) Destroy(card.gameObject);

                // Orta Çað kartlarý bittiðinde oyun biter
                if (middleEraTransform.childCount == 0)
                {
                    print("Game Over");
                }
            }
        }
    }

    // Kartlarýn konuþma metinlerini UI'da gösterir
    public void ShowText()
    {
        if (!isMiddleEraActive && modernEraTransform.childCount > 0) // Modern Çað Kart Metni
        {
            string text = modernEraTransform.GetChild(0).GetComponent<Card>().text;
            cardText.text = $"{text}";
        }
        else if (isMiddleEraActive && middleEraTransform.childCount > 0) // Orta Çað Kart Metni
        {
            string text = middleEraTransform.GetChild(0).GetComponent<Card>().text;
            cardText.text = $"{text}";
        }
        else
        {
            cardText.text = "";
        }
    }

    void Update()
    {
        ShowCard();
        ShowText();
    }
}
