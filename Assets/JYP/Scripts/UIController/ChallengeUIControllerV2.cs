using UnityEngine;
using UnityEngine.UIElements;

public class ChallengeUIControllerV2 : MonoBehaviour
{
    public UIDocument uiDocument;
    private Button closeButton;
    private ScrollView challengeScrollView;
    private Button recycleChallengeButton;
    private Button subwayChallengeButton;
    private Button tumblerChallengeButton;
    private VisualElement challengeImageSelectionContainer;
    private VisualElement challengeSelectionContainer;
    private VisualElement challengeCompleteContainer;

    private ChallengeType currentChallengeType = ChallengeType.None;

    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        closeButton = uiDocument.rootVisualElement.Q<Button>("btn_CloseChallenge");
        recycleChallengeButton = uiDocument.rootVisualElement.Q<Button>("btn_recycleChallenge");
        subwayChallengeButton = uiDocument.rootVisualElement.Q<Button>("btn_busChallenge");
        tumblerChallengeButton = uiDocument.rootVisualElement.Q<Button>("btn_tumblerChallenge");
        challengeScrollView = uiDocument.rootVisualElement.Q<ScrollView>("ScrollView_Challenge");
        challengeImageSelectionContainer =
            uiDocument.rootVisualElement.Q<VisualElement>("Container_ChallengeCompletion");
        challengeSelectionContainer = uiDocument.rootVisualElement.Q<VisualElement>("Container_ChallengeSelection");
        challengeCompleteContainer = uiDocument.rootVisualElement.Q<VisualElement>("Container_ChallengeCompletion");

        closeButton.clicked += () => { HideChallengeUI(); };
        recycleChallengeButton.clicked += () => OnChallengeButtonClicked(ChallengeType.Recycle);
        subwayChallengeButton.clicked += () => OnChallengeButtonClicked(ChallengeType.Transport);
        tumblerChallengeButton.clicked += () => OnChallengeButtonClicked(ChallengeType.Tumbler);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ShowChallengeSelectionUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ShowChallengeImageSelectionUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ShowChallengeCompleteUI();
        }
    }

    private void HideChallengeUI()
    {
        if (!uiDocument.enabled) return;
        uiDocument.enabled = false;
    }

    private void OnChallengeButtonClicked(ChallengeType challengeType)
    {
        //Go to scroll to next step
        currentChallengeType = challengeType;
    }

    private void ShowChallengeImageSelectionUI()
    {
        //Show Challenge Image Selection UI

        challengeScrollView.horizontalScroller.value = 600f;
    }

    private void ShowChallengeSelectionUI()
    {
        //Show Challenge Selection UI
        challengeScrollView.horizontalScroller.value = 0f;
    }

    private void ShowChallengeCompleteUI()
    {
        challengeScrollView.horizontalScroller.value = challengeScrollView.horizontalScroller.highValue;
    }
}