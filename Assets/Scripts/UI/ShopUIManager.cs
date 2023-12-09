using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIManager : MonoBehaviour
{
    [SerializeField] private RawImage _playerPreviewDisplay;
    [SerializeField] private GameObject _shopCameraPrefab;
    [SerializeField] private Button _closeBtn;

    private GameObject _shopCameraInstance;
    private PlayerController _player;

    public void Initialize(PlayerController player)
    {
        _player = player;
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
