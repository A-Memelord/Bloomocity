using UnityEngine;
using TMPro;

public class PlantShop : MonoBehaviour
{
    public TMP_Text plantNameText;
    public TMP_Text plantCostText;
    public TMP_Text buttonText;

    public PlantShopObject plantShopObjects;

    void Start()
    {
        plantNameText.text = plantShopObjects.plantName;
        plantCostText.text = "$" + plantShopObjects.plantCost.ToString();
        buttonText.text = "Buy";
    }

    void Update()
    {
        //if()
    }

    public void Interact()
    {
        if (SaveDataController.Instance.CurrentData.Money >= plantShopObjects.plantCost)
        {
            SaveDataController.Instance.CurrentData.Money -= plantShopObjects.plantCost;

            // Add Plant To The Player's Inventory

        }
    }
}
