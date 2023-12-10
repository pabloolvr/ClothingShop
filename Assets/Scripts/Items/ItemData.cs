using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Game Item")]
public class ItemData : ScriptableObject
{
    public string Name => _itemName;
    public string Description => _itemDescription;
    public Sprite Icon => _itemIcon;

    [SerializeField] private string _itemName;
    [SerializeField] private string _itemDescription;
    [SerializeField] private Sprite _itemIcon;
}
