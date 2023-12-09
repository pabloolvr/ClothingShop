using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public Vector2 Position { get; }
    public void Interact(PlayerController player);
}
