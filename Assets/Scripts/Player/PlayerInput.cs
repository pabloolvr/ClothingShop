using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private KeyCode _moveUpKey = KeyCode.UpArrow;
    [SerializeField] private KeyCode _moveRightKey = KeyCode.RightArrow;
    [SerializeField] private KeyCode _moveDownKey = KeyCode.DownArrow;
    [SerializeField] private KeyCode _moveLeftKey = KeyCode.LeftArrow;
    [SerializeField] private KeyCode _interactKey = KeyCode.E;
    [SerializeField] private KeyCode _inventoryKey = KeyCode.I;

    private PlayerController _playerController;
    private PlayerAnimator _playerAnimator;
    private List<KeyCode> _inputHistory = new List<KeyCode>(4);

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerAnimator = _playerController.PlayerAnimator;
        _playerAnimator.OnAnimatorReset += () => _inputHistory.Clear();
    }

    private void Update()
    {
        if (Input.GetKeyDown(_inventoryKey))
        {
            _playerController.IsAway = !_playerController.IsAway;
            GameManager.Instance.UIManager.ToggleInventory();
            _playerAnimator.ResetAnimator();
        }

        if (_playerController.IsAway) return;

        UpdateMovement();
     
        if (Input.GetKeyDown(_interactKey))
        {
            _playerController.Interact();
            _playerAnimator.ResetAnimator();
        }
    }

    private void UpdateMovement()
    {
        if (Input.GetKeyDown(_moveUpKey))
            _inputHistory.Add(_moveUpKey);

        if (Input.GetKeyDown(_moveRightKey))
            _inputHistory.Add(_moveRightKey);

        if (Input.GetKeyDown(_moveDownKey))
            _inputHistory.Add(_moveDownKey);

        if (Input.GetKeyDown(_moveLeftKey))
            _inputHistory.Add(_moveLeftKey);

        if (Input.GetKeyUp(_moveUpKey))
            _inputHistory.Remove(_moveUpKey);

        if (Input.GetKeyUp(_moveRightKey))
            _inputHistory.Remove(_moveRightKey);

        if (Input.GetKeyUp(_moveDownKey))
            _inputHistory.Remove(_moveDownKey);

        if (Input.GetKeyUp(_moveLeftKey))
            _inputHistory.Remove(_moveLeftKey);

        if (_inputHistory.Count > 0)
        {
            switch (_inputHistory.Last())
            {
                case KeyCode.UpArrow:
                    _playerController.MoveUp();
                    _playerAnimator.PlayWalkAnim(_playerAnimator.WalkUpAnimId);
                    break;
                case KeyCode.RightArrow:
                    _playerController.MoveRight();
                    _playerAnimator.PlayWalkAnim(_playerAnimator.WalkRightAnimId);
                    break;
                case KeyCode.DownArrow:
                    _playerController.MoveDown();
                    _playerAnimator.PlayWalkAnim(_playerAnimator.WalkDownAnimId);
                    break;
                case KeyCode.LeftArrow:
                    _playerController.MoveLeft();
                    _playerAnimator.PlayWalkAnim(_playerAnimator.WalkLeftAnimId);
                    break;
            }
        }
        else
        {
            _playerAnimator.StopWalkAnim();
        }
    }
}
