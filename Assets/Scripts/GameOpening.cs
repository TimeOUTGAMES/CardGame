using System.Collections;
using Unity.VisualScripting;
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
    [SerializeField] private Vector3 thirdCardPosition;

    private int currentCardIndex = 0;
    private bool isInitialized = false;
    private const float ARRIVAL_THRESHOLD = 0.1f;

    public static GameOpening instance;

    private void Awake()
    {
        InstantiateCards();
        isInitialized = true;
        instance = this;
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
    

    public void en()
    {
        this.enabled = true;
    }



    private void FinishOpeningSequence()
    {
        /*foreach(Transform card in transform)
        {
            card.SetParent(GameManager.instance.transform, true);
            GameManager.instance.RegisterTransitionCard(card.gameObject);
        }*/
        GameManager.instance.CreateCards();
        this.enabled = false;
        //Destroy(gameObject);

    }
}