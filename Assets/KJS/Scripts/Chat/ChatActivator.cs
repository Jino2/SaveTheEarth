using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;
using UnityEngine.AI;
public class ChatActivator : MonoBehaviourPun
{
    public float activationDistance = 5f;
    public string uiPrefabPath = "ChatPanel";
    private GameObject uiInstance;
    public TextMeshProUGUI pressEText;
    private bool isUIActive = false;
    public Transform playerTransform;
    private TMP_InputField chatInputField;

    public Mark markScript;  // Mark 스크립트를 참조하기 위한 변수 추가

    public Vector2 panelPosition = new Vector2(-500f, 0f);
    public Transform cameraTransform;  // 카메라 Transform 추가
    public float mouseSensitivity = 100f;  // 마우스 감도
    public float rotationSpeed = 5f;  // Lerp를 위한 회전 속도

    private Vector3 cameraInitialPosition;
    private Quaternion cameraInitialRotation;

    private PhotonView pv;
    private NPCMovement npcMovement;  // NPCMovement 스크립트 참조
    private NavMeshAgent navMeshAgent;  // NavMeshAgent 참조

    void Start()
    {
        pressEText?.gameObject.SetActive(false);
        StartCoroutine(FindLocalPlayer());
        Cursor.lockState = CursorLockMode.None;  // 마우스 커서 항상 보이도록 설정
        Cursor.visible = true;  // 커서 항상 보임
        npcMovement = GetComponent<NPCMovement>();  // NPCMovement 스크립트 가져오기
        navMeshAgent = GetComponent<NavMeshAgent>();  // NavMeshAgent 가져오기
    }

    void Update()
    {
        if (playerTransform == null || pressEText == null || pv == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // "Press E" 텍스트 활성화/비활성화 처리
        if (distance <= activationDistance)
        {
            pressEText.gameObject.SetActive(true);

            // E키를 눌렀을 때 UI를 활성화하거나 비활성화
            if (Input.GetKeyDown(KeyCode.E) && pv.IsMine && EventSystem.current.currentSelectedGameObject == null)
            {
                ToggleUI();
            }
        }
        else
        {
            pressEText.gameObject.SetActive(false);
        }

        // UI 활성화 여부와 상관없이 Escape 키를 감지해 UI를 비활성화
        if (isUIActive && Input.GetKeyDown(KeyCode.Escape))
        {
            DeactivateUI();
        }

        // UI가 활성화된 상태가 아닐 때만 마우스 입력을 처리
        if (!isUIActive && pv.IsMine && EventSystem.current.currentSelectedGameObject == null)
        {
            HandleCameraRotation();  // 카메라 회전 허용
        }
        else
        {
            BlockMouseInput();  // UI가 활성화되면 마우스 입력 차단
        }
    }

    // 마우스 움직임에 따른 카메라 회전 처리
    void HandleCameraRotation()
    {
        EventSystem.current.SetSelectedGameObject(null);
        //// 마우스 이동 입력
        //float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        //float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //Vector3 currentRotation = cameraTransform.localEulerAngles;
        //currentRotation.x -= mouseY;  // 상하 회전
        //currentRotation.y += mouseX;  // 좌우 회전

        //// 상하 회전 범위를 제한하기 위한 방법 (optional)
        //currentRotation.x = Mathf.Clamp(currentRotation.x, -90f, 90f);

        //cameraTransform.localEulerAngles = currentRotation;
    }

    // 마우스 이동을 차단하는 함수 (입력값을 0으로)
    void BlockMouseInput()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
        // 마우스 X와 Y축 입력을 차단
        //Input.ResetInputAxes();  // 모든 입력 축을 리셋 (마우스 입력 포함)
    }

    // UI를 켤 때 카메라의 현재 상태를 저장하고 고정
    void ToggleUI()
    {
        if (isUIActive)
        {
            DeactivateUI();  // UI가 이미 활성화되어 있으면 비활성화
        }
        else
        {
            isUIActive = true;

            // NPCMovement와 NavMeshAgent 비활성화
            if (npcMovement != null)
            {
                npcMovement.enabled = false;
            }
            if (navMeshAgent != null)
            {
                navMeshAgent.enabled = false;
            }

            // UI가 활성화되기 직전의 카메라 위치와 회전을 저장
            cameraInitialPosition = cameraTransform.position;
            cameraInitialRotation = cameraTransform.rotation;

            // 오브젝트의 Z축을 플레이어 방향으로 부드럽게 회전
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            StartCoroutine(SmoothRotation(targetRotation));

            if (uiInstance == null)
            {
                GameObject uiPrefab = Resources.Load<GameObject>(uiPrefabPath);
                if (uiPrefab != null)
                {
                    GameObject canvas = GameObject.Find("Canvas");
                    if (canvas != null)
                    {
                        uiInstance = Instantiate(uiPrefab, canvas.transform);
                        SetPanelPosition();

                        chatInputField = uiInstance.GetComponentInChildren<TMP_InputField>();
                        chatInputField?.ActivateInputField();
                    }
                }
            }
            else
            {
                uiInstance.SetActive(true);
                chatInputField?.ActivateInputField();
            }

            // Mark 스크립트의 ClearText() 메서드를 호출
            if (markScript != null)
            {
                markScript.ClearText();
            }
        }
    }

    // 부드럽게 회전하는 코루틴
    IEnumerator SmoothRotation(Quaternion targetRotation)
    {
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }
        transform.rotation = targetRotation;  // 최종적으로 목표 회전에 맞춤
    }

    void SetPanelPosition()
    {
        RectTransform rectTransform = uiInstance?.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchorMin = rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = panelPosition;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }
    }

    // UI 비활성화 함수 (Esc로 비활성화 가능)
    void DeactivateUI()
    {
        isUIActive = false;
        uiInstance?.SetActive(false);
        if (chatInputField != null)
        {
            chatInputField.DeactivateInputField();
            chatInputField.text = "";
        }

        // Esc로 UI 비활성화 시 마우스 입력을 다시 받도록 설정
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // NPCMovement와 NavMeshAgent 활성화
        if (npcMovement != null)
        {
            npcMovement.enabled = true;
        }
        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = true;
        }

        EventSystem.current.SetSelectedGameObject(null);
    }

    // 로컬 플레이어의 Transform을 찾는 코루틴
    private IEnumerator FindLocalPlayer()
    {
        while (playerTransform == null)
        {
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                PhotonView localPv = player.GetComponent<PhotonView>();
                if (localPv != null && localPv.IsMine)
                {
                    playerTransform = player.transform;
                    pv = localPv;  // PhotonView를 저장하여 나중에 사용
                    break;
                }
            }
            yield return null;
        }
    }
}