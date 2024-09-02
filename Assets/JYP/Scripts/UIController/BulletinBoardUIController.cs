using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class BulletinBoardUIController : MonoBehaviour
{

    public UnityEvent OnChallengeButtonClickedAction;
    public UnityEvent OnShopButtonClickedAction;
    
    private UIDocument uiDocument;

    private Button challengeButton;
    private Button shopButton;
    
    private void Start()
    {
        uiDocument = GetComponent<UIDocument>();
    }

    public void ShowUI()
    {
        if (uiDocument.enabled) return;
        uiDocument.enabled = true;
        challengeButton = uiDocument.rootVisualElement.Q<Button>("ChallengeButton");
        shopButton = uiDocument.rootVisualElement.Q<Button>("ShopButton");
        
        challengeButton.clicked += OnChallengeButtonClicked;
        shopButton.clicked += OnShopButtonClicked;
    }
    
    private void OnChallengeButtonClicked()
    {
        HideUI();
        OnChallengeButtonClickedAction.Invoke();
    }
    
    private void OnShopButtonClicked()
    {
        print($"OnShopButtonClicked");
        HideUI();
        OnShopButtonClickedAction.Invoke();
    }

    
    void HideUI()
    {
        if (!uiDocument.enabled) return;
        uiDocument.enabled = false;
    }
}
