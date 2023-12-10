using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _shopCameraPrefab;
    [SerializeField] private GameObject _shopItemPanelPrefab;

    [Header("References")]
    [SerializeField] private RawImage _playerPreviewDisplay;
    [SerializeField] private TextMeshProUGUI _playerGoldField;
    [SerializeField] private Button _closeBtn;
    
    [Header("Item Information References")]
    [SerializeField] private Button _buyBtn;
    [SerializeField] private Button _sellBtn;
    [SerializeField] private Button _confirmBtn;
    [SerializeField] private Button _equipBtn;
    [SerializeField] private Button _unequipBtn;

    [Header("Settings")]
    [SerializeField] private float _actionDelay = 1f;

    private ShopItemPanel _selectedItemPanel = null;
    private GameObject _shopCameraInstance;
    private PlayerController _player;
    private Shop _shop;

    public void Initialize(PlayerController player, Shop shop)
    {
        _player = player;
        _shop = shop;
        _shopCameraInstance = Instantiate(_shopCameraPrefab, _player.transform);
        _closeBtn.onClick.AddListener(CloseShop);
    }

    public void CloseShop()
    {
        GameManager.Instance.UIManager.CloseShopInterface();
        Destroy(_shopCameraInstance);
        Destroy(gameObject);
    }
}
