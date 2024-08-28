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
    private void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        var sellingItemListController = new SellingItemListUIController();
        sellingItemListController.InitList(uiDocument.rootVisualElement, sellingItemTemplate);
    }

    private void OnEnable()
    {
        // var sellingItemListController = new SellingItemListUIController();
        // sellingItemListController.InitList(uiDocument.rootVisualElement, sellingItemTemplate);
    }
}
