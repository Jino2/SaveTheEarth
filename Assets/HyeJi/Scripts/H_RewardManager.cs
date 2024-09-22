using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChatInfo;

public class H_RewardManager : MonoBehaviour
{
    // 미션 완료 여부 체크
    private bool missionCompleted = false;
    public int trashCount = 0;
    public int clearThreshold = 5;
    public int rewardPoint = 10;

    // AI URL
    private string aiUrl;
    // 챗봇 타입을 저장
    public ChatInfo.ChatType chatType;

    private void Start()
    {
        // 선택된 챗봇 타입에 따른 URL 설정
        aiUrl = ChatInfo.GetApiUrl(chatType);
    }

    public void AddTrashCount()
    {
        trashCount++;

        // 플레이어가 trash 오브젝트를 5개 이상 줍고
        if (trashCount >= clearThreshold)
        {
            ClearEvent();
        }
    }
    void ClearEvent()
    {
        print("쓰줍 완료");

        // UserCache 에서 정보 가져오기 
        UserCache userCache = UserCache.GetInstance();
        string userid = userCache.Id;

        // AI 서버에 메시지 전송
        SendMessageToAI(userid, "쓰레기 5개를 주웠습니다.", trashCount, () =>
        {
            // AI 메시지 전송 성공 시, 보상 포인트 지급
            UserApi.AddPoint(userid, rewardPoint, updatedUserInfo =>
            {
                Debug.Log($"보상 {rewardPoint} 포인트 적용 완료, 사용자 총 포인트: {updatedUserInfo.point}");
            });
        });
    }

    public void SendMessageToAI(string userid, string userMessage, int trashCount = 0, Action onSuccess = null)
    {
        string requestUrl = $"{aiUrl}?user_id={userid}"; // user_id를 쿼리 파라미터로 추가

        // AI 요청 객체 생성
        var aiRequest = new AIRequest
        {
            // 사용자 메시지 설정
            user_id = userid,
            user_message = userMessage,
            //trash_count = trashCount
        };


        var requestInfo = new HttpRequestInfo<Dictionary<string, string>, string>()
        {
            url = requestUrl,
            contentType = "application/x-www-form-urlencoded",
            requestBody = new Dictionary<string, string>
            {
                { "user_message", userMessage },
                { "user_id", userid },
                { "trash_count", Convert.ToString(trashCount)}
            },

            onSuccess = (response) =>
            {
                Debug.Log("서버 전송 성공");
                // 성공 시 보상 처리로 넘어가라
                onSuccess?.Invoke();

            },
            onError = () =>
            {
                Debug.Log("서버 전송 실패");
            }
        };
        HTTPManager.GetInstance().PostWWWForm(requestInfo);
    }
}
