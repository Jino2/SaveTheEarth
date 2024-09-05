using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SellingItemUIController
{
    private Label nameLabel;
    private Label priceLabel;
    private Button purchaseButton;
    
    public void Initialize(VisualElement root)
    {
        purchaseButton = root.Q<Button>("btn_Purchase");
        nameLabel = root.Q<Label>("name");
        priceLabel = root.Q<Label>("price");
    }

    public void SetItemData(VisualElement root ,SellingItem item)
    {
        nameLabel.text = item.name;
        priceLabel.text = $"{item.price} 포인트";
        purchaseButton.clicked += () =>
        {
            new BaseDialogUIBuilder()
                .SetTitle("구매")
                .SetMessage("구매하시겠습니까?")
                .SetOnConfirm(() => Debug.Log("구매완료"))
                .SetConfirmButtonText("확인")
                .SetCancelButtonText("취소")
                .Build();

        };
    }
}
