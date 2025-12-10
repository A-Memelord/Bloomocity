using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ShopSystem : MonoBehaviour
{
    [SerializeField] private List<ShopSlot> _shopInventory;
    [SerializeField] private double _availableMoney;
    [SerializeField] private float _buyMarkup;
    [SerializeField] private float _sellMarkup;



    public ShopSystem(int size, double money, float buyMarkup, float sellMarkup)
    {
        _availableMoney = money;
        _buyMarkup = buyMarkup;
        _sellMarkup = sellMarkup;

        SetShopSize(size);
    }

    private void SetShopSize(int size)
    {
        _shopInventory = new List<ShopSlot>(size);

        for (int i = 0; i < size; i++)
        {
            _shopInventory.Add(new ShopSlot());
        }
    }

    public void AddToShop(InventoryItemData data, int amount)
    {
        if (ContainsItem(data, out ShopSlot shopSlot))
        {
            shopSlot.AddToStack(amount);
        }

        var freeSlot = GetFreeSlot();
        freeSlot.AssignItem(data, amount);
    }

    private ShopSlot GetFreeSlot()
    {
        var freeSlot = _shopInventory.Find(i => i.ItemData == null);

        if (freeSlot == null)
        {
            freeSlot = new ShopSlot();
            _shopInventory.Add(freeSlot);
        }

        return freeSlot;
    }

    public bool ContainsItem(InventoryItemData itemToAdd, out ShopSlot shopSLot)
    {
        shopSLot = _shopInventory.Find(i => i.ItemData == itemToAdd);
        return shopSLot != null;
    }
}
