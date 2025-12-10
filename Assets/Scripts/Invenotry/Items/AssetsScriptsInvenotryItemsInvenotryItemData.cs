using Unity.VisualScripting.FullSerializer;
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

    private void Start()
    {
        //SeedPlacing = GameObject.FindGameObjectWithTag("Player").transform.Find("SeedPlacing").gameObject;
    }

    /* PSEUDOCODE / PLAN (detailed)
     - When UseItem is called with a spawn Transform:
       1. Log which item is being used.
       2. If PlacedPrefab is null -> nothing to place, exit.
       3. Try to find the parent container GameObject named "[Plants]".
          - If found, use its Transform as the parent for the instantiated prefab.
          - If not found, instantiate without a parent.
       4. Use an Instantiate overload that accepts (original, position, rotation, parentTransform)
          to avoid type-mismatch errors:
            - position -> spawn.position
            - rotation -> spawn.rotation (preserve spawn orientation) or Quaternion.identity
            - parent -> plantsTransform (or omit if null)
       5. Guard against null references when calling GameObject.Find.
    */

    public void UseItem(Transform spawn, PlayerInventoryHolder inventory)
    {
        Debug.Log(inventory);
        Debug.Log(spawn.position);
        Debug.Log($"Using {itemName}");
        if (this.PlacedPrefab == null || spawn == null)
        {
            return;
        }

        // Find the parent container for placed items
        GameObject plantsGO = GameObject.Find("[Plants]");
        Transform plantsParent = plantsGO != null ? plantsGO.transform : null;

        // Use the overload Instantiate(original, position, rotation, parent)
        if (plantsParent != null)
        {
            Instantiate(this.PlacedPrefab, spawn.position, spawn.rotation, plantsParent);
        }
        else
        {
            // Fallback if parent not found: instantiate at position with spawn rotation
            Instantiate(this.PlacedPrefab, spawn.position, spawn.rotation);
        }
        inventory.AddToInventory(this, -1);
    }
}