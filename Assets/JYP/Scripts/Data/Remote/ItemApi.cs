using System;
using System.Collections.Generic;
using UnityEngine;

public struct ItemApi
{
    private static readonly string BaseURL = $"{HTTPManager.BACKEND_URL}/items";

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
    
    
    public static void GetItemsWithInventory(Action<List<ItemWithInventory>> onComplete)
    {
        var requestInfo = new HttpRequestInfo<string, ItemWithInventoryListDto>
        {
            url = BaseURL + $"/selling",
            requestBody = "",
            onSuccess = (res) => { onComplete(res.data); },
            onError = () => { ; }
        };
        HTTPManager.GetInstance()
            .Get(requestInfo);
    }
}