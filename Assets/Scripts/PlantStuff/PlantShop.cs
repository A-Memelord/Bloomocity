using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantShop : MonoBehaviour
{
    public TMP_Text plantNameText;
    public TMP_Text plantCostText;
    public TMP_Text buttonText;
    public GameObject interactButton;

    public PlantShopObject plantShopObjects;

    public double plantSellValue;

    void Start()
    {
        plantNameText.text = plantShopObjects.plantName;
        plantCostText.text = "$" + plantShopObjects.plantCost.ToString();
        buttonText.text = "Buy";
        interactButton.GetComponent<Image>().color = Color.green;
        interactButton.GetComponent<Button>().onClick.AddListener(Interact);
    }

    void Update()
    {
        plantSellValue = plantShopObjects.plantCost * 2f;

        if (ShopSystem.instance.buyBool == true)
        {
            plantNameText.text = plantShopObjects.plantName;
            plantCostText.text = "$" + plantShopObjects.plantCost.ToString();
            buttonText.text = "Buy";
            interactButton.GetComponent<Image>().color = Color.green;
        }
        else if(ShopSystem.instance.buyBool == false)
        {
            plantNameText.text = plantShopObjects.plantName;
            plantCostText.text = "$" + plantSellValue.ToString();
            buttonText.text = "Sell";
            interactButton.GetComponent<Image>().color = Color.red;
        }
    }

    public void Interact()
    {
        if (ShopSystem.instance.buyBool == true)
        {
            // Buy Plant Logic Here
            if (SaveDataController.Instance.CurrentData.Money >= plantShopObjects.plantCost)
            {
                SaveDataController.Instance.CurrentData.Money -= plantShopObjects.plantCost;

                // Add Plant To The Player's Inventory

            }
        }
        else if (ShopSystem.instance.buyBool == false)
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
