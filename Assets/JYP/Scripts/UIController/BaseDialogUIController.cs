using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseDialogUIController
{
    private const string DialogUITemplatePath = "Assets/JYP/UI/Uxml/UI_Dialog.uxml";

    private Button confirmButton;
    private Button cancelButton;
    private Label titleLabel;
    private Label messageLabel;

    public BaseDialogUIController(
        string title,
        string message,
        string confirmButtonText,
        string cancelButtonText,
        Action onConfirm,
        Action onCancel
        )
    {
        var dialogTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(DialogUITemplatePath);
        var root = dialogTree.Instantiate();
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