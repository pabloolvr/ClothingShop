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

    private PlayerController _playerController;
    private List<KeyCode> _inputHistory = new List<KeyCode>(4);

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (_playerController.IsAway) return;

        UpdateMovement();
     
        if (Input.GetKeyDown(_interactKey))
            _playerController.Interact();
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
                    break;
                case KeyCode.RightArrow:
                    _playerController.MoveRight();
                    break;
                case KeyCode.DownArrow:
                    _playerController.MoveDown();
                    break;
                case KeyCode.LeftArrow:
                    _playerController.MoveLeft();
                    break;
            }
        }
    }
}