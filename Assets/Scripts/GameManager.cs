using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> modernEraCardsList; // Modern �a� kartlar�n�n tutuldu�u liste
    [SerializeField]
    public List<GameObject> middleEraCardslist; // Orta �a� kartlar�n�n tutuldu�u liste
    [SerializeField]
    public Transform modernEraTransform; // Modern �a� kartlar�n�n olu�turuldu�u bo� objenin referans�
    [SerializeField]
    public Transform middleEraTransform; // Orta �a� kartlar�n�n olu�turuldu�u bo� objenin referans�
    [SerializeField]
    public TextMeshProUGUI cardText; // Kartlar�n UI'da g�r�nen konu�ma metinleri

    private bool isMiddleEraActive = false; // Orta �a��n aktif olup olmad���n� kontrol eder

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
        RandomAlign();
    }

    void Start()
    {
    }

    // Kartlar� rastgele s�ralar ve olu�turur
    public void RandomAlign()
    {
        // Modern �a� kartlar�n� rastgele s�rala ve olu�tur
        List<GameObject> randomModernCards = modernEraCardsList.OrderBy(x => Random.value).ToList();
        foreach (GameObject cards in randomModernCards)
        {
            GameObject card = Instantiate(cards, modernEraTransform);
            card.SetActive(false);
        }

        // Orta �a� kartlar�n� rastgele s�rala ve olu�tur
        List<GameObject> randomMiddleCards = middleEraCardslist.OrderBy(x => Random.value).ToList();
        foreach (GameObject cards in randomMiddleCards)
        {
            GameObject card = Instantiate(cards, middleEraTransform);
            card.SetActive(false);
        }
    }

    // Kart� se�me sonucu di�er kart� g�sterir
    public void ShowCard()
    {
        if (!isMiddleEraActive) // Modern �a� Kartlar�
        {
            if (modernEraTransform.childCount > 0)
            {
                Transform card = modernEraTransform.GetChild(0);
                card.gameObject.SetActive(true);

                bool isSelected = card.GetComponent<Card>().isSelected;

                if (isSelected) Destroy(card.gameObject);

                // Modern �a� kartlar� bitti�inde orta �a�a ge�
                if (modernEraTransform.childCount == 0)
                {
                    isMiddleEraActive = true;
                }
            }
        }
        else // Orta �a� Kartlar�
        {
            if (middleEraTransform.childCount > 0)
            {
                Transform card = middleEraTransform.GetChild(0);
                card.gameObject.SetActive(true);

                bool isSelected = card.GetComponent<Card>().isSelected;

                if (isSelected) Destroy(card.gameObject);

                // Orta �a� kartlar� bitti�inde oyun biter
                if (middleEraTransform.childCount == 0)
                {
                    print("Game Over");
                }
            }
        }
    }

    // Kartlar�n konu�ma metinlerini UI'da g�sterir
    public void ShowText()
    {
        if (!isMiddleEraActive && modernEraTransform.childCount > 0) // Modern �a� Kart Metni
        {
            string text = modernEraTransform.GetChild(0).GetComponent<Card>().text;
            cardText.text = $"{text}";
        }
        else if (isMiddleEraActive && middleEraTransform.childCount > 0) // Orta �a� Kart Metni
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
