using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;
using UnityEngine.UIElements;

public class ChallengeItemUIController : MonoBehaviour
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

    public void SetItemData(ChallengeInfo item)
    {
        challengeInfo = item;
        challengeNameLabel.text = item.name;
        challengeRewardLabel.text = item.reward;
        SetChallengeButton();
    }

    private void SetChallengeButton()
    {
        switch (challengeInfo.status)
        {
            case ChallengeStatus.NotStarted:
                challengeButton.text = "도전하기";
                challengeButton.clicked += StartChallenge;
 
                break;
            case ChallengeStatus.InProgress:
                challengeButton.text = "인증하기";
                challengeButton.clicked += VerifyChallenge;
                break;
            case ChallengeStatus.Completed:
                challengeButton.text = "완료";
                challengeButton.clicked += null;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void VerifyChallenge()
    {
        
    }

    private void StartChallenge()
    {
    }
}
