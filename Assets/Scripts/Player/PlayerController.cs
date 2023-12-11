using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float CharacterSpeed => (float)_characterSpeed / (float)10000;
    /// <summary>
    /// If player can take in-world actions like move, attack, etc.
    /// </summary>
    public bool IsAway { get => _isAway; set => _isAway = value;}
    public PlayerAnimator PlayerAnimator => _playerAnimator;
    public PlayerInventory PlayerInventory => _playerInventory;

    [Header("References")]
    [SerializeField] private PlayerAnimator _playerAnimator;
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private Camera _mainCamera;
    private InteractionDetector _interactionDetector;
    private Rigidbody2D _rigidbody;

    [Header("Movement")]
    [SerializeField, Range(0, 200)] private int _characterSpeed = 100;

    private PlayerWearableSocket[] _wearableSockets;
    private bool _isAway = false;
    private IInteractable _closestInteractable = null;

    private void Awake()
    {
        GetComponents();
        StartComponents();
        SetEvents();       
    }

    private void GetComponents()
    {
        _interactionDetector = GetComponentInChildren<InteractionDetector>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _wearableSockets = GetComponentsInChildren<PlayerWearableSocket>(true);
    }

    private void StartComponents()
    {
        _playerAnimator.Initialize();
        _playerInventory.Initialize(_playerAnimator);
    }

    private void SetEvents()
    {
        _interactionDetector.OnDetectionUpdate += UpdateClosestInteractable;
    }

    private void UpdateClosestInteractable()
    {
        _closestInteractable = null;

        float minDist = float.MaxValue;

        foreach (var interactable in _interactionDetector.InteractablesOnRange)
        {         
            var distanceToInteractable = Vector2.Distance(transform.position, interactable.Position);

            if (distanceToInteractable < minDist)
            {
                _closestInteractable = interactable;
                minDist = distanceToInteractable;
            }
        }

        GameManager.Instance.UIManager.ShowInteractPanel(_closestInteractable != null);
    }

    public void Interact()
    {
        if (_closestInteractable == null) return;

        _closestInteractable.Interact(this);
    }

    public void DetachCamera()
    {
        _mainCamera.transform.SetParent(null);
    }

    public void AttachCamera()
    {
        _mainCamera.transform.SetParent(transform);
    }

    public void MoveUp()
    {
        _rigidbody.MovePosition(transform.position + transform.up * CharacterSpeed);
    }

    public void MoveRight()
    {
        _rigidbody.MovePosition(transform.position + transform.right * CharacterSpeed);
    }

    public void MoveDown()
    {
        _rigidbody.MovePosition(transform.position - transform.up * CharacterSpeed);
    }

    public void MoveLeft()
    {
        _rigidbody.MovePosition(transform.position - transform.right * CharacterSpeed);
    }
}
