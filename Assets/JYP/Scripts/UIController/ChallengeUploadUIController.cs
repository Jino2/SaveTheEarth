using System;
using System.IO;
using SFB;
using UnityEngine;
using UnityEngine.UIElements;

public class ChallengeUploadUIController : MonoBehaviour
{
    public UIDocument uiDocument;
    public ChallengeFinishUIController FinishUIController;
    private Button chooseImageButton;
    private VisualElement selectedImage;
    private Button uploadButton;

    private string selectedImagePath;

    private ChallengeApi challengeApi = new ChallengeApi();
    private ChallengeInfo challengeInfo;

    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ShowUIWith(ChallengeInfo challengeInfo)
    {
        this.challengeInfo = challengeInfo;
        ShowUI();
    }

    public void ShowUI()
    {
        if (uiDocument.enabled) return;
        uiDocument.enabled = true;
        chooseImageButton = uiDocument.rootVisualElement.Q<Button>("btn_ChallengeChooseImage");
        selectedImage = uiDocument.rootVisualElement.Q<VisualElement>("img_SelectedImage");
        uploadButton = uiDocument.rootVisualElement.Q<Button>("btn_ChallengeUpload");

        chooseImageButton.clicked += OnChooseImageButtonClicked;
        uploadButton.clicked += OnUploadButtonClicked;
    }

    private void OnUploadButtonClicked()
    {
        switch (challengeInfo.type)
        {
            case ChallengeType.Transport:
                challengeApi.TryChallengeTransport("test", selectedImagePath,
                    (result) =>
                    {
                        UserApi.AddPoint("test", 200, (t) =>
                        {
                            uiDocument.enabled = false;
                            FinishUIController.ShowUIWith(true);
                            
                        });
                    },
                    () =>
                    {
                        uiDocument.enabled = false;
                        FinishUIController.ShowUIWith(false);
                    });
                break;
            case ChallengeType.Tumbler:
                challengeApi.TryChallengeTumbler("test", selectedImagePath,
                    (result) =>
                    {
                        UserApi.AddPoint("test", 200, (t) =>
                        {
                            uiDocument.enabled = false;
                            FinishUIController.ShowUIWith(true);
                        });
                    },
                    () =>
                    {
                        uiDocument.enabled = false;
                        FinishUIController.ShowUIWith(false);
                    });
                break;
            case ChallengeType.Recycle:
                challengeApi.TryChallengeRecycling("test", selectedImagePath,
                    (result) =>
                    {
                        UserApi.AddPoint("test", 200, (t) =>
                        {
                            uiDocument.enabled = false;
                            FinishUIController.ShowUIWith(true);
                        });
                    },
                    () =>
                    {
                        uiDocument.enabled = false;
                        FinishUIController.ShowUIWith(false);
                    });
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        // goto NextPage
    }


    private void OnChooseImageButtonClicked()
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
        selectedImage.Clear();
        var texture = new Texture2D(2, 2);
        texture.LoadImage(File.ReadAllBytes(selectedImagePath));
        var image = new Image
        {
            image = texture
        };
        selectedImage.Add(image);
    }
}