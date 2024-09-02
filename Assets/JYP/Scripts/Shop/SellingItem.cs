using UnityEngine;

public class SellingItem
{
    public string Name { get; set; }
    public int Price { get; set; }

    public SellingItem(string name, int price)
    {
        Name = name;
        Price = price;
    }
}