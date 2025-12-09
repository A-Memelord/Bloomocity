using TMPro;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    public static ShopSystem instance;

    public GameObject Player;
    public GameObject NPC;
    public GameObject UI;

    public TMP_Text ShopToggle;

    public bool buyBool = true;

    private bool isInRange = false; // 🔥 new flag

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ShopToggle.color = Color.green;
        UI.SetActive(false);
    }

    private void Update()
    {
        if (Player == null)
        {
            Player = GameObject.FindWithTag("Player");
            return;
        }

        float dist = Vector3.Distance(Player.transform.position, NPC.transform.position);

        // ENTER RANGE
        if (!isInRange && dist < 3f)
        {
            isInRange = true;          // mark as entered
            UI.SetActive(true);        // show UI
            PlayerCam.instance.CameraLock(false);
        }

        // EXIT RANGE
        if (isInRange && dist > 3.25f)
        {
            isInRange = false;         // mark as exited
            UI.SetActive(false);       // hide UI
            PlayerCam.instance.CameraLock(true);
        }
    }

    public void BuyMode(bool buy)
    {
        buyBool = buy;

        if (buyBool)
        {
            ShopToggle.text = "Buy Mode";
            ShopToggle.color = Color.green;
        }
        else
        {
            ShopToggle.text = "Sell Mode";
            ShopToggle.color = Color.red;
        }
    }
}
