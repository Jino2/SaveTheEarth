using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SellingItemListUIController : MonoBehaviour
{
    private SellingItemFetcher sellingItemFetcher = new SellingItemFetcher();
    
    private List<SellingItem> sellingItems;
    private ListView sellingListView;
    
    private void UpdateAllSellingItems()
    {
        sellingItems = sellingItemFetcher.GetAllSellingItems();
    }
    
    public void InitList(VisualElement root, VisualTreeAsset itemTemplate)
    {
        UpdateAllSellingItems();
        sellingListView = root.Q<ListView>("SellingListView");
        sellingListView.makeItem = () =>
        {
            var newListEntry = itemTemplate.Instantiate();
            var sellingItemUIController = new SellingItemUIController();
            newListEntry.userData = sellingItemUIController;

            sellingItemUIController.Initialize(newListEntry);
            return newListEntry;
        };
        
        sellingListView.bindItem = (element, i) =>
        {
            var sellingItemUIController = (SellingItemUIController) element.userData;
            sellingItemUIController.SetItemData(sellingItems[i]);
        };

        sellingListView.fixedItemHeight = 45;
        sellingListView.itemsSource = sellingItems;
    }
}
