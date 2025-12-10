using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantShop : MonoBehaviour
{
    public static PlantShop instance;

    public TMP_Text plantNameText;
    public TMP_Text plantCostText;
    public TMP_Text buttonText;
    public GameObject interactButton;

    public PlantShopObject plantShopObjects;

    public double plantSellValue;
    private bool _buyBool = false;

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        plantNameText.text = plantShopObjects.plantName;
        plantCostText.text = "$" + plantShopObjects.plantCost.ToString();
        buttonText.text = "Buy";
        interactButton.GetComponent<Image>().color = Color.green;
        interactButton.GetComponent<Button>().onClick.AddListener(Interact);
    }

    public void ChangeBool(bool value)
    {
        _buyBool = value;
    }

    void Update()
    {
        plantSellValue = plantShopObjects.plantCost * 2f;

        if (_buyBool == true)
        {
            plantNameText.text = plantShopObjects.plantName;
            plantCostText.text = "$" + plantShopObjects.plantCost.ToString();
            buttonText.text = "Buy";
            interactButton.GetComponent<Image>().color = Color.green;
        }
        else if (_buyBool == false)
        {
            plantNameText.text = plantShopObjects.plantName;
            plantCostText.text = "$" + plantSellValue.ToString();
            buttonText.text = "Sell";
            interactButton.GetComponent<Image>().color = Color.red;
        }
    }

    public void Interact()
    {
        if (_buyBool == true)
        {
            // Buy Plant Logic Here
            if (SaveDataController.Instance.CurrentData.Money >= plantShopObjects.plantCost)
            {
                SaveDataController.Instance.CurrentData.Money -= plantShopObjects.plantCost;

                // Add Plant To The Player's Inventory

            }
        }
        else if (_buyBool == false)
        {
            // Sell Plant Logic Here
            if (/* Check If Player Has The Plant To Sell */ true)
            {
                SaveDataController.Instance.CurrentData.Money += plantSellValue;

                // Remove Plant From The Player's Inventory

            }
        }
    }
}
