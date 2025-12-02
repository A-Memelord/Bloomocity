using UnityEngine;
using TMPro;

public class PlantShop : MonoBehaviour
{
    public TMP_Text plantNameText;
    public TMP_Text plantCostText;

    public PlantShopObject plantShopObjects;

    void Start()
    {
        plantNameText.text = plantShopObjects.plantName;
        plantCostText.text = "$" + plantShopObjects.plantCost.ToString();
    }

    void Update()
    {
        
    }

    public void Buy()
    {
        if (SaveDataController.Instance.CurrentData.Money >= plantShopObjects.plantCost)
        {
            SaveDataController.Instance.CurrentData.Money -= plantShopObjects.plantCost;

            // Add Plant To The Player's Inventory

        }
    }
}
