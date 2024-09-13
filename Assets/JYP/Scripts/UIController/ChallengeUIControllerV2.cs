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

    public ChallengeType currentChallengeType { get; private set; } = ChallengeType.None;
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
}