using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    //public PlayerController Player => _player;
    public UIManager UIManager => _uiManager;

    [Header("Prefabs")]
    [SerializeField] private GameObject _playerPrefab;

    [Header("References")]
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private Transform _entitiesHolder;

    private Vector2 _lastPlayerPos;
    private PlayerController _player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Destroy(Camera.main.gameObject);
            _player = Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity, _entitiesHolder).GetComponent< PlayerController>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _uiManager.OnShopOpened += OnShopOpened;
        _uiManager.OnShopClosed += OnShopClosed;
        _uiManager.OnInventoryOpened += OnShopOpened;
        _uiManager.OnInventoryClosed += OnShopClosed;
        _uiManager.InventoryButton.onClick.AddListener(() => _uiManager.ToggleInventory(_player));
    }

    private void OnShopOpened()
    {
        _player.DetachCamera();
        _player.IsAway = true;
        _player.PlayerAnimator.ResetAnimator();
        // teleports the player to a position with nothing around
        _lastPlayerPos = _player.transform.position;
        _player.transform.position = new Vector2(99999, 99999);
    }

    private void OnShopClosed()
    {
        _player.IsAway = false;
        _player.transform.position = _lastPlayerPos;
        _player.AttachCamera();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
