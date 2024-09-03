using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class ChallengeListUIController
{
    private ChallengeApi challengeApi = new ChallengeApi();
    public string userId = "test";
    private ScrollView challengeScrollView; 
    private  Action<ChallengeInfo> onChallengeClicked;

    // Start is called before the first frame update
    private List<ChallengeInfo> challengeInfos;

    public void InitList(VisualElement root, VisualTreeAsset itemTemplate, Action<ChallengeInfo> onChallengeClicked)
    {
        challengeScrollView = root.Q<ScrollView>("ChallengeScrollView");
        this.onChallengeClicked = onChallengeClicked;
        
        challengeApi.GetChallengeListByUserId(userId, (list) =>
        {
            Debug.Log($"Hello");

            challengeInfos = list;
            challengeScrollView.Clear();
            AddItems(challengeInfos, itemTemplate);
            // challengeScrollView.
        });
    }

    private void AddItems(List<ChallengeInfo> challengeInfos, VisualTreeAsset itemTemplate)
    {
        for (int i = 0; i < challengeInfos.Count*2; i++)
        {
            var newListEntry = itemTemplate.Instantiate();
            var sellingItemUIController = new ChallengeItemUIController();
            newListEntry.userData = sellingItemUIController;

            sellingItemUIController.Initialize(newListEntry);
            sellingItemUIController.SetItemData(challengeInfos[i/2], onChallengeClicked);
            challengeScrollView.Add(newListEntry);
        }
    }
}