using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public UIManager UIManager => _uiManager;

    [Header("References")]
    [SerializeField] private UIManager _uiManager;

    private PlayerController _player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
