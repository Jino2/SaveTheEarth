using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class ChatActivator : MonoBehaviourPun
{
    public float activationDistance = 5f; // UI를 활성화할 거리
    public string uiPrefabPath = "ChatPanel"; // Resources 폴더 내 패널 프리팹 경로
    private GameObject uiInstance; // 동적으로 생성된 UI 인스턴스
    public TextMeshProUGUI pressEText; // "Press E" 텍스트를 표시할 UI 오브젝트
    private bool isUIActive = false; // UI가 활성화되어 있는지 여부
    public Transform playerTransform; // 플레이어의 Transform
    private CharacterController characterController; // 캐릭터 컨트롤러 참조
    private MonoBehaviour playerMoveScript; // 플레이어 이동 스크립트 참조

    public Vector2 panelPosition = new Vector2(-457, 383); // 패널 위치 설정값

    void Start()
    {
        uiInstance = null;

        if (pressEText != null)
        {
            pressEText.gameObject.SetActive(false);
        }

        // 로컬 플레이어의 Transform과 이동 스크립트를 찾기 위한 코루틴 실행
        StartCoroutine(FindLocalPlayer());
    }

    void Update()
    {
        if (playerTransform != null && pressEText != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);

            if (distance <= activationDistance)
            {
                pressEText.gameObject.SetActive(true); // "Press E" 텍스트 표시

                if (Input.GetKeyDown(KeyCode.E)) // photonView.IsMine을 사용하지 않음
                {
                    ToggleUI(); // 모든 클라이언트에서 UI 상태 변경
                }
            }
            else
            {
                pressEText.gameObject.SetActive(false);

                if (isUIActive)
                {
                    DeactivateUI(); // 모든 클라이언트에서 UI 비활성화
                }
            }
        }
    }

    private IEnumerator FindLocalPlayer()
    {
        while (playerTransform == null)
        {
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                PhotonView pv = player.GetComponent<PhotonView>();

                if (pv != null && pv.IsMine)
                {
                    playerTransform = player.transform;
                    characterController = player.GetComponent<CharacterController>();
                    playerMoveScript = player.GetComponent<PlayerMove>();
                    break;
                }
            }

            yield return null;
        }
    }

    void ToggleUI()
    {
        isUIActive = !isUIActive;

        if (isUIActive)
        {
            if (uiInstance == null)
            {
                GameObject uiPrefab = Resources.Load<GameObject>(uiPrefabPath); // 프리팹 로드
                if (uiPrefab != null)
                {
                    // 씬에 있는 Canvas를 찾아서 그 하위에 패널을 생성
                    GameObject canvas = GameObject.Find("Canvas"); // 씬에 있는 Canvas를 찾음
                    if (canvas != null)
                    {
                        // 패널을 Canvas의 자식으로 설정하면서 인스턴스화
                        uiInstance = Instantiate(uiPrefab, canvas.transform);
                        uiInstance.GetComponent<RectTransform>().anchoredPosition = panelPosition;
                        Debug.Log($"Panel instantiated as a child of Canvas with position {panelPosition}");
                    }
                    else
                    {
                        Debug.LogError("Canvas not found in the scene!");
                    }
                }
                else
                {
                    Debug.LogError("UI Prefab not found in Resources folder!");
                }
            }
            else
            {
                uiInstance.SetActive(true); // 이미 생성된 UI를 활성화
            }
        }
        else
        {
            if (uiInstance != null)
            {
                uiInstance.SetActive(false); // UI를 비활성화
            }
        }

        if (playerMoveScript != null)
        {
            playerMoveScript.enabled = !isUIActive; // PlayerMoveScript 비활성화/활성화
        }
    }

    void DeactivateUI()
    {
        isUIActive = false;

        if (uiInstance != null)
        {
            uiInstance.SetActive(false); // UI 비활성화
        }

        if (playerMoveScript != null)
        {
            playerMoveScript.enabled = true; // 플레이어 이동 활성화
        }
    }
}