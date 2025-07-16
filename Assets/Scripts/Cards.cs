using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public abstract class Cards : MonoBehaviour
{
    [Header("Card Settings")]
    [SerializeField] protected LayerMask cardLayermask;
    [SerializeField] protected float maxDistance = 1.5f;
    [SerializeField] protected int cardSpeed = 10;
    
    [SerializeField] protected Vector3 cardEnd = new Vector3(1000,0,0);
    [SerializeField] protected float cardEndDuration = 0.5f;

    [SerializeField] public int currentEra;
    
    [Header("Card Content")]
    [SerializeField] public string characterString;
    [SerializeField] public string characterName;

    // Protected variables
    protected Vector3 originalPosition;
    protected float startPosX;
    protected bool isTouching;
    public bool isSelected;
    protected float distance;
    protected Vector3 firstTouchPos;
    protected bool hasPlayedHoldSound = false;
    protected BarControl barControl;
    protected SpriteRenderer image;
    protected float fadeDuration = 0.15f;

    // Touch sensitivity constants
    private const float ROTATION_FACTOR = 10f;

    protected virtual void Start()
    {
        image = GetComponent<SpriteRenderer>();
        originalPosition = transform.position;
        startPosX = originalPosition.x;
        PlayWhiteFlash();
       
    }

    protected void PlayWhiteFlash()
    {
        if (image == null) return;

        Color originalColor = image.color;
        originalColor.a = 0f;
        image.color = originalColor;
        image.DOFade(1f, fadeDuration).SetEase(Ease.InQuad);
            
             
             
    }


    protected virtual void Update()
    {
        TouchControl();
        
        // Only manage card when not being touched
        if (!isTouching)
        {
            ManageCard();
        }
    }  

    protected virtual void ManageCard()
    {
        distance = Mathf.Abs(originalPosition.x - transform.position.x);

        if (distance >= maxDistance)
        {
            // Card has been swiped far enough
            //AudioManager.instance.Play("CardSelected");
            isSelected = true;
            
            float direction = transform.position.x - startPosX;
            if (direction > 0)
            {
                transform.DOMove(cardEnd, cardEndDuration).SetEase(Ease.OutQuad).OnComplete(() => Destroy(gameObject));
            }
            else if (direction < 0)
            {
                transform.DOMove(-cardEnd, cardEndDuration).SetEase(Ease.OutQuad).OnComplete(() => Destroy(gameObject));
            }
           
            
        }
        else
        {
            // Return card to original position
            ReturnToOriginalPosition();
        }
    }

    protected virtual void ReturnToOriginalPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, Time.deltaTime * cardSpeed);
        transform.eulerAngles = Vector3.zero;
    }

    protected virtual void TouchControl()
    {
        if (Input.touchCount <= 0) return;
        
        Touch touch = Input.GetTouch(0);
        Vector3 touchPosition = GetWorldTouchPosition(touch.position);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                HandleTouchBegan(touchPosition);
                break;

            case TouchPhase.Moved:
                HandleTouchMoved(touchPosition);
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                HandleTouchEnded();
                break;
        }
    }

    protected virtual Vector3 GetWorldTouchPosition(Vector2 screenPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(
            screenPosition.x, 
            screenPosition.y, 
            Camera.main.nearClipPlane
        ));
        worldPosition.z = 0; // Ensure Z is 0 for 2D
        return worldPosition;
    }

    protected virtual void HandleTouchBegan(Vector3 touchPosition)
    {
        isTouching = CheckCard(touchPosition);
        firstTouchPos = touchPosition;
        hasPlayedHoldSound = false;
    }

    protected virtual void HandleTouchMoved(Vector3 touchPosition)
    {
        if (!isTouching) return;
        
        Vector3 moveAmount = touchPosition - firstTouchPos;
        MoveCard(moveAmount);
        firstTouchPos = touchPosition;
    }

    protected virtual void HandleTouchEnded()
    {
        isTouching = false;
        hasPlayedHoldSound = false;
        
        if (barControl != null)
        {
            barControl.ResetBarColors();
        }
    }

    protected virtual bool CheckCard(Vector3 touchPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, Mathf.Infinity, cardLayermask);
        return hit.collider != null;
    }

    protected virtual void MoveCard(Vector3 moveAmount)
    {
        if (!isTouching) return;
        
        transform.position += moveAmount;
        RotateCard();

        if (!hasPlayedHoldSound)
        {
            //AudioManager.instance.Play("HoldCard");
            hasPlayedHoldSound = true;
        }
    }

    protected virtual void RotateCard()
    {
        float rotationAngle = (startPosX - transform.position.x) * ROTATION_FACTOR;
        transform.eulerAngles = new Vector3(0, 0, rotationAngle);
    }
}