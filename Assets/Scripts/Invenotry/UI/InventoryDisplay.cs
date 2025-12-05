using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] MouseItemData mouseInventoryItem;

    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlot_UI, InventorySlot> slotDictionary;
    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary => slotDictionary;

    protected virtual void Start()
    {

    }

    public abstract void AssignSlot(InventorySystem invToDisplay);

    protected virtual void UpdateSlot(InventorySlot updateSlot)
    {
        foreach (var slot in SlotDictionary)
        {
            if (slot.Value == updateSlot) // Slot value - the "under the hood" inventory slot
            {
                slot.Key.UpdateUISlot(updateSlot); // Slot key - the UI representation of the value
            }
        }
    }

    public void SlotClicked(InventorySlot_UI clickedUISlot)
    {

        // Clicked Slot Has An Item - Mouse Dosn't Have An Item - Pick Up That Item
        if (clickedUISlot.AssignedInventorySlot.ItemData != null && mouseInventoryItem.AssignedInventorySlot.ItemData == null)
        {
            // If Player Is Holding Shift Key? Split The Stack

            mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);
            clickedUISlot.ClearSlot();
            return;
        }

        // Clicker Slot Doesn't Have An Item - Mouse Does Have An Item - Place The Mouse Item Into The Empty Slot
        if (clickedUISlot.AssignedInventorySlot.ItemData == null && mouseInventoryItem.AssignedInventorySlot.ItemData != null)
        {
            clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
            clickedUISlot.UpdateUISlot();

            mouseInventoryItem.ClearSlot();
            return;
        }

        // Both Slots Have An Item - Decide What To Do...
            // Are Both Items The Same? If So Combine Them
                // Is The Slot Stack Size + Mouse Stack Size > Max Stack Size? If So, Take Form The Mouse
            // If Different Items, Then Swap The Items
        
    }
}
