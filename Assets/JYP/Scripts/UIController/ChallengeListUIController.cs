using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChallengeListUIController
{
    private ChallengeApi challengeApi = new ChallengeApi();
    public string userId = "test";
    private ScrollView challengeScrollView;

    // Start is called before the first frame update
    private List<ChallengeInfo> challengeInfos;

    public void InitList(VisualElement root, VisualTreeAsset itemTemplate)
    {
        challengeScrollView = root.Q<ScrollView>("ChallengeListScrollView");

        
        challengeApi.GetChallengeListByUserId(userId, (list) =>
        {
            Debug.Log($"Hello");

            challengeInfos = list;
            challengeScrollView.Clear();
            AddItems(challengeInfos, itemTemplate);
        });
    }

    private void AddItems(List<ChallengeInfo> challengeInfos, VisualTreeAsset itemTemplate)
    {
        for (int i = 0; i < challengeInfos.Count; i++)
        {
            var newListEntry = itemTemplate.Instantiate();
            var sellingItemUIController = new ChallengeItemUIController();
            newListEntry.userData = sellingItemUIController;

            sellingItemUIController.Initialize(newListEntry);
            sellingItemUIController.SetItemData(challengeInfos[i]);
        }
    }
}