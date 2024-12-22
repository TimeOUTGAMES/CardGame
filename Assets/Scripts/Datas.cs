using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class Datas : MonoBehaviour
{
    public List<GameObject> modernEraCards;
    public Transform modernEra;

    private void Start()
    {
        RandomAlign();
        
    }


    private void Update()
    {
        
    }

    public void RandomAlign()
    {
        
        List<GameObject> randomCards = modernEraCards.OrderBy(x => Random.value).ToList();
        foreach (GameObject cards in randomCards)
        {
            GameObject card = Instantiate(cards, modernEra);
            card.SetActive(false);
        }
        randomCards.First().SetActive(true);
        
    }


    public void ShowCards()
    {
        int index = 1;
        if (modernEraCards.Count >= index)
        {
            modernEra.GetChild(index - 1).gameObject.SetActive(false);
            modernEra.GetChild(index).gameObject.SetActive(true);
        }
        
        index++;

    }



}



//[System.Serializable]
//public class ModernEra
//{
//    public GameObject card;
//    public string text;
//    public string rightChoose;
//    public string leftChoose;
//}
