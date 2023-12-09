using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementState
{
    Idle,
    MovingUp,
    MovingDown,
    MovingLeft,
    MovingRight
}

public class PlayerController : MonoBehaviour
{
    public float CharacterSpeed => (float)_characterSpeed / (float)10000;

    public MovementState MovementState => _movementState;

    [Header("Movement")]
    [SerializeField, Range(0, 200)] private int _characterSpeed = 100;

    private Rigidbody2D _rigidbody;
    private MovementState _movementState = MovementState.Idle;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact()
    {

    }

    public void MoveUp()
    {
        _rigidbody.MovePosition(transform.position + transform.up * CharacterSpeed);
        Debug.Log("MoveUp");
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
