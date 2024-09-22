using Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopUIController : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset sellingItemTemplate;

    [SerializeField]
    public Shop shop;
    public UIDocument uiDocument;
    public CinemachineVirtualCamera shopUICamera;
    
    private Button closeButton;
    private Label pointLabel;
    private int point = 0;
    public Sprite[] itemPreviewSprites;
    
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
        shopUICamera.gameObject.SetActive(true);
        var sellingItemListController = new SellingItemListUIController();
        closeButton = uiDocument.rootVisualElement.Q<Button>("closeButton");
        pointLabel = uiDocument.rootVisualElement.Q<Label>("point");
        closeButton.clicked += OnCloseButtonClicked;
        sellingItemListController.InitList(uiDocument.rootVisualElement,itemPreviewSprites, sellingItemTemplate, item =>
        {
            point -= item.price;
            pointLabel.text = point.ToString();
            shop.SoldItem(item.id);
        });
        UserApi.GetUserInfo(UserCache.GetInstance().Id ?? UserCache.GetInstance().Id, info =>
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
        shopUICamera.gameObject.SetActive(false);
    }
}