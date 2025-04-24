using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    public LayerMask cardLayermask;//Kartın layerı
    private Vector3 originalPosition;//Kartın ilk konumu
    private float startPosX;//Kartın ilk yatay konumu
    private bool isTouching;//Ekrana tıklama kontrolü
    public float maxDistance = 1.5f;//Kartı kaydırabileceğimiz max mesafe
    public float rotation = 5;

    public Vector3 firstTouchPos;

    #region
    public string characterString, characterName;//Karakterlerin konuşma metinleri
    public bool isSelected;// Kartın seçili olup olmama durumu
    public TextMeshProUGUI rightText, leftText;//Sağ ve sol seçimler
    #endregion

    public static Card instance;//Singleton

    private bool hasPlayedHoldSound = false; // Sesin çalınıp çalınmadığını kontrol eden değişken

    public int cardSpeed;//Kartın kaydırılma hızı







    public float healthChange;
    public float farmerChange;
    public float peopleChange;
    public float economyChange;

    private BarControl barControl;

    private void Awake()
    {
        instance = this;
        barControl = FindAnyObjectByType<BarControl>();
    }

    private void Start()
    {
        originalPosition = transform.position;//Başlangıç konumunu tutar
        startPosX = originalPosition.x;//Başlangıçtaki yatay konumunu tutar
    }

    private void Update()
    {
        TouchControl();
        if (!isTouching)
        {
            ManageCard();
        }
    }

    //Input sistemi 
    void TouchControl()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane));
            touchPosition.z = 0;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    isTouching = CheckCard(touchPosition);
                    firstTouchPos = touchPosition;
                    hasPlayedHoldSound = false; // Yeni dokunuş başladığında sıfırla
                    break;

                case TouchPhase.Moved:
                    MoveCard(touchPosition - firstTouchPos);
                    firstTouchPos = touchPosition;
                    break;

                case TouchPhase.Ended:
                    isTouching = false;
                    hasPlayedHoldSound = false; // Dokunuş bittiğinde tekrar oynatılabilir hale getir                                     
                    break;
            }
        }
    }

    private bool CheckCard(Vector3 touchPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, Mathf.Infinity, cardLayermask);
        return hit.collider != null;
    }

    private void MoveCard(Vector3 moveAmount)
    {
        if (isTouching)
        {
            transform.position += moveAmount;
            RotateCard();

            // Eğer ses daha önce çalınmadıysa, çal ve flag'i güncelle
            if (!hasPlayedHoldSound)
            {
                AudioManager.instance.Play("HoldCard");
                hasPlayedHoldSound = true;
            }
        }
    }

    private void ReturnToOriginalPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, Time.deltaTime * cardSpeed);
        transform.eulerAngles = Vector3.zero;
        leftText.gameObject.SetActive(false);
        rightText.gameObject.SetActive(false);
    }

    private void RotateCard()
    {
        rightText.gameObject.SetActive(transform.position.x > startPosX);
        leftText.gameObject.SetActive(transform.position.x < startPosX);
        transform.eulerAngles = new Vector3(0, 0, (startPosX - transform.position.x) * 10f);
    }

    private void ManageCard()
    {
        float distance = Mathf.Abs(originalPosition.x - transform.position.x);

        if (distance >= maxDistance)
        {
            AudioManager.instance.Play("CardSelected");
            isSelected = true;
            ApplyBarEffect();
            Destroy(gameObject);
        }
        else
        {
            ReturnToOriginalPosition();
        }
    }

    // Bar değerlerini değiştir
    private void ApplyBarEffect()
    {
        if (barControl == null) return;

        if (transform.position.x > startPosX || transform.position.x < startPosX) // Kart sağa veya sola kaydırıldıysa barları değiştir
        {
            barControl.ModifyMilitary(healthChange);
            barControl.ModifyEconomy(farmerChange);
            barControl.ModifyFarm(peopleChange);
            barControl.ModifyPublic(economyChange);
        }
    }
}

