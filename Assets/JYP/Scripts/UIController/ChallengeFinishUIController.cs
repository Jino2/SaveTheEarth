using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class ChallengeFinishUIController : MonoBehaviour
{
    public UIDocument uiDocument;

    public UnityEvent onChallengeAgainButtonClicked;
    public UnityEvent onOtherChallengesButtonClicked;

    private Button closeButton;
    private Button challengeAgainButton;
    private Button otherChallengesButton;
    private Label resultTitleText;
    private Label resultMessageText;


    private string title = "도전 결과";
    private string message = "도전 결과 메시지";

    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
    }

    public void ShowUIWith(bool isSuccess)
    {
        if (isSuccess)
        {
            title = "인증 완료!";
            message = "이미지를 정상적으로 등록했습니다.\n<b>오늘도 지구를 지키셨네요!</b>";
        }
        else
        {
            title = "인증 실패";
            message = "이미지가 인식되지 않았습니다.\n<b>다시 한번 지구를 지켜볼까요?</b>";
        }

        ShowUI();
    }

    public void ShowUI()
    {
        if (uiDocument.enabled) return;
        uiDocument.enabled = true;
        closeButton = uiDocument.rootVisualElement.Q<Button>("btn_Close");
        challengeAgainButton = uiDocument.rootVisualElement.Q<Button>("btn_challengeAgain");
        otherChallengesButton = uiDocument.rootVisualElement.Q<Button>("btn_backToChallenges");
        resultTitleText = uiDocument.rootVisualElement.Q<Label>("lbl_ChallengeFinishTitle");
        resultMessageText = uiDocument.rootVisualElement.Q<Label>("lbl_challengeFinishMessage");

        closeButton.clicked += OnCloseButtonClicked;
        challengeAgainButton.clicked += () =>
        {
            uiDocument.enabled = false;
            onChallengeAgainButtonClicked.Invoke();
        };
        otherChallengesButton.clicked += () =>
        {
            uiDocument.enabled = false;
            onOtherChallengesButtonClicked.Invoke();
        };
        resultMessageText.text = message;
        resultTitleText.text = title;
    }

    private void OnCloseButtonClicked()
    {
        uiDocument.enabled = false;
    }
}