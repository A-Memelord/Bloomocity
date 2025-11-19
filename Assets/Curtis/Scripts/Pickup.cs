using Unity.Mathematics;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Transform pickupPoint;
    private Quaternion rotation;

    void Start()
    {
        
    }

    void Update()
    {
        if (transform.parent)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = rotation;
        }
    }

    private void OnMouseDown()
    {
        transform.parent = pickupPoint.transform;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        GetComponent<Rigidbody>().freezeRotation = true;
        GetComponent<BoxCollider>().enabled = false;
        rotation = transform.rotation;
    }

    private void OnMouseUp()
    {
        transform.parent = null;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<Rigidbody>().freezeRotation = false;
    }
}
