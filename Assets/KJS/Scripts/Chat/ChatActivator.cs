using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;
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

    private TMP_InputField chatInputField; // TMP_InputField 참조

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

                // 현재 선택된 오브젝트가 없고, E키를 눌렀을 때만 실행
                if (Input.GetKeyDown(KeyCode.E) && EventSystem.current.currentSelectedGameObject == null)
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

                        // 패널의 위치를 설정
                        SetPanelPosition();

                        // TMP_InputField 컴포넌트를 찾음
                        chatInputField = uiInstance.GetComponentInChildren<TMP_InputField>();

                        if (chatInputField != null)
                        {
                            // TMP_InputField 활성화 후 포커스
                            chatInputField.ActivateInputField();
                        }
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

                if (chatInputField != null)
                {
                    chatInputField.ActivateInputField(); // TMP_InputField 활성화 후 포커스
                }
            }

            if (playerMoveScript != null)
            {
                playerMoveScript.enabled = false; // PlayerMoveScript 비활성화
            }
        }
    }

    // 패널을 특정 위치에 배치하는 함수
    void SetPanelPosition()
    {
        if (uiInstance != null)
        {
            RectTransform rectTransform = uiInstance.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                // 캔버스의 가운데를 기준으로 위치 설정
                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                rectTransform.anchoredPosition = panelPosition;
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
            }
        }
    }

    void DeactivateUI()
    {
        isUIActive = false;

        // UI를 비활성화
        if (uiInstance != null)
        {
            uiInstance.SetActive(false); // UI 패널 비활성화
        }

        // TMP_InputField 비활성화
        if (chatInputField != null)
        {
            chatInputField.DeactivateInputField(); // 입력 필드 비활성화
            chatInputField.text = ""; // 필요하다면 입력된 텍스트를 초기화 (선택 사항)
        }

        // UI 포커스를 해제하여 게임으로 포커스가 돌아가게 함
        EventSystem.current.SetSelectedGameObject(null);

        // 플레이어 이동 스크립트 다시 활성화
        if (playerMoveScript != null)
        {
            playerMoveScript.enabled = true; // 플레이어 이동 활성화
        }
    }
}