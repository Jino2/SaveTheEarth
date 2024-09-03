using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEditor;

public class BaseDialogUIBuilder
{
    private string title = "제목";
    private string message = "메시지";
    private string confirmButtonText = "확인";
    private string cancelButtonText = "취소";
    private Action onConfirm = null;
    private Action onCancel = null;
    
    private const string DialogUITemplatePath = "Assets/JYP/UI/Uxml/UI_Dialog.uxml";
    
    
    public BaseDialogUIBuilder SetTitle(string title)
    {
        this.title = title;
        return this;
    }
    
    public BaseDialogUIBuilder SetMessage(string message)
    {
        this.message = message;
        return this;
    }
    
    public BaseDialogUIBuilder SetConfirmButtonText(string confirmButtonText)
    {
        this.confirmButtonText = confirmButtonText;
        return this;
    }
    
    public BaseDialogUIBuilder SetCancelButtonText(string cancelButtonText)
    {
        this.cancelButtonText = cancelButtonText;
        return this;
    }
    
    public BaseDialogUIBuilder SetOnConfirm(Action onConfirm)
    {
        this.onConfirm = onConfirm;
        return this;
    }
    
    public BaseDialogUIBuilder SetOnCancel(Action onCancel)
    {
        this.onCancel = onCancel;
        return this;
    }
    
    public void Build()
    {
        var dialogUI = new BaseDialogUIController(
            title,
            message,
            confirmButtonText,
            cancelButtonText,
            onConfirm,
            onCancel
            );
        
    }
    
    public class BaseDialogUIController
    {
        private const string DialogUITemplatePath = "Assets/JYP/UI/Uxml/UI_Dialog.uxml";

        private Button confirmButton;
        private Button cancelButton;
        private Label titleLabel;
        private Label messageLabel;
        private TemplateContainer root;
        internal BaseDialogUIController(
            string title,
            string message,
            string confirmButtonText,
            string cancelButtonText,
            Action onConfirm,
            Action onCancel
        )
        {
            var dialogTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(DialogUITemplatePath);
            root = dialogTree.Instantiate();
            confirmButton= root.Q<Button>("btn_Confirm");
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
            };
        
            cancelButton.clicked += () =>
            {
                onCancel?.Invoke();
                root.RemoveFromHierarchy();
            };
        }
    }
}