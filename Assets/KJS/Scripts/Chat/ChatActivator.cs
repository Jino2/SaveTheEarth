using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

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

    public Vector2 panelPosition = new Vector2(-500f, 0f); // 생성될 패널의 좌표 (X: -1426, Y: -140)

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
                    ToggleUI(); // UI 상태를 활성화
                }
            }
            else
            {
                pressEText.gameObject.SetActive(false);
            }

            // Esc 키를 누르면 UI 비활성화
            if (isUIActive && Input.GetKeyDown(KeyCode.Escape))
            {
                DeactivateUI(); // UI 비활성화
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
        if (!isUIActive) // UI가 비활성화 상태일 때만 활성화
        {
            isUIActive = true;

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

                        // 패널의 위치를 X: -1426, Y: -140에 설정
                        SetPanelPosition();
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

            if (playerMoveScript != null)
            {
                playerMoveScript.enabled = false; // PlayerMoveScript 비활성화
            }
        }
    }

    // 패널을 X: -1426, Y: -140 위치에 배치하는 함수
    void SetPanelPosition()
    {
        if (uiInstance != null)
        {
            RectTransform rectTransform = uiInstance.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                // 캔버스의 가운데를 기준으로 X: -1426, Y: -140 위치로 설정
                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

                // 패널의 포지션을 설정 (X: -1426, Y: -140)
                rectTransform.anchoredPosition = panelPosition;

                // 피벗을 중앙으로 설정 (필요에 따라 피벗을 변경 가능)
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
            }
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