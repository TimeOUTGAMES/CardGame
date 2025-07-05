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
        base.MoveCard(moveAmount);

        if (!isTouching || barControl == null) return;

        float direction = transform.position.x - startPosX;

        if (Mathf.Abs(direction) > DIRECTION_THRESHOLD)
        {
            if (direction > 0) // Sað
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

        if (barControl != null)
        {
            barControl.ResetBarColors();
        }
    }

    protected override void RotateCard()
    {
        base.RotateCard();

        bool isRightSwipe = transform.position.x > startPosX;
        SetTextVisibility(isRightSwipe, !isRightSwipe);
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

        if (direction > 0) // Sað
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

    private void SetTextVisibility(bool showRightText, bool showLeftText)
    {
        if (rightText != null)
            rightText.gameObject.SetActive(showRightText);

        if (leftText != null)
            leftText.gameObject.SetActive(showLeftText);
    }
}
