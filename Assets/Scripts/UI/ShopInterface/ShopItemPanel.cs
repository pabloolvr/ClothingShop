using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemPanel : MonoBehaviour
{
    public Button PanelBtn => _panelBtn;
    public ItemInstance Item { get; private set; }
    public int Quantity
    {
        get 
        { 
            return Item.Quantity; 
        }
    }

    [SerializeField] private Button _panelBtn;

    [SerializeField] private Image _itemIcon;
    [SerializeField] private Image _backgroundIcon;
    [SerializeField] private GameObject _equippedPanel;
    [SerializeField] private TextMeshProUGUI _itemNameField;
    [SerializeField] private TextMeshProUGUI _itemQuantityField;

    private ShopUIManager _shopUIManager;

    private void OnDestroy()
    {
        if (_shopUIManager)
            _shopUIManager.OnShopItemPanelDestroyed(this);
    }

    public void Initialize(ItemInstance item, ShopUIManager shopUIManager)
    {
        Item = item;
        _shopUIManager = shopUIManager;

        _itemIcon.sprite = item.ItemData.Icon;
        _itemIcon.color = item.ItemData.Color;
        _itemNameField.text = item.ItemData.Name;
        _itemQuantityField.text = "x" + Item.Quantity;
    }

    public void SetSelected(bool selected)
    {
        _backgroundIcon.color = selected ? Color.green : Color.white;
    }

    public void SetEquipped(bool equipped)
    {
        _equippedPanel.SetActive(equipped);
    }

    public void AddQuantity(int value, out int addedValue)
    {
        int oldVal = Item.Quantity;
        Item.AddQuantity(value);
        addedValue = Item.Quantity - oldVal;

        if (Item.Quantity <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            _itemQuantityField.text = "x" + Item.Quantity;
        }
    }

    public void RemoveQuantity(int value, out int removedValue)
    {
        int oldVal = Item.Quantity;
        Item.RemoveQuantity(value);

        if (Item.Quantity < 0)
        {
            removedValue = oldVal;
        }
        else
        {
            removedValue = value;
        }

        if (Item.Quantity <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            _itemQuantityField.text = "x" + Item.Quantity;
        }
    }
}
