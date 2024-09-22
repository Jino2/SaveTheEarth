using System;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using Label = UnityEngine.UIElements.Label;

public class SellingItemUIController
{
    private Label nameLabel;
    private Label priceLabel;
    private Button purchaseButton;
    private VisualElement previewImage;

    private SellingItem item;

    public void Initialize(VisualElement root)
    {
        purchaseButton = root.Q<Button>("btn_Purchase");
        nameLabel = root.Q<Label>("name");
        priceLabel = root.Q<Label>("price");
        previewImage = root.Q<VisualElement>("image_sellingItemPreview");
        

    }

    public void SetItemData(VisualElement root, SellingItem item, Action<SellingItem> onBuyItem, Sprite previewImage)
    {
        nameLabel.text = item.name;
        priceLabel.text = $"{item.price} 포인트";
        this.item = item;
        this.previewImage.style.backgroundImage = previewImage.texture;
        
        purchaseButton.clicked += () =>
        {
            new BaseDialogUIBuilder()
                .SetTitle("구매")
                .SetMessage("구매하시겠습니까?")
                .SetOnConfirm(() =>
                {
                    onBuyItem(item);
               })
                .SetConfirmButtonText("확인")
                .SetCancelButtonText("취소")
                .Build();
        };
    }
}