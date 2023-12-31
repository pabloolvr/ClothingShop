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
    private PlayerWearableSocket[] _previewWearableSockets;
    private int _curPreviewSlot = -1;
    private PlayerAnimator _playerAnimator;

    void Start()
    {
        CurGold = 3000;
    }
 
    public void Initialize(PlayerAnimator playerAnimator)
    {
        _playerAnimator = playerAnimator;
        _wearableSockets = playerAnimator.WearableSockets;
        _previewWearableSockets = playerAnimator.PreviewWearableSockets;
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

    public bool RemoveFromInventory(ItemInstance itemInstance, int quantity)
    {
        if (_itemInventory.Contains(itemInstance))
        {
            if (!itemInstance.ItemData.Stackable)
            {
                _itemInventory.Remove(itemInstance);
                return true;
            }
            else if (itemInstance.Quantity >= quantity)
            {
                itemInstance.RemoveQuantity(quantity);
                return true;
            }
            else
            {
                _itemInventory.Remove(itemInstance);
                return true;
            }
        }

        return false;
    }

    public bool HasItemEquipped(WearableItem item)
    {
        return _wearableSockets[(int)item.Slot].Item == item;
    }

    public void PreviewItem(WearableItem item)
    {
        Debug.Log($"Previewing {item}");
        EmptyPreviewSlot(out WearableItem _);
        _previewWearableSockets[(int)item.Slot].Item = item;
        _curPreviewSlot = (int)item.Slot;
        //_previewWearableSockets[(int)item.Slot].gameObject.SetActive(true);
        _wearableSockets[(int)item.Slot].gameObject.SetActive(false);
    }

    public void EmptyPreviewSlot(out WearableItem item)
    {
        _playerAnimator.ResetAnimator();
        if (_curPreviewSlot == -1)
        {
            item = null;
            return;
        }

        item = _previewWearableSockets[_curPreviewSlot].Item;
        Debug.Log($"Emptying slot {_curPreviewSlot} from {item}");
        _previewWearableSockets[_curPreviewSlot].Item = null;
        _wearableSockets[_curPreviewSlot].Item = _wearableSockets[_curPreviewSlot].Item;
    }

    public void EquipItem(WearableItem item)
    {
        EmptyPreviewSlot(out WearableItem _);
        _wearableSockets[(int)item.Slot].Item = item;
    }

    public bool EmptySlot(WearableSlot slot, out WearableItem item)
    {
        EmptyPreviewSlot(out WearableItem _);
        item = _wearableSockets[(int)slot].Item;
        _wearableSockets[(int)slot].Item = null;

        return item != null;
    }
}
