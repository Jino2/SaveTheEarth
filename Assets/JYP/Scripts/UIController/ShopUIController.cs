using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class ShopUIController : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset sellingItemTemplate;

    [SerializeField]
    public Shop shop;
    public UIDocument uiDocument;
    public CinemachineVirtualCamera shopUICamera;
    public ButtonInteractiveObject interactiveObject;
    
    private Button closeButton;
    private Label pointLabel;
    private int point = 0;
    public Sprite[] itemPreviewSprites;
    private GameObject shoppingPlayerObject;
    private VisualElement shopListView;
    private void Start()
    {
        uiDocument = GetComponent<UIDocument>();
    }

    private void Update()
    {

        if (uiDocument.enabled)
        {
            print(shopListView.ClassListContains("hide--vending-machine"));
        }
        
    }

    public void ShowShopUI()
    {
        if (uiDocument.enabled) return;
        Cursor.lockState = CursorLockMode.None;
        this.shoppingPlayerObject = interactiveObject.InteractedObject;
        shoppingPlayerObject.TryGetComponent<PlayerMove>(out var playerMove);
        if(playerMove != null)
        {
            playerMove.controllable = false;
        }
        uiDocument.enabled = true;
        shopUICamera.gameObject.SetActive(true);
        var sellingItemListController = new SellingItemListUIController();
        closeButton = uiDocument.rootVisualElement.Q<Button>("closeButton");
        shopListView = uiDocument.rootVisualElement.Q<VisualElement>("VendingMahcine");
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
            UserCache.GetInstance().Point = info.point;
        });
        StartCoroutine(AnimateUI());
    }

    private IEnumerator AnimateUI()
    {
        yield return new WaitForSeconds(0.1f);
        shopListView.RemoveFromClassList("hide--vending-machine");

    }

    private void OnCloseButtonClicked()
    {
        Cursor.lockState = CursorLockMode.Locked;
        print("Close button clicked");
        closeButton.clicked -= OnCloseButtonClicked;
        StartCoroutine(DelayedHideShopUI());
    }
    
    private IEnumerator DelayedHideShopUI()
    {
        if (!uiDocument.enabled) yield break;
        
        shoppingPlayerObject.TryGetComponent<PlayerMove>(out var playerMove);
        if(playerMove != null)
        {
            playerMove.controllable = true;
        }

        yield return new WaitForSeconds(0.1f);
        shopListView.AddToClassList("hide--vending-machine");
        yield return new WaitForSeconds(0.8f);
        uiDocument.enabled = false;
        shopUICamera.gameObject.SetActive(false);
    }
}