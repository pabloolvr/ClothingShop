using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an item instance of any item data.
/// </summary>
[Serializable]
public class ItemInstance
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

    public ItemInstance(ItemData itemData, int quantity, int price)
    {
        ItemData = itemData;
        Quantity = quantity;
        Price = price;
    }

    public void AddQuantity(int quantity)
    {
        if (!ItemData.Stackable) return;

        Quantity += quantity;
    }

    public void RemoveQuantity(int quantity)
    {
        Quantity -= quantity;
    }
}

//[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Game Item")]
public class ItemData : ScriptableObject
{
    public string Name => _itemName;
    public string Description => _itemDescription;
    public Sprite Icon => _itemIcon;
    public int ItemPrice => _itemPrice;
    public bool Stackable => _stackable;

    [Header("General Item Data")]
    [SerializeField] private string _itemName;
    [SerializeField] private string _itemDescription;
    [SerializeField] private Sprite _itemIcon;
    [SerializeField] private int _itemPrice;
    [SerializeField] private bool _stackable;
}
