using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;
using UnityEngine.UIElements;

public class ChallengeItemUIController
{
    private Label challengeNameLabel;
    private VisualElement challengeImage;
    private Label challengeRewardLabel;
    private Button challengeButton;
    
    private ChallengeInfo challengeInfo;
    
    public void Initialize(VisualElement root)
    {
        challengeNameLabel = root.Q<Label>("ChallengeNameLabel");
        challengeImage = root.Q<VisualElement>("ChallengeTypeContainer");
        challengeRewardLabel = root.Q<Label>("ChallengeRewardLabel");
        challengeButton = root.Q<Button>("ChallengeOrAuthButton");
    }

    public void SetItemData(ChallengeInfo item, Action<ChallengeInfo> onChallenge)
    {
        challengeInfo = item;
        challengeNameLabel.text = item.name;
        challengeRewardLabel.text = item.reward;
        challengeButton.clicked += () => onChallenge(item);
        SetChallengeButton();
    }

    private void SetChallengeButton()
    {
        switch (challengeInfo.status)
        {
            case ChallengeStatus.NotStarted:
                challengeButton.text = "도전하기";
 
                break;
            case ChallengeStatus.InProgress:
                challengeButton.text = "인증하기";
                break;
            case ChallengeStatus.Completed:
                challengeButton.text = "완료";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
