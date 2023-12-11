using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    public List<IInteractable> InteractablesOnRange => _interactablesOnRange;

    public event Action OnDetectionUpdate = () => { };

    //[SerializeField] private float _detectionRadius;
    private List<IInteractable> _interactablesOnRange = new List<IInteractable>();

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            InteractablesOnRange.Add(interactable);
            Debug.Log($"Added {interactable} to interactables on range");
            OnDetectionUpdate();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            InteractablesOnRange.Remove(interactable);
            Debug.Log($"Removed {interactable} from interactables on range");
            OnDetectionUpdate();
        }
    }
}
