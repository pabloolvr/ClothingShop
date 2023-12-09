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

    [Header("References")]
    public int playId;
    private InteractionDetector _interactionDetector;
    private Rigidbody2D _rigidbody;

    [Header("Movement")]
    [SerializeField, Range(0, 200)] private int _characterSpeed = 100;

    //[Header("States")]
    private bool _isAway = false;
    private IInteractable _closestInteractable = null;

    private void Awake()
    {
        _interactionDetector = GetComponentInChildren<InteractionDetector>();
        _interactionDetector.OnDetectionUpdate += UpdateClosestInteractable;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

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
