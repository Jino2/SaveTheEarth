using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseDialogUIController : MonoBehaviour
{
   public VisualTreeAsset dialogUITemplate;
    
    public UIDocument uiDocument;
    private Button confirmButton;
    private Button cancelButton;
    private Label titleLabel;
    private Label messageLabel;
    private TemplateContainer root;
    
    private static BaseDialogUIController instance;
    public static BaseDialogUIController Instance
    {
        get
        {
            if (instance == null)
            {
                var go = Resources.Load("Prefabs/UIPopup");
                var obj = Instantiate(go);
                instance = obj.GetComponent<BaseDialogUIController>();
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        uiDocument.enabled = false;
    }

    public void CreateDialog(
        string title,
        string message,
        string confirmButtonText,
        string cancelButtonText,
        Action onConfirm,
        Action onCancel
    )
    {
        uiDocument.sortingOrder = 100;
        root = dialogUITemplate.Instantiate();
        
        confirmButton = root.Q<Button>("btn_Confirm");
        cancelButton = root.Q<Button>("btn_Cancel");
        titleLabel = root.Q<Label>("txt_Title");
        messageLabel = root.Q<Label>("txt_Message");
        
        confirmButton.text = confirmButtonText;
        cancelButton.text = cancelButtonText;
        titleLabel.text = title;
        messageLabel.text = message;
        confirmButton.clicked += () =>
        {
            onConfirm?.Invoke();
            root.RemoveFromHierarchy();
            uiDocument.enabled = false;
        
        };
        
        cancelButton.clicked += () =>
        {
            onCancel?.Invoke();
            root.RemoveFromHierarchy();
            uiDocument.enabled = false;
        
        };
        uiDocument.enabled = true;
        var container = uiDocument.rootVisualElement.Q<VisualElement>("Container");
        container.Add(root);
        
    }
}