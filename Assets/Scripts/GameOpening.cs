using System.Collections;
using UnityEngine;

public class GameOpening : MonoBehaviour
{
    public GameObject card;
    public int cardCount;

    public Vector3 firstCardPosition;
    public Vector3 secondCardPosition;
    public float cardSpeed;
    public float delay;
    int i =0;

    void Awake()
    {
        InstantiateCards();
    }


    void Update()
    {
        MoveCards();
    }

    public void InstantiateCards()
    {
        for (int i = 0; i < cardCount; i++)
        {
            Instantiate(card, firstCardPosition, Quaternion.identity, transform);
        }
    }

    public void MoveCards()
    {
        

        if (i < transform.childCount)
        {
            GameObject currentCard = transform.GetChild(i).gameObject;
            currentCard.transform.position = Vector3.MoveTowards(currentCard.transform.position, secondCardPosition, cardSpeed * Time.deltaTime);
            if (Vector3.Distance(currentCard.transform.position, secondCardPosition) < 0.1f)
            {                
                i++;
            }
            
        }
        
    }

    
}
