using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class ChallengeSelectUIController : BaseChallengeUIController
{
    private Button transportChallengeButton;
    private Button tumblerChallengeButton;
    private Button recycleChallengeButton;

    public override void Initialize(VisualElement root, ChallengeUIControllerV2 parentController)
    {
        base.Initialize(root, parentController);
        transportChallengeButton = root.Q<Button>("btn_transportChallenge");
        tumblerChallengeButton = root.Q<Button>("btn_tumblerChallenge");
        recycleChallengeButton = root.Q<Button>("btn_recycleChallenge");
        
        transportChallengeButton.clicked += () => OnChallengeButtonClicked(ChallengeType.Transport);
        tumblerChallengeButton.clicked += () => OnChallengeButtonClicked(ChallengeType.Tumbler);
        recycleChallengeButton.clicked += () => OnChallengeButtonClicked(ChallengeType.Recycle);
    }

    public override void BindType(ChallengeType type)
    {
        
    }
    
    private void OnChallengeButtonClicked(ChallengeType type)
    {
        parentController.currentChallengeType = type;
        parentController.GoToProcess(ChallengeUIControllerV2.EChallengeProcess.ChallengeUploadImage);
    }
}