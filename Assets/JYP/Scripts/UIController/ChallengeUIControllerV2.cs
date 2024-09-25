using System;
using Cinemachine;
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

    public ButtonInteractiveObject interactiveObject;
    public UIDocument uiDocument;
    public CinemachineVirtualCamera challengeUICamera;
    private VisualElement challengeProcessContainer;
    public Sprite[] challengeResultSprites;
    public VisualTreeAsset[] challengeProcessAssets;
    private Button closeButton;
    public Sprite[] challengeTypeSprites;
    private GameObject currentPlayerCharacter;
    public AudioClip[] challengeResultSounds;
    public AudioSource audioSource;
    
    private readonly BaseChallengeUIController[] controllers =
    {
        new ChallengeSelectUIController(),
        new BaseChallengeUploadUIController2(),
        new ChallengeCompleteUIController2(),
    };

    private BaseChallengeUIController _currentBaseChallengeUIController;
    private EChallengeProcess currentProcess;
    private bool isTryingChallenge = false;

    public ChallengeType currentChallengeType { get; set; } = ChallengeType.None;
    public bool isChallengeSuccess = false;

    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
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
        _currentBaseChallengeUIController.Root.style.width = new StyleLength(new Length(100, LengthUnit.Percent));
        print($"{_currentBaseChallengeUIController.Root.style.width.value.value}");
        challengeProcessContainer.Add(_currentBaseChallengeUIController.Root);
        _currentBaseChallengeUIController.BindType(currentChallengeType);
    }

    public void TryChallenge()
    {
        currentPlayerCharacter = interactiveObject.InteractedObject;
        currentPlayerCharacter.TryGetComponent<PlayerMove>(out var pm);
        if (pm != null)
        {
            pm.controllable = false;
        }

        OpenChallengeUI();
        SetCamera(currentPlayerCharacter);
    }

    public void PlaySound(int index)
    {
        audioSource.clip = challengeResultSounds[index];
        audioSource.Play();
    }

    #region Private Methods Block

    private void OpenChallengeUI()
    {
        uiDocument.enabled = true;
        closeButton = uiDocument.rootVisualElement.Q<Button>("btn_CloseChallenge");
        closeButton.clicked += CloseChallengeUI;
        challengeProcessContainer = uiDocument.rootVisualElement.Q<VisualElement>("ChallengeContainer");
        for (int i = 0; i < challengeProcessAssets.Length; i++)
        {
            print(controllers[i]);
            print(challengeProcessAssets[i]);
            var t = challengeProcessAssets[i]
                .CloneTree();
            controllers[i]
                .Initialize(
                    t,
                    this
                );
            print(t);
        }

        isTryingChallenge = true;
        GoToProcess(EChallengeProcess.SelectChallenge);
    }

    void CloseChallengeUI()
    {
        currentPlayerCharacter.TryGetComponent<PlayerMove>(out var pm);
        if (pm != null)
        {
            pm.controllable = true;
        }

        uiDocument.enabled = false;
        closeButton.clicked -= CloseChallengeUI;
        challengeUICamera.gameObject.SetActive(false);
        isTryingChallenge = false;
    }

    private void SetCamera(GameObject playerCharacter)
    {
        challengeUICamera.LookAt = playerCharacter.transform;
        challengeUICamera.gameObject.SetActive(true);
    }

    private GameObject GetCurrentPlayerCharacter()
    {
        //TODO: implement with Photon 
        return GameObject.FindWithTag("Player");
    }

    #endregion
}