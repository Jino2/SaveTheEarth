using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public GameObject waterSplash; // Water Splash ������Ʈ
    public PhaseController phaseController; // PhaseController ��ũ��Ʈ ����

    public int activateScore = 50; // Water Splash Ȱ��ȭ ����
    public int deactivateScore = 150; // Water Splash ��Ȱ��ȭ ����

    private void Update()
    {
        // PhaseController�� ���� ������ ������
        int currentScore = phaseController.currentScore;

        // ���� ������ ���� Water Splash ������Ʈ Ȱ��ȭ/��Ȱ��ȭ
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