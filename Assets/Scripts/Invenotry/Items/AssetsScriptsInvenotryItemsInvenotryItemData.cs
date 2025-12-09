using UnityEngine;

[CreateAssetMenu(fileName = "InvenotryItemData", menuName = "Scriptable Objects/InvenotryItemData")]
public class InventoryItemData : ScriptableObject
{
    public int ID = -1;
    public Sprite icon;
    public string itemName;
    [TextArea(4, 4)] public string description;
    public int maxStackSize;
}