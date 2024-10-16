using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonInteractiveObject : MonoBehaviourPun
{
    
    public  GameObject InteractedObject { get; private set; } = null;
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
    
    [SerializeField]
    private string interactMessage = "상호작용";
    
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
        var isCurrentUserOverlapped = false;
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<PhotonView>(out var pv))
            {
                if (pv.IsMine)
                {
                    isCurrentUserOverlapped = true;
                    InteractedObject = collider.gameObject;
                }
            }
        }
        if (isCurrentUserOverlapped && !interactable)
        {
            interactable = true;
            interactGuideText.text = $"{interactKeyCode.ToString()} 키를 눌러 {interactMessage}하세요!";
            interactGuideText.enabled = true;
            
        }
        else if(!isCurrentUserOverlapped && interactable)
        {
            print("O");
            interactable = false;
            interactGuideText.enabled = false;
            InteractedObject = null;
        }
    }
    
}
