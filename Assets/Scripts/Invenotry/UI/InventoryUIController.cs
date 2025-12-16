using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIController : MonoBehaviour
{
    public DynamicInventoryDisplay inventoryPanel;       // chest UI
    public DynamicInventoryDisplay playerBackpackPanel;  // player backpack UI

    private void Awake()
    {
        inventoryPanel.gameObject.SetActive(false);
        playerBackpackPanel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
        PlayerInventoryHolder.OnPlayerInventoryDisplayRequested += DisplayPlayerInventory;

        InventoryHolder.OnCloseChestRequested += CloseInventory;
    }

    private void OnDisable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
        PlayerInventoryHolder.OnPlayerInventoryDisplayRequested -= DisplayPlayerInventory;

        InventoryHolder.OnCloseChestRequested -= CloseInventory;
    }

    void Update()
    {
        // Escape closes everything and unlocks camera
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            bool closedAny = false;

            if (inventoryPanel.gameObject.activeSelf)
            {
                inventoryPanel.gameObject.SetActive(false);
                closedAny = true;
            }

            if (playerBackpackPanel.gameObject.activeSelf)
            {
                playerBackpackPanel.gameObject.SetActive(false);
                closedAny = true;
            }

            if (closedAny)
                PlayerCam.instance.CameraLock(true);
        }
    }

    // This toggles the chest UI. The event provides the InventorySystem + offset.
    void DisplayInventory(InventorySystem invToDisplay, int offset)
    {
        // If already open, close it
        if (inventoryPanel.gameObject.activeSelf)
        {
            inventoryPanel.gameObject.SetActive(false);
            PlayerCam.instance.CameraLock(true);
            return;
        }

        // Otherwise open and refresh with the provided inventory
        inventoryPanel.gameObject.SetActive(true);
        inventoryPanel.RefreshDynamicInventory(invToDisplay, offset);
        PlayerCam.instance.CameraLock(false);
    }

    // This toggles the player backpack UI. The event provides the InventorySystem + offset.
    void DisplayPlayerInventory(InventorySystem invToDisplay, int offset)
    {
        // If already open, close it
        if (playerBackpackPanel.gameObject.activeSelf)
        {
            playerBackpackPanel.gameObject.SetActive(false);
            PlayerCam.instance.CameraLock(true);
            return;
        }

        // Otherwise open and refresh with the provided inventory
        playerBackpackPanel.gameObject.SetActive(true);
        playerBackpackPanel.RefreshDynamicInventory(invToDisplay, offset);
        PlayerCam.instance.CameraLock(false);
    }

    // Called when a chest requests to be closed (e.g. player walked away).
    // Signature matches UnityAction<InventorySystem,int> so we accept those parameters even if unused.
    void CloseInventory(InventorySystem inv, int offset)
    {
        //if (inventoryPanel.gameObject.activeSelf)
        //    inventoryPanel.gameObject.SetActive(false);

        if (playerBackpackPanel.gameObject.activeSelf)
            playerBackpackPanel.gameObject.SetActive(false);

        PlayerCam.instance.CameraLock(true);
    }
}
