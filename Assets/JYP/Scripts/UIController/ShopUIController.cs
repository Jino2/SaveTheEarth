using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopUIController : MonoBehaviour
{
    [SerializeField]
    private VisualTreeAsset sellingItemTemplate;
    
    public UIDocument uiDocument;

    private Button closeButton;
    private void Start()
    {
        uiDocument = GetComponent<UIDocument>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ShopApi.GetTest();
        }
    }

    public void ShowShopUI()
    {
        if (uiDocument.enabled) return;
        uiDocument.enabled = true;
        var sellingItemListController = new SellingItemListUIController();
        closeButton = uiDocument.rootVisualElement.Q<Button>("closeButton");
        closeButton.clicked += OnCloseButtonClicked;
        sellingItemListController.InitList(uiDocument.rootVisualElement, sellingItemTemplate);
    }
    
    private void OnCloseButtonClicked()
    {
        print("Close button clicked");
        HideShopUI();
    }
    
    void HideShopUI()
    {
        if (!uiDocument.enabled) return;
        uiDocument.enabled = false;
    }
}
