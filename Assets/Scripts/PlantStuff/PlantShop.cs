using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantShop : MonoBehaviour
{

    public TMP_Text plantNameText;
    public TMP_Text plantCostText;
    public TMP_Text buttonText;
    public GameObject interactButton;
    public GameObject player;

    public InventoryItemData InventoryItemData;

    public double plantSellValue;
    private bool _buyBool = true;


    void Start()
    {
        plantNameText.text = InventoryItemData.itemName;
        plantCostText.text = "$" + InventoryItemData.value.ToString();
        buttonText.text = "Buy";
        interactButton.GetComponent<Image>().color = Color.green;
        interactButton.GetComponent<Button>().onClick.AddListener(Interact);
    }


    void Update()
    {
        _buyBool = ShopKeeper.instance.buyBool;

        plantSellValue = InventoryItemData.value / 2f;

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
        print(_buyBool);
    }

    /*
    PSEUDOCODE / PLAN (detailed)
    - Get the player's PlayerInventoryHolder component.
    - If shop is in Buy mode:
      1. Check if player has enough money.
      2. Try to add the item to the player's inventory first.
         - If AddToInventory returns true:
             a. Deduct the item's cost from player's money.
             b. Invoke any inventory-changed event to update UI.
             c. Log success.
         - If AddToInventory returns false:
             a. Do not change money.
             b. Log failure (e.g., inventory full).
    - If shop is in Sell mode:
      1. Attempt to remove the item from the player's inventory by calling RemoveFromInventory.
         - If RemoveFromInventory returns true:
             a. Add the sell value to player's money.
             b. Invoke any inventory-changed event to update UI.
             c. Log success.
         - If RemoveFromInventory returns false:
             a. Do not change money.
             b. Log that player doesn't have the item.
    - This ensures money only changes when the inventory operation actually succeeds.
    */

    public void Interact()
    {
        var inventory = player.transform.GetComponent<PlayerInventoryHolder>();
        if (inventory == null)
        {
            Debug.LogWarning("PlayerInventoryHolder not found on player.");
            return;
        }

        if (_buyBool == true)
        {
            // Buy Plant Logic
            double cost = InventoryItemData.value;
            if (SaveDataController.Instance.CurrentData.Money >= cost)
            {
                // Try to add to inventory first; only deduct money on success
                bool added = inventory.AddToInventory(InventoryItemData, 1);
                if (added)
                {
                    SaveDataController.Instance.CurrentData.Money -= cost;
                    PlayerInventoryHolder.OnPlayerInventoryChanged?.Invoke();
                    Debug.Log($"Bought 1x {InventoryItemData.itemName} for ${cost}");
                }
                else
                {
                    Debug.Log("Cannot add item to inventory (inventory full or other constraint).");
                }
            }
            else
            {
                Debug.Log("Not enough money to buy item.");
            }
        }
        else // Sell mode
        {
            // Sell Plant Logic
            // Attempt to remove from inventory; only give money if removal succeeded
            bool removed = inventory.RemoveFromInventory(InventoryItemData, 1);
            if (removed)
            {
                SaveDataController.Instance.CurrentData.Money += plantSellValue;
                PlayerInventoryHolder.OnPlayerInventoryChanged?.Invoke();
                Debug.Log($"Sold 1x {InventoryItemData.itemName} for ${plantSellValue}");
            }
            else
            {
                Debug.Log("Player does not have the item to sell.");
            }
        }
    }
}
