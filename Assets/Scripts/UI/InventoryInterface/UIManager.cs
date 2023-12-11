using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public event Action OnShopOpened;
    public event Action OnShopClosed;
    public event Action OnInventoryOpened;
    public event Action OnInventoryClosed;

    [SerializeField] private Canvas _positionsCanvas;

    [Header("Prefabs")]
    [SerializeField] private GameObject _shopInterfacePrefab;
    [SerializeField] private GameObject _inventoryInterfacePrefab;

    [Header("Middle")]
    [SerializeField] private Canvas _interactPanel;
    [SerializeField] private Canvas[] _tutorialPanels;

    [Header("Bottom Right")]
    [SerializeField] private Button _inventoryButton;
    public Button InventoryButton => _inventoryButton;

    private InventoryManager _curOpenInventory;
    private ShopManager _curOpenShop;

    void Start()
    {
        StartCoroutine(ShowTutorialPanel());
    }

    public void ShowInteractPanel(bool value)
    {
        _interactPanel.enabled = value;
    }

    public void OpenShopInterface(PlayerController player, Shop shop)
    {
        _curOpenShop = Instantiate(_shopInterfacePrefab).GetComponent<ShopManager>();
        _curOpenShop.Initialize(player, shop);
        _positionsCanvas.enabled = false;
        OnShopOpened();
    }

    public void CloseShopInterface()
    {
        _positionsCanvas.enabled = true;
        OnShopClosed();
    }

    public void ToggleInventory(PlayerController player)
    {
        if (_curOpenInventory == null)
        {
            _curOpenInventory = Instantiate(_inventoryInterfacePrefab).GetComponent<InventoryManager>();
            _curOpenInventory.Initialize(player);
            _positionsCanvas.enabled = false;
            OnShopOpened();
        }
        else
        {
            OnShopClosed();
            _curOpenInventory.CloseInventory();
            _positionsCanvas.enabled = true;
            _curOpenInventory = null;
        }
    }

    private IEnumerator ShowTutorialPanel()
    {
        yield return new WaitForSeconds(.3f);

        foreach (Canvas tutorialPanel in _tutorialPanels)
        {
            tutorialPanel.enabled = true;
            yield return new WaitForSeconds(3f);
        }

        foreach (Canvas tutorialPanel in _tutorialPanels)
        {
            tutorialPanel.enabled = false;
        }
    }
}
