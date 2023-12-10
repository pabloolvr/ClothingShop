using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopItem
{
    public ItemData ItemData 
    { 
        get { return _itemData; }
        private set { _itemData = value; }
    }
    public int Quantity
    {
        get { return _quantity; }
        private set { _quantity = value; }
    }
    public int Price
    {
        get { return _price; }
        private set { _price = value; }
    }

    [SerializeField] private ItemData _itemData;
    [SerializeField] private int _quantity;
    [SerializeField] private int _price;

    public ShopItem(ItemData itemData, int quantity, int price)
    {
        ItemData = itemData;
        Quantity = quantity;
    }

    public void AddQuantity(int quantity)
    {
        Quantity += quantity;
    }

    public void RemoveQuantity(int quantity)
    {
        Quantity -= quantity;
    }
}

public class Shop : MonoBehaviour, IInteractable
{
    public Vector2 Position => transform.position;
    public Dictionary<ItemData, ShopItem> ItemStock => _itemStock;

    public int CurGold { get; set; }

    [SerializeField] private List<ShopItem> _stock;
    
    private Dictionary<ItemData, ShopItem> _itemStock = new Dictionary<ItemData, ShopItem>();

    private void Start()
    {
        CurGold = 1000;

        foreach (var item in _stock)
        {
            _itemStock.Add(item.ItemData, item);
        }
    }

    public void Interact(PlayerController player)
    {
        GameManager.Instance.UIManager.OpenShopInterface(player, this);
    }

    public void AddStock(ShopItem shopItem)
    {
        if (_itemStock.ContainsKey(shopItem.ItemData))
        {
            _itemStock[shopItem.ItemData].AddQuantity(shopItem.Quantity);
        }
        else
        {
            _itemStock.Add(shopItem.ItemData, shopItem);
        }
    }
}
