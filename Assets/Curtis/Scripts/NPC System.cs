using System.Xml.Serialization;
using UnityEngine;

public class NPCSystem : MonoBehaviour
{
    public AudioSource Meow;
    public bool playerDetection;

    public static NPCSystem current;

    void Start()
    {
        
    }

    void Update()
    {
        if (playerDetection && Input.GetKeyDown(KeyCode.F))
        {
            Meow.Play();
            print("Dialogue Started");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            playerDetection = true;
            current = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerDetection = false;
        current = null;
    }

}
