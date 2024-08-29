using System.Collections.Generic;

public class ChallengeApi
{
    private static readonly string BASE_URL = "https://api.example.com";
    
    public static List<ChallengeInfo> GetChallengeListByUserId(string userId)
    {
        
        string url = $"{BASE_URL}/challenges/{userId}";
        return HttpHelper.Get(url);
    }

}