using UnityEngine;
using UnityEngine.UIElements;

public class ShopUIController : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset sellingItemTemplate;

    public UIDocument uiDocument;

    private Button closeButton;
    private Label pointLabel;
    private int point = 0;
    private void Start()
    {
        uiDocument = GetComponent<UIDocument>();
    }

    private void Update()
    {
    }

    public void ShowShopUI()
    {
        if (uiDocument.enabled) return;
        uiDocument.enabled = true;
        var sellingItemListController = new SellingItemListUIController();
        closeButton = uiDocument.rootVisualElement.Q<Button>("closeButton");
        pointLabel = uiDocument.rootVisualElement.Q<Label>("point");
        closeButton.clicked += OnCloseButtonClicked;
        sellingItemListController.InitList(uiDocument.rootVisualElement, sellingItemTemplate, i =>
        {
            point -= i;
            pointLabel.text = point.ToString();
        });
        UserApi.GetUserInfo("test", info =>
        {
            point = info.point;
            pointLabel.text = point.ToString();
        });
        
    }

    private void OnCloseButtonClicked()
    {
        print("Close button clicked");
        HideShopUI();
    }

    void HideShopUI()
    {
        if (!uiDocument.enabled) return;
        uiDocument.enabled = false;
    }
}