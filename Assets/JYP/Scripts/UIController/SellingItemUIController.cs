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

    private SellingItem item;

    public void Initialize(VisualElement root)
    {
        purchaseButton = root.Q<Button>("btn_Purchase");
        nameLabel = root.Q<Label>("name");
        priceLabel = root.Q<Label>("price");
    }

    public void SetItemData(VisualElement root, SellingItem item, Action<int> onBuyItem)
    {
        nameLabel.text = item.name;
        priceLabel.text = $"{item.price} 포인트";
        this.item = item;

        purchaseButton.clicked += () =>
        {
            Debug.Log($"item id: {this.item.id} / amount: 1");
            new BaseDialogUIBuilder()
                .SetTitle("구매")
                .SetMessage("구매하시겠습니까?")
                .SetOnConfirm(() =>
                {
                    UserApi.BuyItem("test", item.id, 1, (t) =>
                    {
                        onBuyItem(item.price);
                        Debug.Log("구매완료");
                    });
                })
                .SetConfirmButtonText("확인")
                .SetCancelButtonText("취소")
                .Build();
        };
    }
}