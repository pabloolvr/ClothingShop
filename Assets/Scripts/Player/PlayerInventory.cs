using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerIventoryItem
{
    public ItemData ItemData => _itemData;
    public int Quantity => _quantity;

    private ItemData _itemData;
    private int _quantity;

    public PlayerIventoryItem(ItemData itemData, int quantity)
    {
        _itemData = itemData;
        _quantity = quantity;
    }

    public void AddQuantity(int quantity)
    {
        _quantity += quantity;
    }

    public void RemoveQuantity(int quantity)
    {
        _quantity -= quantity;
    }
}

/// <summary>
/// This class manages items on player inventory or equipped.
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    public int CurGold { get; set; }
    public bool IsInventoryFull => _itemInventory.Count >= _maxInventorySize;

    public List<PlayerIventoryItem> Inventory => _itemInventory.Values.ToList();

    [SerializeField] private int _maxInventorySize = 20;

    /// <summary>
    /// Stores the items and their quantities.
    /// </summary>
    private Dictionary<ItemData, PlayerIventoryItem> _itemInventory = new Dictionary<ItemData, PlayerIventoryItem>();
    private PlayerWearableSocket[] _wearableSockets;
    private PlayerAnimator _playerAnimator;

    void Start()
    {
        CurGold = 3000;
    }
 
    public void Initialize(PlayerAnimator playerAnimator, PlayerWearableSocket[] wearableSockets)
    {
        _wearableSockets = wearableSockets;
        _playerAnimator = playerAnimator;
    }

    public bool TryAddItem(ItemData itemData, int quantity)
    {
        if (_itemInventory.Count >= _maxInventorySize)
        {
            Debug.Log("Inventory is full.");
            return false;
        }

        if (_itemInventory.ContainsKey(itemData))
        {
            _itemInventory[itemData].AddQuantity(quantity);
        }
        else
        {
            _itemInventory.Add(itemData, new PlayerIventoryItem(itemData, quantity));
        }

        return true;
    }

    public bool HasItemEquipped(WearableItem item)
    {
        return _wearableSockets[(int)item.Slot].Item == item;
    }

    public void EquipItem(WearableItem item)
    {
        _wearableSockets[(int)item.Slot].Item = item;
    }

    public void EmptySlot(WearableSlot slot, out WearableItem item)
    {
        item = _wearableSockets[(int)slot].Item;
        _wearableSockets[(int)item.Slot].Item = null;
    }
}
