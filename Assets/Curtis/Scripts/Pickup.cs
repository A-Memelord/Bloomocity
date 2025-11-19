using Unity.Mathematics;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Transform pickupPoint;
    private Quaternion rotation;
    public Transform originalPoint;
    private bool positionSet;
    private bool pickedUp;

    void Start()
    {
        originalPoint = pickupPoint;
    }

    void Update()
    {
        if (transform.parent)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = rotation;
        }

        if (pickedUp)
        {
            pickupPoint.transform.Translate(Camera.main.transform.forward * Time.deltaTime * 300 * Input.GetAxis("Mouse ScrollWheel"), Space.World);
        }
    }

    private void OnMouseDown()
    {
        pickedUp = true;
        if (!positionSet)
        {
            positionSet = true;
            pickupPoint.position = transform.position;
        }
        transform.parent = pickupPoint.transform;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        GetComponent<Rigidbody>().freezeRotation = true;
        GetComponent<BoxCollider>().enabled = false;
        //rotation = transform.rotation;
    }

    private void OnMouseUp()
    {
        pickedUp = false;
        positionSet = false;
        pickupPoint.position = originalPoint.position;
        transform.parent = null;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<Rigidbody>().freezeRotation = false;
    }
}
