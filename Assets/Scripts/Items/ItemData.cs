using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Game Item")]
public class ItemData : ScriptableObject
{
    public string ItemName => _itemName;
    public Sprite ItemIcon => _itemIcon;
    public int ItemDescription => _itemDescription;

    [SerializeField] private string _itemName;
    [SerializeField] private Sprite _itemIcon;
    [SerializeField] private int _itemDescription;
}
