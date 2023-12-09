using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Game Item")]
public class Item : ScriptableObject
{
    [SerializeField] private string _itemName;
    [SerializeField] private Sprite _itemSprite;
    [SerializeField] private int _itemDescription;
}
