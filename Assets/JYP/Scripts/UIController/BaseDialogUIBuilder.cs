using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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
        BaseDialogUIController.Instance.CreateDialog(
            title,
            message,
            confirmButtonText,
            cancelButtonText,
            onConfirm,
            onCancel
        );
    }

}