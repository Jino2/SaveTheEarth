using System;
using System.Collections.Generic;

public struct ItemApi
{
    private static readonly string BaseURL = "http://54.180.232.125/items";

    public static void GetItems(Action<List<ItemDto>> onComplete)
    {
        var requestInfo = new HttpRequestInfo<string, ItemListResponseDto>
        {
            url = BaseURL + "/selling",
            requestBody = "",
            onSuccess = (res) => { onComplete(res.data); },
            onError = () => { }
        };
        HTTPManager.GetInstance()
            .Get(requestInfo);
    }
    
    public static void GetItemsWithInventory(string userId, Action<List<ItemWithInventory>> onComplete)
    {
        var requestInfo = new HttpRequestInfo<string, ItemWithInventoryListDto>
        {
            url = BaseURL + $"users/{userId}/selling",
            requestBody = "",
            onSuccess = (res) => { onComplete(res.data); },
            onError = () => { }
        };
        HTTPManager.GetInstance()
            .Get(requestInfo);
    }
}