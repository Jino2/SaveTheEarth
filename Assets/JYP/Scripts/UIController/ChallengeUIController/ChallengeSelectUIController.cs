using System;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using Image = UnityEngine.UIElements.Image;

public class ChallengeSelectUIController : BaseChallengeUIController
{
    private Label title;
    private Label description;
    private Image icon;
    private Button uploadButton;
    private Button completeButton;
    public override void Initialize(VisualElement root, ChallengeUIControllerV2 parentController)
    {
        base.Initialize(root, parentController);
        title = root.Q<Label>("title");
        description = root.Q<Label>("description");
        icon = root.Q<Image>("icon");
        uploadButton = root.Q<Button>("uploadButton");
    }

    public override void BindType(ChallengeType type)
    {
        
    }
}