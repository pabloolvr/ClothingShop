using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public event Action OnShopOpened;
    public event Action OnShopClosed;

    [SerializeField] private Canvas _positionsCanvas;

    [Header("Prefabs")]
    [SerializeField] private GameObject _shopInterfacePrefab;

    [Header("Middle")]
    [SerializeField] private Canvas _interactPanel;
    [SerializeField] private Canvas _tutorialPanel;

    [Header("Bottom Right")]
    [SerializeField] private Button _inventoryButton;

    private ShopUIManager _curOpenShop;

    void Start()
    {
        StartCoroutine(ShowTutorialPanel());
        _inventoryButton.onClick.AddListener(ToggleInventory);
    }

    public void ShowInteractPanel(bool value)
    {
        _interactPanel.enabled = value;
    }

    public void OpenShopInterface(PlayerController player, Shop shop)
    {
        _curOpenShop = Instantiate(_shopInterfacePrefab).GetComponent<ShopUIManager>();
        _curOpenShop.Initialize(player, shop);
        _positionsCanvas.enabled = false;
        OnShopOpened();
    }

    public void CloseShopInterface()
    {
        _positionsCanvas.enabled = true;
        OnShopClosed();
    }

    public void ToggleInventory()
    {

    }

    private IEnumerator ShowTutorialPanel()
    {
        yield return new WaitForSeconds(.3f);
        _tutorialPanel.enabled = true;

        yield return new WaitForSeconds(3f);
        _tutorialPanel.enabled = false;
    }
}
