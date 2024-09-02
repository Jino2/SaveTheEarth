using System;
using System.Collections.Generic;

public class ChallengeApi
{
    private static readonly string BASE_URL = "https://api.example.com";
    
    public void GetChallengeListByUserId(string userId, Action<List<ChallengeInfo>> onComplete)
    {
        
        string url = $"{BASE_URL}/challenges/{userId}";
        // return HTTPManager.GetInstance()
        //     .Get();
        onComplete(
            new List<ChallengeInfo>()
            {
                new ChallengeInfo( "챌린지 1", ChallengeType.A,  "보상 1", ChallengeStatus.NotStarted),
                new ChallengeInfo( "챌린지 2", ChallengeType.B,  "보상 2", ChallengeStatus.InProgress),
                new ChallengeInfo( "챌린지 3", ChallengeType.A,  "보상 3", ChallengeStatus.Completed),
            }
        );
    }
}