using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    public LayerMask cardLayermask;//KArt�n layer�
    private Vector3 originalPosition;//Kart�n ilk konumu
    private float startPosX;//Kart�n ilk yatay konumu
    private bool isTouching;//Ekrana t�klama kontrol�
    public float maxDistance = 1.5f;//Kart� kayd�rabilece�imiz max mesafe
    public float rotation=5;



    #region
    public string text;//Karakterlerin konu�ma metinleri
    public bool isSelected;// Kart�n se�ili olup olmama durumu
    public TextMeshProUGUI rightText, leftText;//Sa� ve sol se�imler
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

        originalPosition = transform.position;//ba�lang�� konumunu tutar
        startPosX = originalPosition.x;//Ba�lang��taki yatay konumunu tutar
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


    //�lk t�kland��� zaman kart m� diye kontrol et
    private void CheckCard(Vector3 touchPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, Mathf.Infinity, cardLayermask);

        if (hit.collider != null) isTouching = true;
        else isTouching = false;

    }


    //Kart�n sa� sol hareketi
    private void MoveCard(Vector3 touchPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, Mathf.Infinity);
        transform.position = hit.point;
        RotateCard();

    }

    //Ba�lang�� pozisyonuna d�nd�r
    private void ReturnToOriginalPosition()
    {
        transform.position = originalPosition;
        transform.eulerAngles = Vector3.zero;
        leftText.gameObject.SetActive(false);
        rightText.gameObject.SetActive(false);
    }

    private void RotateCard()
    {
        //Kart sa�da ise
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

   



    //Kart� belli bir mesafeye s�r�kledi�imiz zaman kart�n durumu
    private void ManageCard()
    {
        float distance = Mathf.Abs(originalPosition.x - transform.position.x);
        //Kart max s�r�kleme mesafesini a�arsa kart� se�ildi olarak se� a�mazsa ilk konumuna geri getir
        if (distance >= maxDistance)
        {
            isSelected = true;
            ApplyBarEffect();
        }
        else ReturnToOriginalPosition();
    }

    // Bar de�erlerini de�i�tir
    private void ApplyBarEffect()
    {
        if (barManager == null) return;

        if (transform.position.x > startPosX) // Kart sa�a kayd�r�ld�ysa
        {
            barManager.ModifyHealth(healthChange);
            barManager.ModifyFarmer(farmerChange);
            barManager.ModifyPeople(peopleChange);
            barManager.ModifyEconomy(economyChange);
        }
        else if (transform.position.x < startPosX) // Kart sola kayd�r�ld�ysa
        {
            barManager.ModifyHealth(-healthChange);
            barManager.ModifyFarmer(-farmerChange);
            barManager.ModifyPeople(-peopleChange);
            barManager.ModifyEconomy(-economyChange);
        }
    }
}
