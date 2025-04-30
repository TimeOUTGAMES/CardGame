using System.Collections;
using UnityEngine;

public class GameOpening : MonoBehaviour
{
    [Header("Card Settings")]
    [SerializeField] private GameObject card;
    [SerializeField] private int cardCount = 5;
    [SerializeField] private float cardSpeed = 5f;
    [SerializeField] private float delay = 0.2f;

    [Header("Position Settings")]
    [SerializeField] private Vector3 firstCardPosition;
    [SerializeField] private Vector3 secondCardPosition;

    private int currentCardIndex = 0;
    private bool isInitialized = false;
    private const float ARRIVAL_THRESHOLD = 0.1f;

    private void Awake()
    {
        InstantiateCards();
        isInitialized = true;
    }

    private void Update()
    {
        if (isInitialized)
        {
            MoveCards();
        }
    }

    private void InstantiateCards()
    {
        if (card == null)
        {
            Debug.LogError("Card prefab is not assigned!");
            return;
        }

        for (int i = 0; i < cardCount; i++)
        {
            Instantiate(card, firstCardPosition, Quaternion.identity, transform);
        }
    }

    private void MoveCards()
    {
        // Check if we're done with all cards
        if (currentCardIndex >= transform.childCount)
        {
            FinishOpeningSequence();
            return;
        }

        // Get current card and move it
        Transform currentCard = transform.GetChild(currentCardIndex);
        if (currentCard != null)
        {
            currentCard.position = Vector3.MoveTowards(
                currentCard.position,
                secondCardPosition,
                cardSpeed * Time.deltaTime
            );

            // Check if card reached destination
            if (Vector3.Distance(currentCard.position, secondCardPosition) < ARRIVAL_THRESHOLD)
            {
                AudioManager.instance.Play("HoldCard");
                currentCardIndex++;
            }
        }
    }

    private void FinishOpeningSequence()
    {

        GameManager.instance.CreateCards();        
        Destroy(gameObject);
    }
}