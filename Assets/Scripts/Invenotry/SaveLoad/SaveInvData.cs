using System.Collections.Generic;
using UnityEngine;

public class SaveInvData
{
    public List<string> collectedItems;
    public SerializableDictionary<string, ItemPickupSaveData> activeItems;

    public SerializableDictionary<string, InventorySaveData> chestDictionary;

    public InventorySaveData playerInventory;

    public SaveInvData()
    {
        collectedItems = new List<string>();
        activeItems = new SerializableDictionary<string, ItemPickupSaveData>();
        chestDictionary = new SerializableDictionary<string, InventorySaveData>();
        playerInventory = new InventorySaveData();
    }
}