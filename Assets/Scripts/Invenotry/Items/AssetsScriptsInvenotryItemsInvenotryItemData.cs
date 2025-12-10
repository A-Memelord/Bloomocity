using UnityEngine;

[CreateAssetMenu(fileName = "InvenotryItemData", menuName = "Scriptable Objects/InvenotryItemData")]
public class InventoryItemData : ScriptableObject
{
    public int ID = -1;
    public int maxStackSize;
    public Sprite icon;
    public string itemName;
    [TextArea(4, 4)] public string description;
    public double Cost;
    public GameObject ItemPrefab; // Prefab Of The Item Just On The Ground So You Can Pick It Up
    public GameObject PlacedPrefab; // Prefab Of The Item Placed On The Ground

    public void UseItem()
    {
        Debug.Log($"Using {itemName}");
    }
}