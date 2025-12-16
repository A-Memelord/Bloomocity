using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantShop : MonoBehaviour
{
    public static PlantShop instance;

    public TMP_Text plantNameText;
    public TMP_Text plantCostText;
    public TMP_Text buttonText;
    public GameObject interactButton;
    public GameObject player;

    public InventoryItemData InventoryItemData;

    public double plantSellValue;
    private bool _buyBool = true;

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        plantNameText.text = InventoryItemData.itemName;
        plantCostText.text = "$" + InventoryItemData.value.ToString();
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
        plantSellValue = InventoryItemData.value * 2f;

        if (_buyBool == true)
        {
            plantNameText.text = InventoryItemData.itemName;
            plantCostText.text = "$" + InventoryItemData.value.ToString();
            buttonText.text = "Buy";
            interactButton.GetComponent<Image>().color = Color.green;
        }
        else if (_buyBool == false)
        {
            plantNameText.text = InventoryItemData.itemName;
            plantCostText.text = "$" + plantSellValue.ToString();
            buttonText.text = "Sell";
            interactButton.GetComponent<Image>().color = Color.red;
        }
    }

    public void Interact()
    {
        var inventory = player.transform.GetComponent<PlayerInventoryHolder>();

        if (_buyBool == true)
        {
            // Buy Plant Logic Here
            if (SaveDataController.Instance.CurrentData.Money >= InventoryItemData.value)
            {
                SaveDataController.Instance.CurrentData.Money -= InventoryItemData.value;

                // Add Plant To The Player's Inventory
                

                inventory.AddToInventory(InventoryItemData, 1);
                print("Trigger");
            }
        }
        else if (_buyBool == false)
        {
            // Sell Plant Logic Here
            if (/* Check If Player Has The Plant To Sell */ true)
            {
                SaveDataController.Instance.CurrentData.Money += plantSellValue;

                // Remove Plant From The Player's Inventory

                inventory.RemoveFromInventory(InventoryItemData, 1);
            }
        }
    }
}
