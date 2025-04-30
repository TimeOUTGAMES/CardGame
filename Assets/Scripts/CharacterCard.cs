using TMPro;
using UnityEngine;

public class CharacterCard : Cards
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI rightText;
    [SerializeField] private TextMeshProUGUI leftText;

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

    // Threshold for activation of preview effects
    private const float DIRECTION_THRESHOLD = 0.1f;
    
    // Remove singleton if possible and unnecessary
    public static CharacterCard instance { get; private set; }

    private void Awake()
    {
        instance = this;
        
        // Use GetComponent or other proper reference methods instead of FindAnyObjectByType
        // FindAnyObjectByType is inefficient and can cause performance issues
        if (barControl == null)
        {
            barControl = GameObject.FindObjectOfType<BarControl>();
            
            if (barControl == null)
            {
                Debug.LogWarning("BarControl reference not found. Bar effects won't work.");
            }
        }
    }

    // No need to override Update if it only calls the base method
    
    protected override void MoveCard(Vector3 moveAmount)
    {
        base.MoveCard(moveAmount);
        
        if (!isTouching || barControl == null) return;
        
        float direction = transform.position.x - startPosX;
        
        if (Mathf.Abs(direction) > DIRECTION_THRESHOLD)
        {
            if (direction > 0) // Right swipe
            {
                barControl.PreviewBarEffects(
                    rightEconomyChange,
                    rightFarmChange,
                    rightPublicChange,
                    rightMilitaryChange
                );
            }
            else // Left swipe
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
            barControl.ResetBarColors(); // Reset if movement is minimal
        }
    }

    protected override void ReturnToOriginalPosition()
    {
        base.ReturnToOriginalPosition();
        
        SetTextVisibility(false, false);
    }

    protected override void RotateCard()
    {
        base.RotateCard();
        
        bool isRightSwipe = transform.position.x > startPosX;
        SetTextVisibility(isRightSwipe, !isRightSwipe);
    }

    // Helper method to manage text visibility
    private void SetTextVisibility(bool showRightText, bool showLeftText)
    {
        if (rightText != null && leftText != null)
        {
            rightText.gameObject.SetActive(showRightText);
            leftText.gameObject.SetActive(showLeftText);
        }
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
        
        if (direction > 0) // Right swipe effect application
        {
            ApplyEffects(rightEconomyChange, rightFarmChange, rightPublicChange, rightMilitaryChange);
        }
        else if (direction < 0) // Left swipe effect application
        {
            ApplyEffects(leftEconomyChange, leftFarmChange, leftPublicChange, leftMilitaryChange);
        }
    }
    
    // Helper method to apply all effects at once
    private void ApplyEffects(float economyChange, float farmChange, float publicChange, float militaryChange)
    {
        barControl.ModifyEconomy(economyChange);
        barControl.ModifyFarm(farmChange);
        barControl.ModifyPublic(publicChange);
        barControl.ModifyMilitary(militaryChange);
    }
}