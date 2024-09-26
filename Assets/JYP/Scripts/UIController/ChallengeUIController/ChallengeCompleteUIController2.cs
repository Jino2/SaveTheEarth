using UnityEngine.UIElements;

public class ChallengeCompleteUIController2 : BaseChallengeUIController
{
    private Label title;
    private VisualElement challengeResultImage;
    private Label challengeFinishMessage;
    private Button tryAgainButton;
    private Button goToChallengeSelectionButton;

    private const string ChallengeSuccessMessage = "챌린지를 성공적으로 완료하였습니다!";
    private const string ChallengeFailedMessage = "챌린지를 실패하였습니다.. 다른 이미지를 업로드해보세요!";


    public override void Initialize(VisualElement root, ChallengeUIControllerV2 parentController)
    {
        base.Initialize(root, parentController);
        title = root.Q<Label>("lbl_ChallengeFinishTitle");
        challengeResultImage = root.Q<VisualElement>("image_ChallengeResult");
        challengeFinishMessage = root.Q<Label>("lbl_challengeFinishMessage");
        tryAgainButton = root.Q<Button>("btn_challengeAgain");
        goToChallengeSelectionButton = root.Q<Button>("btn_backToChallenges");
        
        tryAgainButton.clicked += () =>
        {
            parentController.GoToProcess(ChallengeUIControllerV2.EChallengeProcess.ChallengeUploadImage);
        };
        
        goToChallengeSelectionButton.clicked += () =>
        {
            parentController.GoToProcess(ChallengeUIControllerV2.EChallengeProcess.SelectChallenge);
        };
    }

    public override void BindType(ChallengeType type)
    {
        if (parentController.isChallengeSuccess)
        {
            parentController.OnChallengeSuccess();
            ShowSuccessUI();
        }
        else
        {
            ShowFailedUI();
        }
    }

    private void ShowSuccessUI()
    {
        tryAgainButton.style.display = DisplayStyle.None;
        title.text = "챌린지 성공!";
        challengeFinishMessage.text = ChallengeSuccessMessage;
        challengeResultImage.style.backgroundImage = parentController.challengeResultSprites[0].texture;
    }

    private void ShowFailedUI()
    {
        tryAgainButton.style.display = DisplayStyle.Flex;
        title.text = "챌린지 실패..";
        challengeFinishMessage.text = ChallengeFailedMessage;

        challengeResultImage.style.backgroundImage = parentController.challengeResultSprites[1].texture;
    }
}