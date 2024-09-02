using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public GameObject waterSplash; // Water Splash 오브젝트
    public PhaseController phaseController; // PhaseController 스크립트 참조

    public int activateScore = 50; // Water Splash 활성화 점수
    public int deactivateScore = 150; // Water Splash 비활성화 점수

    private void Update()
    {
        // PhaseController의 현재 점수를 가져옴
        int currentScore = phaseController.currentScore;

        // 현재 점수에 따라 Water Splash 오브젝트 활성화/비활성화
        if (currentScore >= activateScore && currentScore < deactivateScore)
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