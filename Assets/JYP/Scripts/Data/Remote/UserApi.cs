using System;
using System.Collections.Generic;

public struct UserApi
{
    public static string BASE_URL = "http://54.180.232.125/users";


    public static void Register(string id, string password, string name, Action<string> onComplete)
    {
        var requestInfo = new HttpRequestInfo<CreateUserRequestDto, string>()
        {
            url = BASE_URL,
            requestBody = new CreateUserRequestDto()
            {
                id = id,
                password = password,
                name = name
            },
            onSuccess = onComplete,
            onError = () => { }
        };
        HTTPManager.GetInstance()
            .Post(requestInfo);
    }

    public static void Login(string id, Action<LoginUserResponseDto> onComplete)
    {
        var requestInfo = new HttpRequestInfo<LoginUserRequestDto, LoginUserResponseDto>()
        {
            url = BASE_URL + "/login",
            requestBody = new LoginUserRequestDto()
            {
                id = id,
            },
            onSuccess = (t) =>
            {
                var user = UserCache.GetInstance();
                user.Id = t.id;
                user.Point = t.point;
                onComplete(t);
            },
            onError = () => { }
        };
        
        HTTPManager.GetInstance()
            .Post(requestInfo);
    }

    public static void GetUserInventoryList(string id=null, Action<List<InventoryItem>> onComplete = null)
    {
        id ??= UserCache.GetInstance().Id;
        var requestInfo = new HttpRequestInfo<string, InventoryItemListResponseDto>
        {
            url = BASE_URL + $"/{id}/inventory",
            requestBody = "",
            onSuccess = (res) => { onComplete?.Invoke(res.data); },
            onError = () => {  }
        };
        HTTPManager.GetInstance()
            .Get(requestInfo);
    }

    public static void AddItemToUserInventory(string userId, int itemId, Action<string> onComplete)
    {
        var requestInfo = new HttpRequestInfo<AddItemToInventoryRequestDto, string>
        {
            url = BASE_URL + $"/{userId}/inventory/add",
            requestBody = new AddItemToInventoryRequestDto()
            {
                itemId = itemId
            },
            onSuccess = onComplete,
            onError = () => { }
        };
        HTTPManager.GetInstance()
            .Post(requestInfo);
    }

    public static void DeleteItemFromUserInventory(string userId, int itemId, Action<string> onComplete)
    {
        var requestInfo = new HttpRequestInfo<DeleteItemFromInventoryRequestDto, string>
        {
            url = BASE_URL + $"/{userId}/inventory",
            requestBody = new DeleteItemFromInventoryRequestDto()
            {
                itemId = itemId
            },
            onSuccess = onComplete,
            onError = () => { }
        };
        HTTPManager.GetInstance()
            .Delete(requestInfo);
    }

    public static void BuyItem(string userId, int itemId,int amount, Action<string> onComplete)
    {
        var requestInfo = new HttpRequestInfo<BuyItemRequestDto, string>
        {
            url = BASE_URL + $"/items/buy",
            requestBody = new BuyItemRequestDto()
            {
                userId = userId,
                itemId = itemId,
                amount = amount
            },
            onSuccess = onComplete,
            onError = () => { }
        };
        HTTPManager.GetInstance()
            .Post(requestInfo);
    }
    
    public static void GetUserInfo(string userId, Action<UserInfo> onComplete)
    {
        var requestInfo = new HttpRequestInfo<string, UserInfo>
        {
            url = BASE_URL + $"/{userId}",
            requestBody = "",
            onSuccess = onComplete,
            onError = () => { }
        };
        HTTPManager.GetInstance()
            .Get(requestInfo);
    }
    
    public static void AddPoint(string userId, int point, Action<UserInfo> onComplete)
    {
        var requestInfo = new HttpRequestInfo<AddPointRequestDto, UserInfo>
        {
            url = BASE_URL + $"/{userId}/point",
            requestBody = new AddPointRequestDto()
            {
                point = point
            },
            onSuccess = onComplete,
            onError = () => { }
        };
        HTTPManager.GetInstance()
            .Post(requestInfo);
    }
}