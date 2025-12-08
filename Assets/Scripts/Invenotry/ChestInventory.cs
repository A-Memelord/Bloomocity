using System;
using UnityEngine;
using UnityEngine.Events;

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
        var chestSaveData = new ChestSaveData(primaryInventorySystem, this.transform.position, this.transform.rotation);

        SaveGamemanager.data.chestDictionary.Add(GetComponent<UniqueID>().ID, chestSaveData);
    }

    private void LoadInventory(SaveInvData data)
    {
        // Check The Save Data For This Specific Chests Save Data And If Found Load it Into This Chest
        if (data.chestDictionary.TryGetValue(GetComponent<UniqueID>().ID, out ChestSaveData chestData))
        {
            this.primaryInventorySystem = chestData.invSystem;
            this.transform.position = chestData.pos;
            this.transform.rotation = chestData.rot;
        }
    }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        OnDynamicInventoryDisplayRequested?.Invoke(primaryInventorySystem);
        interactSuccessful = true;
    }

    public void EndInteraction()
    {
        OnCloseChestRequested?.Invoke(primaryInventorySystem);
    }
}

[Serializable]
public struct ChestSaveData
{
    public InventorySystem invSystem;
    public Vector3 pos;
    public Quaternion rot;

    public ChestSaveData(InventorySystem _invSystem, Vector3 _pos, Quaternion _rot)
    {
        invSystem = _invSystem;
        pos = _pos;
        rot = _rot;
    }
}