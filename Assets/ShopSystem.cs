using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    public static ShopSystem instance;

    public GameObject Player;

    public GameObject NPC;

    public GameObject UI;

    public bool Buy = true;

    private void Awake()
    {
        instance = this;
    }

    public void BuyMode(bool buy)
    {
        print(buy);
        Buy = buy;
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
            }
            else
            {
                UI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else
        {
            Player = GameObject.FindWithTag("Player");
        }
    }
}
