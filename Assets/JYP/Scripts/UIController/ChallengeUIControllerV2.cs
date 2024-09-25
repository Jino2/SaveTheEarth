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
    public Sprite[] challengeGuideSprites;
    public string[] challengeGuideTexts;
    
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
        if(uiDocument.enabled) return;
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
        audioSource.PlayOneShot(challengeResultSounds[index]);
    }

    public void PlayAnim()
    {
        var anim = currentPlayerCharacter.GetComponentInChildren<Animator>();
        if (anim != null)
            anim.SetTrigger("Happy");
    }


    #region Private Methods Block

    private void OpenChallengeUI()
    {
        uiDocument.enabled = true;
        closeButton = uiDocument.rootVisualElement.Q<Button>("btn_CloseChallenge");
        closeButton.clicked += CloseChallengeUI;
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
        StartCoroutine(PlayUIHideAnim());
        var cam = Camera.main;
        cam.cullingMask = ~0;
        cam.GetUniversalAdditionalCameraData().renderPostProcessing = true;
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
        StartCoroutine(DelayCamOff());
    }

    private IEnumerator DelayCamOff()
    {
        yield return new WaitForSeconds(1.5f);
        var cam = Camera.main;
        cam.GetUniversalAdditionalCameraData().renderPostProcessing = false;
        cam.cullingMask = 1 << LayerMask.NameToLayer("Player");
    }

    private IEnumerator DelayCamOn()
    {
        yield return new WaitForSeconds(1.5f);
    }

    #endregion
}