using System;
using System.IO;
using SFB;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using Label = UnityEngine.UIElements.Label;

public class BaseChallengeUploadUIController2 : BaseChallengeUIController
{
    private Label title;
    private VisualElement typeIcon;
    private VisualElement imageContainer;
    private Button uploadButton;
    private Button selectButton;

    private string selectedImagePath = null;
    BaseDialogUIBuilder dialogBuiler = new BaseDialogUIBuilder();
    private ChallengeApi challengeApi = new ChallengeApi();

    public override void Initialize(VisualElement root, ChallengeUIControllerV2 parentController)
    {
        base.Initialize(root, parentController);

        Debug.Log($"Init");

        title = root.Q<Label>("lbl_Title");
        typeIcon = root.Q<VisualElement>("icon_Type");
        imageContainer = root.Q<VisualElement>("img_SelectedImage");
        uploadButton = root.Q<Button>("btn_ChallengeUpload");
        selectButton = root.Q<Button>("btn_ChallengeChooseImage");

        selectButton.clicked += OnSelectButtonClicked;
        uploadButton.clicked += OnUploadButtonClicked;
    }

    public override void BindType(ChallengeType type)
    {
        if(type != ChallengeType.None)
            typeIcon.style.backgroundImage = parentController.challengeTypeSprites[(int)type].texture;
        switch (type)
        {
            case ChallengeType.Transport:
                title.text = "이동수단 챌린지";
                break;
            case ChallengeType.Tumbler:
                title.text = "텀블러 챌린지";
                break;
            case ChallengeType.Recycle:
                title.text = "재활용 챌린지";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #region Private Methods Block

    private void OnSelectButtonClicked()
    {
        string filePath = ChooseImage();
        if (string.IsNullOrEmpty(filePath)) return;
        selectedImagePath = filePath;
        ShowSelectedImage();
    }

    private string ChooseImage()
    {
        string[] ex = new string[]
        {
            "xbm", "tif", "jfif", "ico", "tiff", "gif", "svg", "jpeg", "svgz", "jpg", "webp", "png", "bmp", "pjp",
            "apng", "pjpeg", "avif"
        };
        var paths = StandaloneFileBrowser.OpenFilePanel("열기", "",
            new ExtensionFilter[1] { new ExtensionFilter("이미지 파일", ex) }, false);
        if (paths.Length == 0) return null;
        return paths[0];
    }

    private void ShowSelectedImage()
    {
        imageContainer.Clear();
        var texture = new Texture2D(2, 2);
        texture.LoadImage(File.ReadAllBytes(selectedImagePath));
        var image = new Image
        {
            image = texture
        };
        imageContainer.Add(image);
    }

    private void OnUploadButtonClicked()
    {
        uploadButton.SetEnabled(false);
        if (selectedImagePath == null)
        {
            dialogBuiler
                .SetTitle("경고")
                .SetMessage("이미지를 선택해주세요.")
                .SetConfirmButtonText("확인")
                .SetCancelButtonText("취소")
                .SetOnConfirm(() => { uploadButton.SetEnabled(true); })
                .SetOnCancel(() => { uploadButton.SetEnabled(true); })
                .Build();

            return;
        }

        UploadImage();
    }

    private void UploadImage()
    {
        switch (parentController.currentChallengeType)
        {
            case ChallengeType.Transport:
                challengeApi.TryChallengeTransport(
                    UserCache.GetInstance().Id,
                    selectedImagePath,
                    (result) =>
                    {
                        uploadButton.SetEnabled(true);
                        UserApi.AddPoint(UserCache.GetInstance().Id, 200, (t) => { NextPage(true); });
                    },
                    () =>
                    {
                        NextPage(false);
                        uploadButton.SetEnabled(true);
                    }
                );
                break;
            case ChallengeType.Tumbler:
                challengeApi.TryChallengeTumbler(
                    UserCache.GetInstance().Id,
                    selectedImagePath,
                    (result) =>
                    {
                        uploadButton.SetEnabled(true);
                        UserApi.AddPoint(UserCache.GetInstance().Id, 200, (t) => { NextPage(true); });
                    },
                    () =>
                    {
                        NextPage(false);
                        uploadButton.SetEnabled(true);
                    });
                break;
            case ChallengeType.Recycle:
                challengeApi.TryChallengeRecycling(
                    UserCache.GetInstance().Id,
                    selectedImagePath,
                    (result) =>
                    {
                        uploadButton.SetEnabled(true);
                        UserApi.AddPoint(UserCache.GetInstance().Id, 200, (t) => { NextPage(true); });
                    },
                    () =>
                    {
                        NextPage(false);
                        uploadButton.SetEnabled(true);
                    });
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void NextPage(bool isSuccess)
    {
        parentController.isChallengeSuccess = isSuccess;
        parentController.GoToProcess(ChallengeUIControllerV2.EChallengeProcess.ChallengeComplete);
    }

    #endregion
}