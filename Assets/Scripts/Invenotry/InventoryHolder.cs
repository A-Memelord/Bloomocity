using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int inventorySize;
    [SerializeField] protected InventorySystem primaryInventorySystem;
    [SerializeField] protected int offset = 10;

    public int Offset => offset;

    public InventorySystem PrimaryInventorySystem => primaryInventorySystem;

    public static UnityAction<InventorySystem, int> OnDynamicInventoryDisplayRequested;

    public static UnityAction<InventorySystem, int> OnCloseChestRequested;

    protected virtual void Awake()
    {
        SaveLoad.OnLoad += LoadInventory;

        primaryInventorySystem = new InventorySystem(inventorySize);
    }

    protected abstract void LoadInventory(SaveInvData data);

}

[Serializable]
public struct InventorySaveData
{
    public InventorySystem invSystem;
    public Vector3 pos;
    public Quaternion rot;

    public InventorySaveData(InventorySystem _invSystem, Vector3 _pos, Quaternion _rot)
    {
        invSystem = _invSystem;
        pos = _pos;
        rot = _rot;
    }

    public InventorySaveData(InventorySystem _invSystem)
    {
        invSystem = _invSystem;
        pos = Vector3.zero;
        rot = Quaternion.identity;
    }
}