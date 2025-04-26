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


    [Header("Sola kaydırınca barlara etkileri")]
    public float leftMilitaryChange;
    public float leftFarmChange;
    public float leftPublicChange;
    public float leftEconomyChange;


    [Header("Sağa kaydırınca barlara etkileri")]
    public float rightMilitaryChange;
    public float rightFarmChange;
    public float rightPublicChange;
    public float rightEconomyChange;

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
                    if (barControl != null)
                        barControl.ResetBarColors();
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

            if (!hasPlayedHoldSound)
            {
                AudioManager.instance.Play("HoldCard");
                hasPlayedHoldSound = true;
            }

            // 🔥 BURASI ÖNEMLİ: Bar rengi değişimi
            if (barControl != null)
            {
                float direction = transform.position.x - startPosX;

                if (Mathf.Abs(direction) > 0.1f)
                {
                    if (direction > 0) // sağa kaydırma
                    {
                        barControl.PreviewBarEffects(
                            rightEconomyChange,
                            rightFarmChange,
                            rightPublicChange,
                            rightMilitaryChange
                        );
                    }
                    else // sola kaydırma
                    {
                        barControl.PreviewBarEffects(
                            leftEconomyChange,
                            leftFarmChange,
                            leftPublicChange,
                            leftMilitaryChange
                        );
                    }
                }
                else
                {
                    barControl.ResetBarColors(); // çok az hareket varsa resetle
                }
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

        if (transform.position.x > startPosX ) // Kart sağa kaydırıldıysa barları değiştir
        {
            barControl.ModifyMilitary(rightMilitaryChange);
            barControl.ModifyEconomy(rightEconomyChange);
            barControl.ModifyFarm(rightFarmChange);
            barControl.ModifyPublic(rightPublicChange);
        }

        if (transform.position.x < startPosX) // kart sola kaydırıldıysa barları değiştir
        {
            barControl.ModifyMilitary(leftMilitaryChange);
            barControl.ModifyEconomy(leftEconomyChange);
            barControl.ModifyFarm(leftFarmChange);
            barControl.ModifyPublic(leftPublicChange);
        } 
    }
}

