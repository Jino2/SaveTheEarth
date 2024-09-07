using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonInteractiveObjectIntoScene : MonoBehaviour
{
    [SerializeField]
    private KeyCode interactKeyCode = KeyCode.M;

    //[SerializeField]
    //private Text interactGuideText;

    [SerializeField]
    private float interactRange = 2.0f;

    [SerializeField]
    private LayerMask interactLayer = 0;

    [SerializeField]
    private string sceneToLoad;

    [SerializeField]
    private UnityEvent onInteract;

    private bool interactable = false;

    private void Start()
    {
        //if (interactGuideText != null)
        //    interactGuideText.enabled = false;
    }

    private void Update()
    {
        CheckInteraction();
        if (interactable && Input.GetKeyDown(interactKeyCode))
        {
            onInteract.Invoke();
            // 씬 전환
            LoadScene();
        }
    }

    private void CheckInteraction()
    {
        var colliders = Physics.OverlapSphere(transform.position, interactRange, interactLayer);

        if (colliders.Length != 0 && !interactable)
        {
            print("X");
            interactable = true;
            //interactGuideText.text = $"Press {interactKeyCode.ToString()}";
            //interactGuideText.enabled = true;

        }
        else if (colliders.Length == 0 && interactable)
        {
            print("O");
            interactable = false;
            //interactGuideText.enabled = false;
        }
    }

    // 씬 전환 함수
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            // 지정된 씬으로 이동
            SceneManager.LoadScene("KJS"); 
        }
        else
        {
            Debug.LogWarning("Scene name is not set!");
        }
    }

}
