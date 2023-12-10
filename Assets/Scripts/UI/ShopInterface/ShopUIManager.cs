using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

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
    [SerializeField] private int _defaultSellPrice = 1;
    [SerializeField, Range(0, 1)] private float _sellPriceMultiplier = .4f;

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
            _selectedItemPanel = value;
            _itemInfoPanel.SetActive(_selectedItemPanel != null);
        }
    }

    private ShopItemPanel _selectedItemPanel = null;
    private GameObject _shopCameraInstance;
    private PlayerController _player;
    private Shop _shop;
    private Dictionary<ItemData, ShopItemPanel> _shopItemPanels = new Dictionary<ItemData, ShopItemPanel>();
    private Dictionary<ItemData, ShopItemPanel> _playerItemPanels = new Dictionary<ItemData, ShopItemPanel>();

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

            if (SelectedItemPanel.ShopItem.ItemData is WearableItem)
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

    private void PlayerUnequipItem()
    {
        if (SelectedItemPanel == null) return;

        WearableItem item = SelectedItemPanel.ShopItem.ItemData as WearableItem;

        if (_player.PlayerInventory.HasItemEquipped(item))
        {
            _player.PlayerInventory.EmptySlot(item.Slot, out _);
        }
    }

    private void PlayerEquipItem()
    {
        if (SelectedItemPanel == null) return;

        _player.PlayerInventory.EquipItem(SelectedItemPanel.ShopItem.ItemData as WearableItem);
    }

    private void PlayerSellItem()
    {
        if (SelectedItemPanel == null) return;

        //SelectedItemPanel.ShopItem.RemoveQuantity(1);

        //if (_shopItemPanels.ContainsKey(SelectedItemPanel.ShopItem.ItemData))
        //{
        //    _shopItemPanels[SelectedItemPanel.ShopItem.ItemData].AddQuantity(1);
        //}
        //else
        //{
        //    _shopItemPanels.Add(SelectedItemPanel.ShopItem.ItemData, SelectedItemPanel.ShopItem);
        //}

        _playerGoldField.text = "<sprite index=0> " + _player.PlayerInventory.CurGold.ToString();
        _shopGoldField.text = "<sprite index=0> " + _shop.CurGold.ToString();
    }

    private void PlayerBuyItem()
    {
        if (SelectedItemPanel == null) return;
        if (!_player.PlayerInventory.TryAddItem(SelectedItemPanel.ShopItem.ItemData, 1)) return;

        _shop.AddStock(SelectedItemPanel.ShopItem);

        // update item panel quantity on shop side
        SelectedItemPanel.Quantity -= 1;

        // update item panel quantity on player side
        if (_playerItemPanels.ContainsKey(SelectedItemPanel.ShopItem.ItemData))
        {
            _playerItemPanels[SelectedItemPanel.ShopItem.ItemData].Quantity += 1;
            return;
        }
        else
        {
            SpawnItemPanel(SelectedItemPanel.ShopItem, false);
        }

        // update gold qty of shop and player
        _shop.CurGold += SelectedItemPanel.ShopItem.Price;
        _player.PlayerInventory.CurGold -= SelectedItemPanel.ShopItem.Price;

        _playerGoldField.text = "<sprite index=0> " + _player.PlayerInventory.CurGold.ToString();
        _shopGoldField.text = "<sprite index=0> " + _shop.CurGold.ToString();
    }

    /// <summary>
    /// Instantiate a shop item panel for each item in the shop inventory.
    /// </summary>
    private void FillShopInventory()
    {
        foreach (var item in _shop.ItemStock.Values)
        {           
            SpawnItemPanel(item, true);
        }
    }

    /// <summary>
    /// Instantiate a shop item panel and Shop Item for each item in the player inventory.
    /// </summary>
    private void FillPlayerInventory()
    {
        foreach (PlayerIventoryItem item in _player.PlayerInventory.Inventory)
        {
            if (_shopItemPanels.ContainsKey(item.ItemData))
            {
                ShopItem shopItem = new ShopItem(item.ItemData, item.Quantity, _shopItemPanels[item.ItemData].ShopItem.Price);
                SpawnItemPanel(shopItem, false);
            }
            else // if shopkeeper doesn't have the item, buy it from the player at a default price
            {
                ShopItem shopItem = new ShopItem(item.ItemData, item.Quantity, _defaultSellPrice);
                SpawnItemPanel(shopItem, false);

            }           
        }
    }

    private void SpawnItemPanel(ShopItem item, bool fromShop)
    {
        ShopItemPanel itemPanel;
        
        if (fromShop)
        {
            itemPanel = Instantiate(_shopItemPanelPrefab, _shopInventoryScrollView.content).GetComponent<ShopItemPanel>();
            itemPanel.Initialize(item, this);
            itemPanel.PanelBtn.onClick.AddListener(() => OnItemSelected(itemPanel, true));
            _shopItemPanels.Add(item.ItemData, itemPanel);
        }
        else
        {
            itemPanel = Instantiate(_shopItemPanelPrefab, _playerInventoryScrollView.content).GetComponent<ShopItemPanel>();
            itemPanel.Initialize(item, this);
            itemPanel.PanelBtn.onClick.AddListener(() => OnItemSelected(itemPanel, false));
            _playerItemPanels.Add(item.ItemData, itemPanel);
        }
    }

    private void OnItemSelected(ShopItemPanel itemPanel, bool fromShop)
    {
        SelectedItemPanel = itemPanel;
        _itemInfoPanel.SetActive(true);

        _selectedItemNameField.text = itemPanel.ShopItem.ItemData.Name;
        _selectedItemDescriptionField.text = itemPanel.ShopItem.ItemData.Description;

        // check if it is on player inventory or shop inventory
        if (fromShop)
        {
            _buyBtn.gameObject.SetActive(true);
            _buyBtnField.text = $"[{_buySellKey}] Buy <sprite index=0> {itemPanel.ShopItem.Price}";

            if (_player.PlayerInventory.CurGold >= itemPanel.ShopItem.Price && !_player.PlayerInventory.IsInventoryFull)
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
            int sellPrice = (int)(itemPanel.ShopItem.Price * _sellPriceMultiplier);

            _buyBtn.gameObject.SetActive(false);
            _sellBtn.gameObject.SetActive(true);
            _sellBtnField.text = $"[{_buySellKey}] Sell <sprite index=0> {sellPrice}";

            if (_shop.CurGold >= sellPrice)
            {
                _buyBtn.GetComponent<CanvasGroup>().alpha = 1f;
            }
            else
            {
                _buyBtn.GetComponent<CanvasGroup>().alpha = .5f;
            }

            if (itemPanel.ShopItem.ItemData is not WearableItem)
            {
                _equipBtn.gameObject.SetActive(false);
                _unequipBtn.gameObject.SetActive(false);
            }
            else if (_player.PlayerInventory.HasItemEquipped(itemPanel.ShopItem.ItemData as WearableItem))
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
        _shopItemPanels.Remove(shopItemPanel.ShopItem.ItemData);
        _playerItemPanels.Remove(shopItemPanel.ShopItem.ItemData);
    }

    public void CloseShop()
    {
        GameManager.Instance.UIManager.CloseShopInterface();
        Destroy(_shopCameraInstance);
        Destroy(gameObject);
    }
}
