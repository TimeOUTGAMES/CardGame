using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    private GameManager gameManager;
    private Transform cardsTransform;
    [SerializeField]
    public TextMeshProUGUI characterString, characterName, currentEraName;
    void Start()
    {
        gameManager = GameManager.instance;
        cardsTransform = gameManager.cardsTransform;
    }


    void Update()
    {
        ShowText();
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
                characterString.text = $"{cardsTransform.GetChild(0).GetComponent<CharacterCard>().characterString}";
                characterName.text = $"{cardsTransform.GetChild(0).GetComponent<CharacterCard>().characterName}";
                UpdateCurrentEraName(cardsTransform.GetChild(0).GetComponent<CharacterCard>().currentEra);
            }
            else if (cardsTransform.GetChild(0).GetComponent<AICard>() != null)
            {
                characterString.text = $"{cardsTransform.GetChild(0).GetComponent<AICard>().characterString}";
                characterName.text = $"{cardsTransform.GetChild(0).GetComponent<AICard>().characterName}";
                UpdateCurrentEraName(cardsTransform.GetChild(0).GetComponent<AICard>().currentEra);
            }
        }
        else
        {
            characterString.text = "";
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
