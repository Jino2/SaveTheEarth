using System;
using UnityEngine.Android;

public struct ShopApi
{
    private static readonly string BASE_URL = "https://localhost:8000/shop";

    public static void GetTest()
    {
        HTTPManager.GetInstance()
            .Test();
    }

    public static void BuySellingItem(int buyingItemId, Action<string> onComplete)
    {
        //TODO: backend 연결
        return;
        var requestInfo = new HttpRequestInfo<BuyItemRequestDto, string>
        {
            url = BASE_URL + "/buy",
            requestBody = new BuyItemRequestDto { itemId = buyingItemId },
            onSuccess = onComplete,
            onError = () => { },
            contentType = "application/json"
        };
        HTTPManager.GetInstance()
            .Post(requestInfo);
    }
}