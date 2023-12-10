using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemPanel : MonoBehaviour
{
    public Button PanelBtn => _panelBtn;
    public ShopItem ShopItem { get; private set; }
    public int Quantity
    {
        get 
        { 
            return ShopItem.Quantity; 
        }
        set 
        { 
            ShopItem.AddQuantity(value - ShopItem.Quantity);

            if (ShopItem.Quantity <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                _itemQuantityField.text = "x" + ShopItem.Quantity;
            }
        }
    }

    [SerializeField] private Button _panelBtn;

    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemNameField;
    [SerializeField] private TextMeshProUGUI _itemQuantityField;

    private ShopUIManager _shopUIManager;

    private void OnDestroy()
    {
        if (_shopUIManager)
            _shopUIManager.OnShopItemPanelDestroyed(this);
    }

    public void Initialize(ShopItem item, ShopUIManager shopUIManager)
    {
        ShopItem = item;
        _shopUIManager = shopUIManager;

        _itemIcon.sprite = item.ItemData.Icon;
        _itemNameField.text = item.ItemData.Name;
        _itemQuantityField.text = "x" + ShopItem.Quantity;
    }
}
