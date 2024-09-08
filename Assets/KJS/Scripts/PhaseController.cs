using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SocialPlatforms.Impl;

public class PhaseController : MonoBehaviour
{
    // 트리거가 발생할 점수입니다.
    public int triggerScore = 100;

    [SerializeField]
    // 현재 점수를 추적하는 변수입니다.
    public int currentScore = 0;

    public event Action OnUpdateChildObjects;

    private void Start()
    {
        DisableEnvironmentObjects();

        UserApi.GetUserInfo("test", info =>
        {
            currentScore = info.point;
            // 점수가 특정 포인트에 도달하면 자식 오브젝트들의 상태를 변경합니다.
            if (currentScore >= triggerScore)
            {
                UpdateChildObjects();
            }
        });
    }

    private void DisableEnvironmentObjects()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("environment"))
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    // 점수 1 증가
        //    currentScore += 1;
        //    // 점수 UI 업데이트

        //}
        //if (currentScore >= triggerScore)
        //{
        //    UpdateChildObjects();
        //}
    }

    // 자식 오브젝트들의 상태를 변경하는 메서드입니다.
    public void UpdateChildObjects()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("trash"))
            {
                // 'trash' 태그가 있는 오브젝트는 활성화합니다.
                child.gameObject.SetActive(false);
            }
            else if (child.CompareTag("environment"))
            {
                // 'environment' 태그가 있는 오브젝트는 비활성화합니다.
                child.gameObject.SetActive(true);
            }
            OnUpdateChildObjects?.Invoke();
        }
    }
}