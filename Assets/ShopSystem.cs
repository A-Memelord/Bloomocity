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

    private void Awake()
    {
        instance = this;
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

    public void Start()
    {
        ShopToggle.color = Color.green;
    }

    void Update()
    {
        if (Player != null)
        {
            if (Vector3.Distance(Player.transform.position, NPC.transform.position) < 3f)
            {
                UI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                PlayerCam.instance.sensX = 0f;
                PlayerCam.instance.sensY = 0f;
            }
            else
            {
                UI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                PlayerCam.instance.sensX = 1600f;
                PlayerCam.instance.sensY = 1600f;
            }
        }
        else
        {
            Player = GameObject.FindWithTag("Player");
        }
    }
}
