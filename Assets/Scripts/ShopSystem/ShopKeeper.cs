using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class ShopKeeper : MonoBehaviour, IInteractable
{
    [SerializeField] private ShopItemList _shopItemsHeld;
    [SerializeField, HideInInspector] private ShopSystem _shopSystem;

    public GameObject shopUI;
    private Transform _player;
    public TMP_Text shopToggle;
    public bool buyBool = true;
    private bool isInRange = false;

    public static UnityAction<ShopSystem, PlayerInventoryHolder> OnShopWindowRequested;

    private void Start()
    {
        shopToggle.color = Color.green;
        shopUI.SetActive(false);
    }

    private void Awake()
    {
        print("Working");
        _shopSystem =new ShopSystem(_shopItemsHeld.Items.Count, _shopItemsHeld.MaxAllowedMoney, _shopItemsHeld.BuyMarkup, _shopItemsHeld.SellMarkup);
        print("Working1");
        //foreach (var item in _shopItemsHeld.Items)
        //{
        //    print(item.ItemData.name);
        //    _shopSystem.AddToShop(item.ItemData, item.Amount);
        //}

        foreach (var item in _shopItemsHeld.Items)
        {
            if (item.ItemData == null)
            {
                Debug.LogError("❌ Shop Item is NULL.");
                continue;
            }

            if (item.ItemData == null)
            {
                Debug.LogError("❌ ItemData is NULL on shop entry: " + item);
                continue;
            }

            Debug.Log("✔ Found item: " + item.ItemData.name);

            _shopSystem.AddToShop(item.ItemData, item.Amount);
        }



        print("Working2");

        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        print("Working3");
    }

    private void Update()
    {
        print("Working4");
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
            return;
        }

        float dist = Vector3.Distance(_player.position, this.transform.position);

        // ENTER RANGE
        if (!isInRange && dist < 3f)
        {
            isInRange = true;          // mark as entered
            shopUI.SetActive(true);        // show UI
            if (PlayerCam.instance != null)
                PlayerCam.instance.CameraLock(false);
        }

        // EXIT RANGE
        if (isInRange && dist > 3.25f)
        {
            isInRange = false;         // mark as exited
            shopUI.SetActive(false);       // hide UI
            if (PlayerCam.instance != null)
                PlayerCam.instance.CameraLock(true);
        }
    }

    public void BuyMode(bool buy)
    {
        buyBool = buy;
        print(buyBool);

        if (buyBool)
        {
            shopToggle.text = "Buy Mode";
            shopToggle.color = Color.green;
        }
        else
        {
            shopToggle.text = "Sell Mode";
            shopToggle.color = Color.red;
        }

        PlantShop.instance.ChangeBool(buyBool);
    }

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }
    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        var playerInv = interactor.GetComponent<PlayerInventoryHolder>();

        if (playerInv != null )
        {
            OnShopWindowRequested?.Invoke(_shopSystem, playerInv);
            interactSuccessful = true;
        }
        else
        {
            interactSuccessful = false;
            print("Player Inventory Not Found");
        }
    }

    public void EndInteraction()
    {

    }
}
