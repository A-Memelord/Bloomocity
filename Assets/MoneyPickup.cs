using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(UniqueID))]
public class MoneyPickup : MonoBehaviour
{
    public float PickupRadius = 1f;
    public float attractionRange = 5f;
    public float attractionSpeed = 10f;

    private float _rotSpeed = 250f;

    private SphereCollider myCollider;
    private Rigidbody rb;
    private Transform Player;

    public float amount;

    void Awake()
    {

        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = PickupRadius;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, _rotSpeed * Time.deltaTime, 0f));
    }

    void FixedUpdate()
    {

        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        float distance = Vector3.Distance(transform.position, Player.position);
        if (distance > attractionRange) return;
       
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SaveDataController.Instance.CurrentData.Money += amount + Random.Range(1, 10);
            Destroy(this.gameObject);
        }
    }
}