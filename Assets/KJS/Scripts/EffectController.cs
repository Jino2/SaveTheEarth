using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public GameObject waterSplash; // Water Splash 오브젝트
    public PhaseController phaseController; // PhaseController 스크립트 참조

    public int activateScore; // 첫 번째 변수: Water Splash 활성화 스코어
    public int deactivateScore; // 첫 번째 변수: Water Splash 비활성화 스코어

    public int secondaryActivateScore; // 두 번째 변수: Water Splash 활성화 스코어
    public int secondaryDeactivateScore; // 두 번째 변수: Water Splash 비활성화 스코어

    public int tertiaryActivateScore; // 세 번째 변수: Water Splash 활성화 스코어
    public int tertiaryDeactivateScore; // 세 번째 변수: Water Splash 비활성화 스코어

    private void Update()
    {
        // 현재 점수를 가져옴
        int currentScore = phaseController.currentScore;

        // 첫 번째, 두 번째, 세 번째 범위에 따라 Water Splash 오브젝트 활성화/비활성화
        bool shouldActivate = false;

        // 첫 번째 범위 체크
        if (currentScore >= activateScore && currentScore < deactivateScore)
        {
            shouldActivate = true;
        }
        // 두 번째 범위 체크
        else if (currentScore >= secondaryActivateScore && currentScore < secondaryDeactivateScore)
        {
            shouldActivate = true;
        }
        // 세 번째 범위 체크
        else if (currentScore >= tertiaryActivateScore && currentScore < tertiaryDeactivateScore)
        {
            shouldActivate = true;
        }

        // 활성화 여부에 따라 Water Splash를 켜거나 끔
        if (shouldActivate)
        {
            if (!waterSplash.activeInHierarchy)
            {
                waterSplash.SetActive(true);
            }
        }
        else
        {
            if (waterSplash.activeInHierarchy)
            {
                waterSplash.SetActive(false);
            }
        }
    }
}