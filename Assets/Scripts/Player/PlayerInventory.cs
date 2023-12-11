using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class manages items on player inventory or equipped.
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    public int CurGold { get; set; }
    public bool IsInventoryFull => _itemInventory.Count >= _maxInventorySize;
    public List<ItemInstance> ItemInventory => _itemInventory;
    [SerializeField] private int _maxInventorySize = 20;

    /// <summary>
    /// Stores the items and their quantities.
    /// </summary>
    //private Dictionary<ItemData, List<ItemInstance>> _itemInventory = new Dictionary<ItemData, List<ItemInstance>>();
    private List<ItemInstance> _itemInventory = new List<ItemInstance>();
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

    /// <summary>
    /// Tries to add a single item to the inventory. 
    /// It creates a new item instance if the item is 
    /// non stackable or stackable without a item instance.
    /// </summary>
    /// <param name="itemData">Base item data.</param>
    /// <param name="itemInstance">New item instance created, null otherwise.</param>
    /// <returns>True if item was added, false otherwise.</returns>
    public bool TryAddItem(ItemData itemData, out ItemInstance itemInstance)
    {
        itemInstance = null;

        if (_itemInventory.Count >= _maxInventorySize)
        {
            Debug.Log("Inventory is full.");
            return false;
        }

        if (!itemData.Stackable)
        {
            itemInstance = new ItemInstance(itemData, 1, itemData.ItemPrice);
            _itemInventory.Add(itemInstance);
        }
        else 
        {
            foreach (ItemInstance item in _itemInventory)
            {
                if (item.ItemData == itemData)
                {
                    item.AddQuantity(1);
                    return true;
                }
            }

            itemInstance = new ItemInstance(itemData, 1, itemData.ItemPrice);
            _itemInventory.Add(itemInstance);
        }

        return true;
    }

    public bool HasItemEquipped(WearableItem item)
    {
        return _wearableSockets[(int)item.Slot].Item == item;
    }

    public void EquipItem(WearableItem item)
    {
        _playerAnimator.ResetAnimator();
        _wearableSockets[(int)item.Slot].Item = item;
    }

    public void EmptySlot(WearableSlot slot, out WearableItem item)
    {
        item = _wearableSockets[(int)slot].Item;
        _wearableSockets[(int)item.Slot].Item = null;
    }
}
