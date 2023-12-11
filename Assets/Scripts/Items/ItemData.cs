using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public string Name => _itemName;
    public string Description => _itemDescription;
    public bool Stackable => _stackable; 
    public Color Color => _color;
    public Sprite Icon => _itemIcon;
    public int ItemPrice => _itemPrice;

    [Header("Base Item Data")]
    [SerializeField] private string _itemName;
    [SerializeField] private string _itemDescription;
    [SerializeField] private Sprite _itemIcon;
    [SerializeField] private Color _color = Color.white;
    [SerializeField] private int _itemPrice;
    [SerializeField] private bool _stackable;
}
