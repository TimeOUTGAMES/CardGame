using DG.Tweening;
using TMPro;
using UnityEngine;

public class CharacterCard : Cards
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI rightText;
    [SerializeField] private TextMeshProUGUI leftText;
    [SerializeField] [Range(0, 1)] private float textFadeDistance = 0.5f;

    [Header("Left Swipe Effects")]
    [SerializeField] private float leftMilitaryChange;
    [SerializeField] private float leftFarmChange;
    [SerializeField] private float leftPublicChange;
    [SerializeField] private float leftEconomyChange;

    [Header("Right Swipe Effects")]
    [SerializeField] private float rightMilitaryChange;
    [SerializeField] private float rightFarmChange;
    [SerializeField] private float rightPublicChange;
    [SerializeField] private float rightEconomyChange;
    

    private const float DIRECTION_THRESHOLD = 0.1f;

    private static CharacterCard _instance;
    public static CharacterCard instance => _instance;

    private void Awake()
    {
        _instance = this;

        if (barControl == null)
        {
            barControl = FindFirstObjectByType<BarControl>();
            if (barControl == null)
            {
                Debug.LogWarning("BarControl reference not found. Bar effects won't work.");
            }
        }
    }

    protected override void MoveCard(Vector3 moveAmount)
    {
        leftText.gameObject.SetActive(true);
        rightText.gameObject.SetActive(true);
        
        base.MoveCard(moveAmount);

        if (!isTouching || barControl == null) return;

        float direction = transform.position.x - startPosX;
        
        float fadeDirection = Mathf.Clamp(direction, -textFadeDistance, textFadeDistance);

        float leftAlpha = Mathf.InverseLerp(0f, -textFadeDistance, fadeDirection); 
        float rightAlpha = Mathf.InverseLerp(0f, textFadeDistance, fadeDirection);
        
        SetTextAlpha(leftText, leftAlpha);
        SetTextAlpha(rightText, rightAlpha);

        if (Mathf.Abs(direction) > DIRECTION_THRESHOLD)
        {
            if (direction > 0) // Sa�
            {
                barControl.PreviewBarEffects(
                    rightEconomyChange,
                    rightFarmChange,
                    rightPublicChange,
                    rightMilitaryChange
                );
            }
            else // Sol
            {
                barControl.PreviewBarEffects(
                    leftEconomyChange,
                    leftFarmChange,
                    leftPublicChange,
                    leftMilitaryChange
                );
            }
        }
    }

    protected override void ReturnToOriginalPosition()
    {
        base.ReturnToOriginalPosition();
        SetTextVisibility(false, false);
        SetTextAlpha(leftText,0);
        SetTextAlpha(rightText,0);
        
        if (barControl != null)
        {
            barControl.ResetBarColors();
        }
    }

    protected override void RotateCard()
    {
        base.RotateCard();

        bool isRightSwipe = transform.position.x > startPosX;
        
    }

    protected override void ManageCard()
    {
        base.ManageCard();

        if (distance >= maxDistance)
        {
            ApplyBarEffect();
        }
    }

    private void ApplyBarEffect()
    {
        if (barControl == null) return;

        float direction = transform.position.x - startPosX;

        if (direction > 0) // Sa�
        {
            barControl.ApplyEffects(
                rightEconomyChange,
                rightFarmChange,
                rightPublicChange,
                rightMilitaryChange
            );
        }
        else if (direction < 0) // Sol
        {
            barControl.ApplyEffects(
                leftEconomyChange,
                leftFarmChange,
                leftPublicChange,
                leftMilitaryChange
            );
        }
    }

    private void SetTextVisibility(bool showRightText, bool showLeftText)  //BURAYI DÜZELT BETO
    {
   		//Null
    }
    
    void SetTextAlpha(TextMeshProUGUI text, float alpha)
    {
        Color c = text.color;
        c.a = alpha;
        text.color = c;
    }
    
}
