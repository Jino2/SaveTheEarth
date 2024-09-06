using System;

[Serializable]
class BuyItemRequestDto
{
    public string userId;
    public int itemId;
    public int amount;
}