using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
public class ChatActivator : MonoBehaviourPun
{
    public float activationDistance = 5f; // 활성화할 거리
    private GameObject player; // 플레이어 오브젝트
    public GameObject uiObject; // 활성화/비활성화할 UI 오브젝트
    public TextMeshProUGUI pressEText; // "Press E" 텍스트를 표시할 UI 오브젝트
    private bool isUIActive = false; // UI가 활성화되어 있는지 여부

    void Start()
    {
        // 태그가 "Player"인 오브젝트를 찾음
        player = GameObject.FindGameObjectWithTag("Player");

        // 처음에 UI 오브젝트와 텍스트를 비활성화
        if (uiObject != null)
        {
            uiObject.SetActive(false);
        }

        if (pressEText != null)
        {
            pressEText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (player != null && uiObject != null && pressEText != null)
        {
            // 현재 오브젝트와 플레이어 사이의 거리 계산
            float distance = Vector3.Distance(transform.position, player.transform.position);

            // 플레이어가 일정 거리 내에 있으면 E 키 입력을 감지
            if (distance <= activationDistance)
            {
                // "Press E" 텍스트를 활성화
                pressEText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    // UI 활성화/비활성화 전환
                    isUIActive = !isUIActive;
                    uiObject.SetActive(isUIActive);
                }
            }
            else
            {
                // 플레이어가 거리를 벗어나면 UI와 "Press E" 텍스트를 비활성화
                pressEText.gameObject.SetActive(false);

                if (isUIActive)
                {
                    isUIActive = false;
                    uiObject.SetActive(false);
                }
            }
        }
    }
}