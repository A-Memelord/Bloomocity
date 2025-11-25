using System.Xml.Serialization;
using UnityEngine;

public class NPCSystem : MonoBehaviour
{
    public bool playerDetection;

    void Start()
    {
        
    }

    void Update()
    {
        if (playerDetection && Input.GetKeyDown(KeyCode.F))
        {
            print("Dialogue Started");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            playerDetection = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerDetection = false;
    }
}
