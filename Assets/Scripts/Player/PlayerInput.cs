using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private KeyCode _moveUpKey = KeyCode.UpArrow;
    [SerializeField] private KeyCode _moveRightKey = KeyCode.RightArrow;
    [SerializeField] private KeyCode _moveDownKey = KeyCode.DownArrow;
    [SerializeField] private KeyCode _moveLeftKey = KeyCode.LeftArrow;
    [SerializeField] private KeyCode _interactKey = KeyCode.E;

    private PlayerController _playerController;

    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKey(_moveUpKey))
            _playerController.MoveUp();
        if (Input.GetKey(_moveRightKey))
            _playerController.MoveRight();
        if (Input.GetKey(_moveDownKey))
            _playerController.MoveDown();
        if (Input.GetKey(_moveLeftKey))
            _playerController.MoveLeft();
        
        if (Input.GetKeyDown(_interactKey))
            _playerController.Interact();
    }
}
