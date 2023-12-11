using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _cameraPrefab;
    [SerializeField] private GameObject _itemPanelPrefab;

    [Header("References")]
    [SerializeField] private RawImage _playerPreviewDisplay;
    [SerializeField] private TextMeshProUGUI _playerGoldField;
    [SerializeField] private ScrollRect _playerInventoryScrollView;

    [SerializeField] private Button _closeBtn;

    [Header("Item Information References")]
    [SerializeField] private GameObject _itemInfoPanel;
    [SerializeField] private TextMeshProUGUI _selectedItemNameField;
    [SerializeField] private TextMeshProUGUI _selectedItemDescriptionField;
    [SerializeField] private Button _equipBtn;
    [SerializeField] private Button _unequipBtn;

    //[Header("Settings")]
    //[SerializeField] private float _actionDelay = 1f;

    [Header("Input")]
    [SerializeField] private KeyCode _EquipUnequipKey = KeyCode.E;
    [SerializeField] private KeyCode _inventoryKey = KeyCode.I;

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
    private GameObject _cameraInstance;
    private PlayerController _player;
    private Dictionary<ItemInstance, ShopItemPanel> _playerItemPanels = new Dictionary<ItemInstance, ShopItemPanel>();

    private void Start()
    {
        _equipBtn.onClick.AddListener(PlayerEquipItem);
        _unequipBtn.onClick.AddListener(PlayerUnequipItem);
        _closeBtn.onClick.AddListener(() => GameManager.Instance.UIManager.ToggleInventory(_player));
    }

    public void Initialize(PlayerController player)
    {
        _player = player;
        _cameraInstance = Instantiate(_cameraPrefab, _player.transform);

        _playerGoldField.text = "<sprite index=0> " + _player.PlayerInventory.CurGold.ToString();

        FillPlayerInventory();
    }

    private void Update()
    {
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

        if (Input.GetKeyDown(_inventoryKey))
        {
            GameManager.Instance.UIManager.ToggleInventory(_player);
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
            CloseItemInfoPanel();
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
        OnItemSelected(SelectedItemPanel);
    }

    /// <summary>
    /// Instantiate a shop item panel and Shop Item for each item in the player inventory.
    /// </summary>
    private void FillPlayerInventory()
    {
        foreach (ItemInstance item in _player.PlayerInventory.ItemInventory)
        {
            SpawnItemPanel(item);
        }
    }

    /// <summary>
    /// Tries to spawn item panel. 
    /// If item is stackable, it will try to find the item panel and add the quantity, withou spawning a new one.
    /// </summary>
    /// <param name="itemInstance"></param>
    /// <param name="fromShop"></param>
    /// <returns></returns>
    private bool SpawnItemPanel(ItemInstance itemInstance)
    {
        ShopItemPanel itemPanel;

        if (itemInstance.ItemData.Stackable)
        {
            foreach (ItemInstance item in _playerItemPanels.Keys)
            {
                if (item.ItemData == itemInstance.ItemData)
                {
                    //itemPanels[item].Quantity += itemInstance.Quantity;
                    _playerItemPanels[item].AddQuantity(itemInstance.Quantity, out _);
                    return false;
                }
            }
        }

        itemPanel = Instantiate(_itemPanelPrefab, _playerInventoryScrollView.content).GetComponent<ShopItemPanel>();
        itemPanel.Initialize(itemInstance);
        itemPanel.PanelBtn.onClick.AddListener(() => OnItemSelected(itemPanel));
        itemPanel.SetEquipped(_player.PlayerInventory.HasItemEquipped(itemInstance.ItemData as WearableItem));
        _playerItemPanels.Add(itemInstance, itemPanel);
        return true;
    }

    private void OnItemSelected(ShopItemPanel itemPanel)
    {
        SelectedItemPanel = itemPanel;

        _selectedItemNameField.text = itemPanel.Item.ItemData.Name;
        _selectedItemDescriptionField.text = itemPanel.Item.ItemData.Description;

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

    public void OnShopItemPanelDestroyed(ShopItemPanel shopItemPanel)
    {
        _playerItemPanels.Remove(shopItemPanel.Item);
    }

    public void CloseInventory()
    {
        _player.PlayerInventory.EmptyPreviewSlot(out WearableItem _);      
        Destroy(_cameraInstance);
        Destroy(gameObject);
    }
}
