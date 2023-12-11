using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopItem
{
    public ItemData ItemData => _itemData;
    public int Quantity => _quantity;
    public int Price => _useLocalPrice ? _price : _itemData.ItemPrice;

    [SerializeField] private ItemData _itemData;
    [SerializeField] private int _quantity;
    [SerializeField] private bool _useLocalPrice;
    [SerializeField] private int _price;
}

public class Shop : MonoBehaviour, IInteractable
{
    public Vector2 Position => transform.position;
    public List<ItemInstance> ItemStock => _curStock;

    public int CurGold { get; set; }

    [Header("Settings")]
    [SerializeField] private int _defaultSellPrice = 1;
    [SerializeField, Range(0, 1)] private float _sellPriceMultiplier = .4f;
    [SerializeField] private List<ShopItem> _baseStock;
    
    private List<ItemInstance> _curStock = new List<ItemInstance>();

    private void Start()
    {
        CurGold = 1000;

        foreach (ShopItem shopItem in _baseStock)
        {
            _curStock.Add(new ItemInstance(shopItem.ItemData, shopItem.Quantity, shopItem.Price));
        }
    }

    /// <summary>
    /// If shop is interested in the item, returns the price of the item.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int GetBuyPrice(ItemData item)
    {
        foreach (ShopItem shopItem in _baseStock)
        {
            if (shopItem.ItemData == item)
            {
                return (int)(shopItem.Price * _sellPriceMultiplier);
            }
        }

        return (int)(item.ItemPrice * _sellPriceMultiplier);
    }

    public void Interact(PlayerController player)
    {
        GameManager.Instance.UIManager.OpenShopInterface(player, this);
    }

    public bool TryAddStock(ItemData itemData, out ItemInstance itemInstance)
    {
        itemInstance = null;

        if (!itemData.Stackable)
        {
            itemInstance = new ItemInstance(itemData, 1, itemData.ItemPrice);
            _curStock.Add(itemInstance);
        }
        else
        {
            foreach (ItemInstance item in _curStock)
            {
                if (item.ItemData == itemData)
                {
                    item.AddQuantity(1);
                    return true;
                }
            }

            itemInstance = new ItemInstance(itemData, 1, itemData.ItemPrice);
            _curStock.Add(itemInstance);
        }

        return true;
        //if (_curStock.Contains(itemInstance)) return;

        //if (!itemInstance.ItemData.Stackable)
        //{
        //    _curStock.Add(itemInstance);
        //}
        //else
        //{
        //    foreach (ItemInstance item in _curStock)
        //    {
        //        if (item.ItemData == itemInstance.ItemData)
        //        {
        //            item.AddQuantity(quantity);
        //            return;
        //        }
        //    }

        //    _curStock.Add(itemInstance);
        //}
    }

    public bool RemoveFromStock(ItemInstance itemInstance, int quantity)
    {
        if (_curStock.Contains(itemInstance))
        {
            if (!itemInstance.ItemData.Stackable)
            {
                _curStock.Remove(itemInstance);
                return true;
            }
            else if (itemInstance.Quantity >= quantity)
            {
                itemInstance.RemoveQuantity(quantity);
                return true;
            }
            else
            {
                _curStock.Remove(itemInstance);
                return true;
            }
        }

        return false;
    }
}
