using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SellingItemUIController
{
    private Label nameLabel;
    private Label priceLabel;
    
    public void Initialize(VisualElement root)
    {
        nameLabel = root.Q<Label>("name");
        priceLabel = root.Q<Label>("price");
    }

    public void SetItemData(SellingItem item)
    {
        nameLabel.text = item.Name;
        priceLabel.text = $"{item.Price} 포인트";
    }
}
