using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(UniqueID))]
public class ItemPickup : MonoBehaviour
{
    public float PickupRadius = 1f;
    public float attractionRange = 5f;
    public float attractionSpeed = 10f;
    public InventoryItemData ItemData;

    private SphereCollider myCollider;
    private Rigidbody rb;
    private Transform Player;

    [SerializeField] private ItemPickupSaveData itemSaveData;
    private string id;

    void Awake()
    {
        id = GetComponent<UniqueID>().ID;
        SaveLoad.OnLoad += Load;
        itemSaveData = new ItemPickupSaveData(ItemData, transform.position, transform.rotation);

        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = PickupRadius;
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        SaveGameManager.data.activeItems.Add(id, itemSaveData);
    }

    private void Load(SaveInvData data)
    {
        if (data.collectedItems.Contains(id)) Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        if (SaveGameManager.data.activeItems.ContainsKey(id)) SaveGameManager.data.activeItems.Remove(id);
        SaveLoad.OnLoad -= Load;
    }

    void FixedUpdate()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        var inventory = Player.transform.GetComponent<InventoryHolder>();
            float distance = Vector3.Distance(transform.position, Player.position);
            if (distance > attractionRange) return;

        if (inventory.PrimaryInventorySystem.HasFreeSlot(out InventorySlot freeSlot))
        {
            // Speed scales with proximity so the motion is gentle when far and slightly stronger when close
            float t = 1f - Mathf.Clamp01(distance / attractionRange);
            float currentSpeed = Mathf.Lerp(0f, attractionSpeed, t);

            if (rb != null)
            {
                Vector3 newPos = Vector3.MoveTowards(rb.position, Player.position, currentSpeed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
            }
            else
            {
                Vector3 newPos = Vector3.MoveTowards(transform.position, Player.position, currentSpeed * Time.deltaTime);
                transform.position = newPos;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        var inventory = other.transform.GetComponent<PlayerInventoryHolder>();

        if (!inventory) return;

        if (inventory.AddToInventory(ItemData, 1))
        {
            SaveGameManager.data.collectedItems.Add(id);
            Destroy(this.gameObject);
        }
    }
}

[Serializable]
public struct ItemPickupSaveData
{
    public InventoryItemData itemData;
    public Vector3 pos;
    public Quaternion rot;
    public ItemPickupSaveData(InventoryItemData _data, Vector3 _pos, Quaternion _rot)
    {
        itemData = _data;
        pos = _pos;
        rot = _rot;
    }
}