using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class ItemPickup : MonoBehaviour
{
    public float PickupRadius = 1f;
    public float attractionRange = 5f;
    public float attractionSpeed = 10f;
    public InventoryItemData ItemData;

    private SphereCollider myCollider;
    private Rigidbody rb;
    private Transform Player;

    void Awake()
    {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = PickupRadius;
        rb = GetComponent<Rigidbody>();
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

        if (inventory.InventorySystem.HasFreeSlot(out InventorySlot freeSlot))
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
        var inventory = other.transform.GetComponent<InventoryHolder>();

        if (!inventory) return;

        if (inventory.InventorySystem.AddToInventory(ItemData, 1))
        {
            Destroy(this.gameObject);
        }
    }
}
