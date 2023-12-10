using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public event Action OnShopOpened;
    public event Action OnShopClosed;

    [Header("Prefabs")]
    [SerializeField] private GameObject _shopInterfacePrefab;

    [Header("Middle Bottom")]
    [SerializeField] private Canvas _interactPanel;

    private ShopUIManager _curOpenShop;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ShowInteractPanel(bool value)
    {
        _interactPanel.enabled = value;
    }

    public void OpenShopInterface(PlayerController player, Shop shop)
    {
        _curOpenShop = Instantiate(_shopInterfacePrefab).GetComponent<ShopUIManager>();
        _curOpenShop.Initialize(player, shop);
        OnShopOpened();
    }

    public void CloseShopInterface()
    {
        OnShopClosed();
    }
}
