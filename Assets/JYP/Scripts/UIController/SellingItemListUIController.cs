using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class SellingItemListUIController : MonoBehaviour
{
    private List<SellingItem> sellingItems;
    private ListView sellingListView;

    private UserInfo userInfo = new UserInfo()
    {
        id = "test",
        name = "이름",
        level = 0,
    };

    public void InitList(VisualElement root, VisualTreeAsset itemTemplate)
    {
        ItemApi.GetItemsWithInventory(userInfo.id, (list) =>
            {
                sellingItems = list.Select(dto => new SellingItem()
                    {
                        id = dto.itemId,
                        name = dto.name,
                        price = dto.price
                    })
                    .ToList();

                sellingListView.itemsSource = sellingItems;
                sellingListView.Rebuild();
            }
        );
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
            print($"{i} - bind");
            var sellingItemUIController = (SellingItemUIController)element.userData;
            sellingItemUIController.SetItemData(root, sellingItems[i]);
        };

        sellingListView.fixedItemHeight = 45;
    }
}