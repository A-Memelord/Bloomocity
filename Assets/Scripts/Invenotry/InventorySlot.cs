using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    [SerializeField] private InvenotryItemData itemData;
    [SerializeField] private int stackSize;

    public InvenotryItemData ItemData => itemData;
    public int StackSize => stackSize;

    public InventorySlot(InvenotryItemData source, int amount)
    {
        itemData = source;
        stackSize = amount;
    }

    public InventorySlot()
    {
        ClearSlot();
    }

    public void ClearSlot()
    {
        itemData = null;
        stackSize = -1;
    }

    public bool RoomLeftInStack(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = itemData.maxStackSize - stackSize;

        return RoomLeftInStack(amountToAdd);
    }

    public bool RoomLeftInStack(int amountToAdd)
    {
        if (stackSize + amountToAdd <= itemData.maxStackSize) return true;
        else return false;
    }

    public void AddToStack(int amount)
    {
        stackSize += amount;
    }

    public void RemoveFromStack(int amount)
    {
        stackSize -= amount;
    }
}
