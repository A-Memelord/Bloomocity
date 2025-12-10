using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop System/Shop Item  List")]
public class ShopItemList : ScriptableObject
{
    [SerializeField] private List<ShopInventoryItem> _items;
    [SerializeField] private int _maxAllowedMoney;
    [SerializeField] private float _sellMarkup;
    [SerializeField] private float _buyMarkup;

    public List<ShopInventoryItem> Items => _items;
    public double MaxAllowedMoney => _maxAllowedMoney;
    public float SellMarkup => _sellMarkup;
    public float BuyMarkup => _buyMarkup;
}

[System.Serializable]
public struct ShopInventoryItem
{
    public InventoryItemData ItemData;
    public int Amount;
}