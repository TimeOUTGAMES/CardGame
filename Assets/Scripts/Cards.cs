using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Cards : MonoBehaviour
{
    [Header("Card Settings")]
    [SerializeField] protected LayerMask cardLayermask;
    [SerializeField] protected float maxDistance = 1.5f;
    [SerializeField] protected int cardSpeed = 10;

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
    public Image image;
    public float fadeDuration = 0.5f;

    // Touch sensitivity constants
    private const float ROTATION_FACTOR = 10f;

    protected virtual void Start()
    {
        originalPosition = transform.position;
        startPosX = originalPosition.x;
        if (image != null)
        {
            StartCoroutine(FadeAlpha(image, fadeDuration));
        }
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
    protected IEnumerator FadeAlpha(Image img, float duration)
    {
        Color color = img.color;
        float startAlpha = 250f / 255f; // 125 değeri 0-1 aralığına çevrildi
        float endAlpha = 0f;
        float elapsed = 0f;

        // Başlangıç alfa değeri ayarla
        color.a = startAlpha;
        img.color = color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            color.a = newAlpha;
            img.color = color;
            yield return null;
        }

        // Bitiş alfa değeri kesin olarak ayarla
        color.a = endAlpha;
        img.color = color;
    }

    protected virtual void ManageCard()
    {
        distance = Mathf.Abs(originalPosition.x - transform.position.x);

        if (distance >= maxDistance)
        {
            // Card has been swiped far enough
            //AudioManager.instance.Play("CardSelected");
            isSelected = true;            
            Destroy(gameObject);
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
            AudioManager.instance.Play("HoldCard");
            hasPlayedHoldSound = true;
        }
    }

    protected virtual void RotateCard()
    {
        float rotationAngle = (startPosX - transform.position.x) * ROTATION_FACTOR;
        transform.eulerAngles = new Vector3(0, 0, rotationAngle);
    }
}