using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.Networking;

public class ChallengeApi
{
    private static readonly string BASE_URL = "http://54.180.232.125/challenges";
    private static readonly string CHALLENGE_AI_URL = "https://1bd2-222-103-183-137.ngrok-free.app";

    public void GetChallengeListByUserId(string userId, Action<List<ChallengeInfo>> onComplete)
    {
        string url = $"{BASE_URL}/challenges/{userId}";
        // return HTTPManager.GetInstance()
        //     .Get();
        onComplete(
            new List<ChallengeInfo>()
            {
                new ChallengeInfo("이동", ChallengeType.Transport, "보상 1", ChallengeStatus.NotStarted),
                new ChallengeInfo("텀블러 사용", ChallengeType.Tumbler, "보상 2", ChallengeStatus.InProgress),
                new ChallengeInfo("재활용", ChallengeType.Recycle, "보상 3", ChallengeStatus.Completed),
            }
        );
    }

    public void TryChallengeTumbler(string userId, string imagePath, Action<string> onComplete)
    {
        Debug.Log($"path: {imagePath}");
        string url = $"{CHALLENGE_AI_URL}/tumbler-challenge";
        var data = File.ReadAllBytes(imagePath);
        Debug.Log($"data: {data.Length}");
        var multipartForm = new List<IMultipartFormSection>
        {
            new MultipartFormFileSection ("file", data, "image.jpg", "multipart/form-data")
        };
        var info = new HttpRequestInfo<List<IMultipartFormSection>, string>
        {
            url = url,
            requestBody = multipartForm,
            onSuccess = (t) => { OnTryChallengeSuccess(userId, 1, onComplete); },
            onError = () => { }
        };

        HTTPManager.GetInstance()
            .UploadMultipart(info);
    }

    public void TryChallengeTransport(string userId, string imagePath, Action<string> onComplete)
    {
        string url = $"{CHALLENGE_AI_URL}/transport-challenge";

        var multipartForm = new List<IMultipartFormSection>
        {
            new MultipartFormFileSection("image", imagePath)
        };
        var info = new HttpRequestInfo<List<IMultipartFormSection>, string>
        {
            url = url,
            requestBody = multipartForm,
            onSuccess = (t) => { OnTryChallengeSuccess(userId, 0, onComplete); },
            onError = () => { }
        };

        HTTPManager.GetInstance()
            .UploadMultipart(info);
    }

    private void OnTryChallengeSuccess(string userId, int challengeId, Action<string> onSuccess)
    {
        //do job
        onSuccess("success");
        
    }
}