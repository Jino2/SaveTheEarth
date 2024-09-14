using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ChallengeUIControllerV2 : MonoBehaviour
{
    public enum EChallengeProcess
    {
        SelectChallenge,
        ChallengeUploadImage,
        ChallengeComplete,
    }

    public UIDocument uiDocument;
    private VisualElement challengeProcessContainer;
    public Sprite[] challengeResultSprites;
    public VisualTreeAsset[] challengeProcessAssets;
    private Button closeButton;

    private BaseChallengeUIController[] controllers =
    {
        new ChallengeSelectUIController(),
        new BaseChallengeUploadUIController2(),
        new ChallengeCompleteUIController2(),
    };

    private BaseChallengeUIController _currentBaseChallengeUIController;
    private EChallengeProcess currentProcess;

    public ChallengeType currentChallengeType { get; set; } = ChallengeType.None;
    public bool isChallengeSuccess = false;

    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        closeButton = uiDocument.rootVisualElement.Q<Button>("btn_CloseChallenge");
        closeButton.clicked += () => { uiDocument.enabled = false; };

        challengeProcessContainer = uiDocument.rootVisualElement.Q<VisualElement>("ChallengeContainer");

        for (int i = 0; i < challengeProcessAssets.Length; i++)
        {
            controllers[i]
                .Initialize(
                    challengeProcessAssets[i]
                        .CloneTree(),
                    this
                );
        }

        GoToProcess(EChallengeProcess.SelectChallenge);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GoToProcess(EChallengeProcess.SelectChallenge);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GoToProcess(EChallengeProcess.ChallengeUploadImage);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GoToProcess(EChallengeProcess.ChallengeComplete);
        }
    }

    public void GoToProcess(EChallengeProcess process)
    {
        currentProcess = process;
        switch (process)
        {
            case EChallengeProcess.SelectChallenge:
                isChallengeSuccess = false;
                currentChallengeType = ChallengeType.None;
                _currentBaseChallengeUIController = controllers[(int)process];
                break;
            case EChallengeProcess.ChallengeUploadImage:
                _currentBaseChallengeUIController = controllers[(int)process];
                break;
            case EChallengeProcess.ChallengeComplete:
                _currentBaseChallengeUIController = controllers[(int)process];
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        challengeProcessContainer.Clear();

        challengeProcessContainer.Add(_currentBaseChallengeUIController.Root);
        _currentBaseChallengeUIController.BindType(currentChallengeType);
    }

    public void TryChallenge(Camera userCamera = null)
    {
        if (userCamera == null) userCamera = Camera.main;
        OpenChallengeUI();
        SetCamera(userCamera);
    }

    #region Private Methods Block

    private void OpenChallengeUI()
    {
        currentProcess = EChallengeProcess.SelectChallenge;
        uiDocument.enabled = true;
        isChallengeSuccess = false;
        GoToProcess(currentProcess);
    }

    void CloseChallengeUI()
    {
        uiDocument.enabled = false;
    }

    private void SetCamera(Camera userCamera)
    {
        if (userCamera == null)
        {
            Debug.LogError("User Camera is null");
            return;
        }
    }

    #endregion
}