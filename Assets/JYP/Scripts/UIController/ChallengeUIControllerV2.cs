using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class ChallengeUIControllerV2 : MonoBehaviour
{
    public enum EChallengeProcess
    {
        SelectChallenge,
        ChallengeUploadImage,
        ChallengeComplete,
    }

    public ChallengeNPCChatController challengeNPCChatController;
    public ButtonInteractiveObject interactiveObject;
    public UIDocument uiDocument;
    public CinemachineVirtualCamera challengeUICamera;
    private VisualElement challengeProcessContainer;
    public Sprite[] challengeResultSprites;
    public VisualTreeAsset[] challengeProcessAssets;
    private Button closeButton;
    public Sprite[] challengeTypeSprites;
    private GameObject currentPlayerCharacter;
    public Texture2D[] challengeGuideTextures;
    public string[] challengeGuideTexts;
    public ChallengeFloor challengeFloor;
    
    private readonly BaseChallengeUIController[] controllers =
    {
        new ChallengeSelectUIController(),
        new BaseChallengeUploadUIController2(),
        new ChallengeCompleteUIController2(),
    };

    private BaseChallengeUIController _currentBaseChallengeUIController;
    private VisualElement rootContainer;
    private EChallengeProcess currentProcess;
    private bool isTryingChallenge = false;
    private LayerMask prevCullMask;
    public ChallengeType currentChallengeType { get; set; } = ChallengeType.None;
    public bool isChallengeSuccess = false;
    
    private Coroutine cameraSettingCoroutine;
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
        if (uiDocument.enabled) return;
        currentPlayerCharacter = interactiveObject.InteractedObject;
        currentPlayerCharacter.TryGetComponent<PlayerMove>(out var pm);
        if (pm != null)
        {
            pm.controllable = false;
        }

        OpenChallengeUI();
        SetCamera(currentPlayerCharacter);
    }

    
    public void OnChallengeSuccess()
    {
        isChallengeSuccess = true;
        PlaySuccessSound();
        StartCoroutine(DanceWhile15Seconds());
    }
    public void PlaySuccessSound()
    {
        SoundManager.Instance.PlaySfx(SoundManager.ESfxType.Success, currentPlayerCharacter.transform.position);
    }


    #region Private Methods Block
    
    private IEnumerator DanceWhile15Seconds()
    {
        SoundManager.Instance.PlayBgm(SoundManager.EBgmType.Dance);
        var anim = currentPlayerCharacter.GetComponentInChildren<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Happy");
            challengeFloor.TurnOnLights();
        }

        yield return new WaitForSeconds(15f);
        anim.SetTrigger("Idle");
        SoundManager.Instance.PlayBgm(SoundManager.EBgmType.Lobby);
        challengeFloor.TurnOffLights();
    }
    private void OpenChallengeUI()
    {
        challengeFloor.TurnOffLights();
        
        uiDocument.enabled = true;
        closeButton = uiDocument.rootVisualElement.Q<Button>("btn_CloseChallenge");
        challengeProcessContainer = uiDocument.rootVisualElement.Q<VisualElement>("ChallengeContainer");
        rootContainer = uiDocument.rootVisualElement.Q<VisualElement>("Container");

        PlayUIShowAnim();


        for (int i = 0; i < challengeProcessAssets.Length; i++)
        {
            var t = challengeProcessAssets[i]
                .CloneTree();
            controllers[i]
                .Initialize(
                    t,
                    this
                );
        }

        isTryingChallenge = true;
        GoToProcess(EChallengeProcess.SelectChallenge);
    }

    private void PlayUIShowAnim()
    {
        //play show anim
        rootContainer.RemoveFromClassList("hide_left");
    }

    private IEnumerator PlayUIHideAnim()
    {
        //play hide anim
        rootContainer.AddToClassList("hide_left");
        yield return new WaitForSeconds(1.5f);
        uiDocument.enabled = false;
    }

    void CloseChallengeUI()
    {
        StopAllCoroutines();
        if (isChallengeSuccess)
        {
            challengeNPCChatController.GreetPlayer();
        }
        var anim = currentPlayerCharacter.GetComponentInChildren<Animator>();
        anim.SetTrigger("Idle");
        challengeFloor.TurnOffLights();
        SoundManager.Instance.PlayBgm(SoundManager.EBgmType.Lobby);
        StartCoroutine(PlayUIHideAnim());
        var cam = Camera.main;
        cam.cullingMask = prevCullMask;
        cam.GetUniversalAdditionalCameraData()
            .renderPostProcessing = true;
        challengeUICamera.gameObject.SetActive(false);

        currentPlayerCharacter.TryGetComponent<PlayerMove>(out var pm);
        if (pm != null)
        {
            pm.controllable = true;
        }

        closeButton.clicked -= CloseChallengeUI;
        isTryingChallenge = false;
    }

    private void SetCamera(GameObject playerCharacter)
    {
        challengeUICamera.LookAt = playerCharacter.transform;

        challengeUICamera.gameObject.SetActive(true);
        cameraSettingCoroutine = StartCoroutine(DelayCamOff(
            () =>
            {
                closeButton.clicked += CloseChallengeUI;
            }
            ));
    }

    private IEnumerator DelayCamOff(Action onComplete = null)
    {
        yield return new WaitForSeconds(2.5f);
        var cam = Camera.main;
        cam.GetUniversalAdditionalCameraData()
            .renderPostProcessing = false;
        prevCullMask = cam.cullingMask;
        cam.cullingMask =  1<< LayerMask.NameToLayer("ChallengeFloor") | 1 << LayerMask.NameToLayer("Player");
        onComplete?.Invoke();
        
    }
    
    #endregion
}