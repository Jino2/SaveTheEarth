using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_RewardManager : MonoBehaviour
{
    public int trashCount = 0;
    public int clearThreshold = 5;

    // URL 추가
    public static string BASE_URL = "";

    public void AddTrashCount()
    {
        trashCount++;

        // 플레이어가 trash 오브젝트를 5개 이상 줍고
        if( trashCount >= clearThreshold)
        {
            ClearEvent();
        }
    }
    void ClearEvent()
    {
        print("쓰줍 완료");

        // 유저 정보를 가져온 후 AI 서버로 데이터 보내기
        UserApi.GetUserInfo("Aquaman", userInfo => 
        {
            // 보내고 
            SendMessageToAI(userInfo.id, "쓰레기 5개를 주웠다.", trashCount);

            // 보상 추가
            UserApi.AddPoint(userInfo.id, userInfo.point, updatedUserInfo => 
            {
                print("보상받자");
            });
        });       
    }

    public void SendMessageToAI(string userid, string message, int trashCount)
    {
        // 파라미터로 전송하기
        H_PlayerInfo h_PlayerInfo = new H_PlayerInfo(userid, message, trashCount);

        // JSON 변환
        string jsonData = JsonUtility.ToJson(h_PlayerInfo);

        var requestInfo = new HttpRequestInfo<string, string>
        {
            url = BASE_URL,
            requestBody = jsonData,
            onSuccess = response => 
            {
                Debug.Log("서버 전송 성공");
            },
            onError = () => 
            {
                Debug.Log("서버 전송 실패");
            }
        };
        HTTPManager.GetInstance().Post(requestInfo);
    }  
}
