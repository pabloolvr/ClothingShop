using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _shopCameraPrefab;
    [SerializeField] private GameObject _shopItemPanelPrefab;

    [Header("References")]
    [SerializeField] private RawImage _playerPreviewDisplay;
    [SerializeField] private TextMeshProUGUI _playerGoldField;
    [SerializeField] private TextMeshProUGUI _shopGoldField;
    [SerializeField] private ScrollRect _shopInventoryScrollView;
    [SerializeField] private ScrollRect _playerInventoryScrollView;

    [SerializeField] private Button _closeBtn;
    
    [Header("Item Information References")]
    [SerializeField] private GameObject _itemInfoPanel;
    [SerializeField] private TextMeshProUGUI _selectedItemNameField;
    [SerializeField] private TextMeshProUGUI _selectedItemDescriptionField;
    [SerializeField] private Button _buyBtn;
    [SerializeField] private TextMeshProUGUI _buyBtnField;
    [SerializeField] private Button _sellBtn;
    [SerializeField] private TextMeshProUGUI _sellBtnField;
    [SerializeField] private Button _equipBtn;
    [SerializeField] private Button _unequipBtn;

    [Header("Settings")]
    [SerializeField] private float _actionDelay = 1f;

    [Header("Input")]
    [SerializeField] private KeyCode _buySellKey = KeyCode.Q;
    [SerializeField] private KeyCode _EquipUnequipKey = KeyCode.E;

    public ShopItemPanel SelectedItemPanel
    {
        get
        {
            return _selectedItemPanel;
        }
        set
        {
            if (_selectedItemPanel != null)
            {
                _selectedItemPanel.SetSelected(false);
            }

            _selectedItemPanel = value;

            if (_selectedItemPanel != null)
            {
                _itemInfoPanel.SetActive(true);
                _selectedItemPanel.SetSelected(true);

                if (_selectedItemPanel.Item.ItemData is WearableItem)
                {
                    _player.PlayerInventory.PreviewItem(_selectedItemPanel.Item.ItemData as WearableItem);
                }              
            }
            else
            {
                _itemInfoPanel.SetActive(false);
                _player.PlayerInventory.EmptyPreviewSlot(out WearableItem _);
            }          
        }
    }

    private ShopItemPanel _selectedItemPanel = null;
    private GameObject _shopCameraInstance;
    private PlayerController _player;
    private Shop _shop;
    private Dictionary<ItemInstance, ShopItemPanel> _shopItemPanels = new Dictionary<ItemInstance, ShopItemPanel>();
    private Dictionary<ItemInstance, ShopItemPanel> _playerItemPanels = new Dictionary<ItemInstance, ShopItemPanel>();

    private void Start()
    {
        _buyBtn.onClick.AddListener(PlayerBuyItem);
        _sellBtn.onClick.AddListener(PlayerSellItem);
        _equipBtn.onClick.AddListener(PlayerEquipItem);
        _unequipBtn.onClick.AddListener(PlayerUnequipItem);
        _closeBtn.onClick.AddListener(CloseShop);
    }

    public void Initialize(PlayerController player, Shop shop)
    {
        _player = player;
        _shop = shop;
        _shopCameraInstance = Instantiate(_shopCameraPrefab, _player.transform);

        _playerGoldField.text = "<sprite index=0> " + _player.PlayerInventory.CurGold.ToString();
        _shopGoldField.text = "<sprite index=0> " + _shop.CurGold.ToString();

        FillShopInventory();
        FillPlayerInventory();
    }

    private void Update()
    {
        if (Input.GetKeyDown(_buySellKey))
        {
            if (SelectedItemPanel == null) return;

            if (_buyBtn.gameObject.activeSelf)
            {
                PlayerBuyItem();
            }
            else if (_sellBtn.gameObject.activeSelf)
            {
                PlayerSellItem();
            }
        }

        if (Input.GetKeyDown(_EquipUnequipKey))
        {
            if (SelectedItemPanel == null) return;

            if (SelectedItemPanel.Item.ItemData is WearableItem)
            {
                if (_equipBtn.gameObject.activeSelf)
                {
                    PlayerUnequipItem();
                }
                else if (_unequipBtn.gameObject.activeSelf)
                {
                    PlayerEquipItem();
                }
            }
        }
    }

    public void CloseItemInfoPanel()
    {
        SelectedItemPanel = null;
    }

    private void PlayerUnequipItem()
    {
        if (SelectedItemPanel == null) return;

        WearableItem item = SelectedItemPanel.Item.ItemData as WearableItem;

        if (_player.PlayerInventory.HasItemEquipped(item))
        {
            _player.PlayerInventory.EmptySlot(item.Slot, out _);
            SelectedItemPanel.SetEquipped(false);
            OnItemSelected(SelectedItemPanel, false);
        }
    }

    private void PlayerEquipItem()
    {
        if (SelectedItemPanel == null) return;

        WearableItem item = SelectedItemPanel.Item.ItemData as WearableItem;

        if (_player.PlayerInventory.EmptySlot(item.Slot, out WearableItem removedItem))
        {
            foreach (ItemInstance itemInstance in _playerItemPanels.Keys)
            {
                if (itemInstance.ItemData == removedItem)
                {
                    _playerItemPanels[itemInstance].SetEquipped(false);
                }
            }           
        }

        _player.PlayerInventory.EquipItem(item);
        SelectedItemPanel.SetEquipped(true);
        OnItemSelected(SelectedItemPanel, false);
    }

    private void PlayerSellItem()
    {
        if (SelectedItemPanel == null) return;

        ItemInstance itemInstance = null;

        SelectedItemPanel.RemoveQuantity(1, out int removedQty);
        if (removedQty == 1)
        {
            //_shop.RemoveFromStock(SelectedItemPanel.Item, removedQty);
            //_player.PlayerInventory.TryAddItem(SelectedItemPanel.Item.ItemData, out itemInstance);            
            _shop.TryAddStock(SelectedItemPanel.Item.ItemData, out itemInstance);
            _player.PlayerInventory.RemoveFromInventory(SelectedItemPanel.Item, removedQty);
            PlayerUnequipItem();
        }
        else
        {
            return;
        }

        if (SelectedItemPanel.Item.ItemData.Stackable)
        {
            if (itemInstance != null)
            {
                SpawnItemPanel(itemInstance, true);
            }
            else
            {
                foreach (ItemInstance item in _shopItemPanels.Keys)
                {
                    if (item.ItemData == SelectedItemPanel.Item.ItemData)
                    {
                        _shopItemPanels[item].AddQuantity(1, out _);
                    }
                }
            }
        }
        else
        {
            SpawnItemPanel(itemInstance, true);
        }

        // update gold qty of shop and player
        int sellPrice = _shop.GetBuyPrice(SelectedItemPanel.Item.ItemData);
        _shop.CurGold -= sellPrice;
        _player.PlayerInventory.CurGold += sellPrice;

        _playerGoldField.text = "<sprite index=0> " + _player.PlayerInventory.CurGold.ToString();
        _shopGoldField.text = "<sprite index=0> " + _shop.CurGold.ToString();

        SelectedItemPanel = null;
    }

    private void PlayerBuyItem()
    {
        if (SelectedItemPanel == null) return;

        ItemInstance itemInstance = null;

        SelectedItemPanel.RemoveQuantity(1, out int removedQty);
        if (removedQty == 1)
        {
            _shop.RemoveFromStock(SelectedItemPanel.Item, removedQty);
            _player.PlayerInventory.TryAddItem(SelectedItemPanel.Item.ItemData, out itemInstance);
        }
        else
        {
            return;
        }

        if (SelectedItemPanel.Item.ItemData.Stackable)
        {
            if (itemInstance != null)
            {
                SpawnItemPanel(itemInstance, false);
            }
            else
            {
                foreach (ItemInstance item in _playerItemPanels.Keys)
                {
                    if (item.ItemData == SelectedItemPanel.Item.ItemData)
                    {
                        _playerItemPanels[item].AddQuantity(1, out _);
                    }
                }
            }
        }
        else
        {
            SpawnItemPanel(itemInstance, false);
        }

        // update gold qty of shop and player
        _shop.CurGold += SelectedItemPanel.Item.Price;
        _player.PlayerInventory.CurGold -= SelectedItemPanel.Item.Price;

        _playerGoldField.text = "<sprite index=0> " + _player.PlayerInventory.CurGold.ToString();
        _shopGoldField.text = "<sprite index=0> " + _shop.CurGold.ToString();

        SelectedItemPanel = null;
    }

    /// <summary>
    /// Instantiate a shop item panel for each item in the shop inventory.
    /// </summary>
    private void FillShopInventory()
    {
        foreach (ItemInstance item in _shop.ItemStock)
        {           
            SpawnItemPanel(item, true);
        }
    }

    /// <summary>
    /// Instantiate a shop item panel and Shop Item for each item in the player inventory.
    /// </summary>
    private void FillPlayerInventory()
    {
        foreach (ItemInstance item in _player.PlayerInventory.ItemInventory)
        {
            //ItemInstance shopItem = new ItemInstance(item.ItemData, item.Quantity, _shop.GetBuyPrice(item.ItemData));
            SpawnItemPanel(item, false);
        }
    }

    /// <summary>
    /// Tries to spawn item panel. 
    /// If item is stackable, it will try to find the item panel and add the quantity, withou spawning a new one.
    /// </summary>
    /// <param name="itemInstance"></param>
    /// <param name="fromShop"></param>
    /// <returns></returns>
    private bool SpawnItemPanel(ItemInstance itemInstance, bool fromShop)
    {
        ShopItemPanel itemPanel;
        Dictionary<ItemInstance, ShopItemPanel> itemPanels;
        ScrollRect scrollView;

        if (fromShop)
        {
            itemPanels = _shopItemPanels;
            scrollView = _shopInventoryScrollView;
        }
        else
        {
            itemPanels = _playerItemPanels;
            scrollView = _playerInventoryScrollView;
        }

        if (itemInstance.ItemData.Stackable)
        {
            foreach (ItemInstance item in itemPanels.Keys)
            {
                if (item.ItemData == itemInstance.ItemData)
                {
                    //itemPanels[item].Quantity += itemInstance.Quantity;
                    itemPanels[item].AddQuantity(itemInstance.Quantity, out _);
                    return false;
                }
            }
        }

        itemPanel = Instantiate(_shopItemPanelPrefab, scrollView.content).GetComponent<ShopItemPanel>();
        itemPanel.Initialize(itemInstance, this);
        itemPanel.PanelBtn.onClick.AddListener(() => OnItemSelected(itemPanel, fromShop));
        itemPanel.SetEquipped(_player.PlayerInventory.HasItemEquipped(itemInstance.ItemData as WearableItem));
        itemPanels.Add(itemInstance, itemPanel);
        return true;
    }

    private void OnItemSelected(ShopItemPanel itemPanel, bool fromShop)
    {
        SelectedItemPanel = itemPanel;

        _selectedItemNameField.text = itemPanel.Item.ItemData.Name;
        _selectedItemDescriptionField.text = itemPanel.Item.ItemData.Description;

        // check if it is on player inventory or shop inventory
        if (fromShop)
        {
            _buyBtn.gameObject.SetActive(true);
            
            _buyBtnField.text = $"[{_buySellKey}] Buy <sprite index=0> {itemPanel.Item.Price}";

            if (_player.PlayerInventory.CurGold >= itemPanel.Item.Price && !_player.PlayerInventory.IsInventoryFull)
            {
                _buyBtn.GetComponent<CanvasGroup>().alpha = 1f;
            }
            else
            {
                _buyBtn.GetComponent<CanvasGroup>().alpha = .5f;
            }

            _sellBtn.gameObject.SetActive(false);
            _equipBtn.gameObject.SetActive(false);
            _unequipBtn.gameObject.SetActive(false);
        }
        else
        {
            _buyBtn.gameObject.SetActive(false);
            _sellBtn.gameObject.SetActive(true);

            int sellPrice = _shop.GetBuyPrice(itemPanel.Item.ItemData);
            _sellBtnField.text = $"[{_buySellKey}] Sell <sprite index=0> {sellPrice}";

            if (_shop.CurGold >= sellPrice)
            {
                _buyBtn.GetComponent<CanvasGroup>().alpha = 1f;
            }
            else
            {
                _buyBtn.GetComponent<CanvasGroup>().alpha = .5f;
            }

            if (itemPanel.Item.ItemData is not WearableItem)
            {
                _equipBtn.gameObject.SetActive(false);
                _unequipBtn.gameObject.SetActive(false);
            }
            else if (_player.PlayerInventory.HasItemEquipped(itemPanel.Item.ItemData as WearableItem))
            {
                _equipBtn.gameObject.SetActive(false);
                _unequipBtn.gameObject.SetActive(true);
            }
            else
            {
                _equipBtn.gameObject.SetActive(true);
                _unequipBtn.gameObject.SetActive(false);
            }
        }
    }

    public void OnShopItemPanelDestroyed(ShopItemPanel shopItemPanel)
    {
        _shopItemPanels.Remove(shopItemPanel.Item);
        _playerItemPanels.Remove(shopItemPanel.Item);
    }

    public void CloseShop()
    {
        _player.PlayerInventory.EmptyPreviewSlot(out WearableItem _);
        GameManager.Instance.UIManager.CloseShopInterface();
        Destroy(_shopCameraInstance);
        Destroy(gameObject);
    }
}
