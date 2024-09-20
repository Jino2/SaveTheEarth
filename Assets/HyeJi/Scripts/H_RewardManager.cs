using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_RewardManager : MonoBehaviour
{
    private int trashCount = 0;
    public int clearThreshold = 5;

    public void AddTrashCount()
    {
        trashCount++;

        // 플레이어가 trash 오브젝트를 5개 이상 줍고
        if( trashCount >= clearThreshold)
        {
            ClearEvent();
        }
        // 동물 ai에게 대화를 시도시
        // 클리어 이벤트 발생
    }

    void ClearEvent()
    {
        print("쓰줍 완료");
    }


    public void SendMessageToAI(string clearMent)
    {
        
    }
}
