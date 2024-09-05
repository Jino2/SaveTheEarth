using System;
using System.Collections.Generic;

public struct ItemApi
{
    private static readonly string BaseURL = "http://localhost:8000/items";

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
}