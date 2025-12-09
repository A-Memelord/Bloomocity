using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class ChestInventory : InventoryHolder, IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    protected override void Awake()
    {
        base.Awake();
        SaveLoad.OnLoad += LoadInventory;
    }

    void Start()
    {
        var chestSaveData = new InventorySaveData(primaryInventorySystem, this.transform.position, this.transform.rotation);

        SaveGameManager.data.chestDictionary.Add(GetComponent<UniqueID>().ID, chestSaveData);
    }

    protected override void LoadInventory(SaveInvData data)
    {
        // Check The Save Data For This Specific Chests Save Data And If Found Load it Into This Chest
        if (data.chestDictionary.TryGetValue(GetComponent<UniqueID>().ID, out InventorySaveData chestData))
        {
            this.primaryInventorySystem = chestData.invSystem;
            this.transform.position = chestData.pos;
            this.transform.rotation = chestData.rot;
        }
    }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        OnDynamicInventoryDisplayRequested?.Invoke(primaryInventorySystem, 0);
        interactSuccessful = true;
    }

    public void EndInteraction()
    {
        OnCloseChestRequested?.Invoke(primaryInventorySystem, 0);
    }
}