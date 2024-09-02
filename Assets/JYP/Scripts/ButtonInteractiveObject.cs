using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonInteractiveObject : MonoBehaviour
{
    [SerializeField]
    private KeyCode interactKeyCode = KeyCode.E;

    [SerializeField]
    private Text interactGuideText;
    
    [SerializeField]
    private float interactRange = 2.0f;
    
    [SerializeField]
    private LayerMask interactLayer = 0;

    [SerializeField]
    private UnityEvent onInteract;
    
    private bool interactable = false;
    
    private void Start()
    {
        if (interactGuideText != null)
            interactGuideText.enabled = false;
    }

    private void Update()
    {
        CheckInteraction();
        if (interactable && Input.GetKeyDown(interactKeyCode))
        {
            onInteract.Invoke();
        }
    }

    private void CheckInteraction()
    {
        var colliders = Physics.OverlapSphere(transform.position, interactRange, interactLayer);
    
        if (colliders.Length != 0 && !interactable)
        {
            print("X");
            interactable = true;
            interactGuideText.text = $"Press {interactKeyCode.ToString()}";
            interactGuideText.enabled = true;
            
        }
        else if(colliders.Length == 0 && interactable)
        {
            print("O");
            interactable = false;
            interactGuideText.enabled = false;
        }
    }
    
}
