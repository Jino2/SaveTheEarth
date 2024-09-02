using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChallengeUIController : MonoBehaviour
{
    [SerializeField]
    private VisualTreeAsset challengeItemTemplate;
    
    public UIDocument uiDocument;

    private Button closeButton;
    private void Start()
    {
        uiDocument = GetComponent<UIDocument>();
    }

    private void Update()
    {

    }

    public void ShowUI()
    {
        if (uiDocument.enabled) return;
        uiDocument.enabled = true;
        var challengeItemListController = new ChallengeListUIController();
        closeButton = uiDocument.rootVisualElement.Q<Button>("closeButton");
        closeButton.clicked += OnCloseButtonClicked;
        challengeItemListController.InitList(uiDocument.rootVisualElement, challengeItemTemplate);
    }
    
    private void OnCloseButtonClicked()
    {
        print("Close button clicked");
        HideUI();
    }
    
    void HideUI()
    {
        if (!uiDocument.enabled) return;
        uiDocument.enabled = false;
    }
}
