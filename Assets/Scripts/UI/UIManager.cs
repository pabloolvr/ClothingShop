using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Middle Bottom")]
    [SerializeField] private Canvas _interactPanel;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ShowInteractPanel(bool value)
    {
        _interactPanel.enabled = value;
    }
}
