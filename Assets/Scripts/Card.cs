using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    public LayerMask cardLayermask;//KArtýn layerý
    private Vector3 originalPosition;//Kartýn ilk konumu
    private float startPosX;//Kartýn ilk yatay konumu
    private bool isTouching;//Ekrana týklama kontrolü
    public float maxDistance = 1.5f;//Kartý kaydýrabileceðimiz max mesafe
    public float rotation=5;



    #region
    public string text;//Karakterlerin konuþma metinleri
    public bool isSelected;// Kartýn seçili olup olmama durumu
    public TextMeshProUGUI rightText, leftText;//Sað ve sol seçimler
    #endregion

    public float healthChange;
    public float farmerChange;
    public float peopleChange;
    public float economyChange;

    private BarManager barManager;

    public static Card instance;//Singelton


    private void Awake()
    {
        instance = this;
        barManager = FindFirstObjectByType<BarManager>();
    }

    private void Start()
    {

        originalPosition = transform.position;//baþlangýç konumunu tutar
        startPosX = originalPosition.x;//Baþlangýçtaki yatay konumunu tutar
    }

    private void Update()
    {
        TouchControl();
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
                    CheckCard(touchPosition);
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (isTouching) MoveCard(touchPosition);
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isTouching = false;
                    ManageCard();
                    
                    break;
            }
        }
    }


    //Ýlk týklandýðý zaman kart mý diye kontrol et
    private void CheckCard(Vector3 touchPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, Mathf.Infinity, cardLayermask);

        if (hit.collider != null) isTouching = true;
        else isTouching = false;

    }


    //Kartýn sað sol hareketi
    private void MoveCard(Vector3 touchPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, Mathf.Infinity);
        transform.position = hit.point;
        RotateCard();

    }

    //Baþlangýç pozisyonuna döndür
    private void ReturnToOriginalPosition()
    {
        transform.position = originalPosition;
        transform.eulerAngles = Vector3.zero;
        leftText.gameObject.SetActive(false);
        rightText.gameObject.SetActive(false);
    }

    private void RotateCard()
    {
        //Kart saðda ise
        if (transform.position.x > startPosX)
        {
            leftText.gameObject.SetActive(false);
            rightText.gameObject.SetActive(true);
            transform.eulerAngles = new Vector3(0, 0, -rotation);
        }
        //Kart solda ise
        else if (transform.position.x < startPosX)
        {
            rightText.gameObject.SetActive(false);
            leftText.gameObject.SetActive(true);
            transform.eulerAngles = new Vector3(0, 0, rotation);
        }
    }

   



    //Kartý belli bir mesafeye sürüklediðimiz zaman kartýn durumu
    private void ManageCard()
    {
        float distance = Mathf.Abs(originalPosition.x - transform.position.x);
        //Kart max sürükleme mesafesini aþarsa kartý seçildi olarak seç aþmazsa ilk konumuna geri getir
        if (distance >= maxDistance)
        {
            isSelected = true;
            ApplyBarEffect();
        }
        else ReturnToOriginalPosition();
    }

    // Bar deðerlerini deðiþtir
    private void ApplyBarEffect()
    {
        if (barManager == null) return;

        if (transform.position.x > startPosX) // Kart saða kaydýrýldýysa
        {
            barManager.ModifyHealth(healthChange);
            barManager.ModifyFarmer(farmerChange);
            barManager.ModifyPeople(peopleChange);
            barManager.ModifyEconomy(economyChange);
        }
        else if (transform.position.x < startPosX) // Kart sola kaydýrýldýysa
        {
            barManager.ModifyHealth(-healthChange);
            barManager.ModifyFarmer(-farmerChange);
            barManager.ModifyPeople(-peopleChange);
            barManager.ModifyEconomy(-economyChange);
        }
    }
}
