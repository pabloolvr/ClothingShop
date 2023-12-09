using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour, IInteractable
{
    public Vector2 Position => transform.position;

    [SerializeField] private List<Item> _itemStock;
    [SerializeField] private Collider2D _collider;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void Interact(PlayerController player)
    {
        GameManager.Instance.UIManager.OpenShopInterface(player);
    }
}
