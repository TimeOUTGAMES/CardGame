using System;
using TMPro;
using UnityEngine;
using DG.Tweening;
public class UIManager : MonoBehaviour
{

    private GameManager gameManager;
    private Transform cardsTransform;
    [SerializeField]
    public TextMeshProUGUI characterString, characterName, currentEraName;
    public static UIManager instance;

    [SerializeField] private float characterStringFadeSpeed = 0.4f;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameManager = GameManager.instance;
        cardsTransform = gameManager.cardsTransform;
        
    }

    
    public void ShowText()
    {
        if (gameManager.IsTransitioning)
        {
            characterString.text = "";
            characterName.text = "";
            currentEraName.text = "";
            return;
        }

        if (cardsTransform.childCount > 0)
        {
            if (cardsTransform.GetChild(0).GetComponent<CharacterCard>() != null)
            {
                Color color = characterString.color;
                color.a = 0;
                characterString.color = color;
                
                characterString.text = $"{cardsTransform.GetChild(0).GetComponent<CharacterCard>().characterString}";
                characterString.DOFade(1f, characterStringFadeSpeed).SetEase(Ease.InOutSine);
                
                characterName.text = $"{cardsTransform.GetChild(0).GetComponent<CharacterCard>().characterName}";
                UpdateCurrentEraName(cardsTransform.GetChild(0).GetComponent<CharacterCard>().currentEra);
            }
            else if (cardsTransform.GetChild(0).GetComponent<AICard>() != null)
            {
                Color color = characterString.color;
                color.a = 0;
                characterString.color = color;
                
                characterString.text = $"{cardsTransform.GetChild(0).GetComponent<AICard>().characterString}";
                characterString.DOFade(1f, characterStringFadeSpeed).SetEase(Ease.InOutSine);
                
                characterName.text = $"{cardsTransform.GetChild(0).GetComponent<AICard>().characterName}";
                UpdateCurrentEraName(cardsTransform.GetChild(0).GetComponent<AICard>().currentEra);
            }
            
            else if (cardsTransform.GetChild(0).GetComponent<SpecialCharacterCard>() != null)
            {
                Color color = characterString.color;
                color.a = 0;
                characterString.color = color;
                
                characterString.text = $"{cardsTransform.GetChild(0).GetComponent<SpecialCharacterCard>().characterString}";
                characterString.DOFade(1f, characterStringFadeSpeed).SetEase(Ease.InOutSine);
                
                characterName.text = $"{cardsTransform.GetChild(0).GetComponent<SpecialCharacterCard>().characterName}";
                UpdateCurrentEraName(cardsTransform.GetChild(0).GetComponent<SpecialCharacterCard>().currentEra);
            }
        }
        else
        {
            characterString.text = "Malsin";
            characterName.text = "";
        }

    }

    public void UpdateCurrentEraName(int era)
    {
        switch (era)
        {
            case 1:
                currentEraName.text = "Modern Era";
                break;
            case 2:
                currentEraName.text = "Middle Era";
                break;
            case 3:
                currentEraName.text = "Ancient Era";
                break;
            default:
                currentEraName.text = "";
                break;
            
        }
    }
    
}
