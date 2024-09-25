using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;
public class ChatActivator : MonoBehaviourPun
{
    public float activationDistance = 5f;
    public string uiPrefabPath = "ChatPanel";
    private GameObject uiInstance;
    public TextMeshProUGUI pressEText;
    private bool isUIActive = false;
    public Transform playerTransform;
    private TMP_InputField chatInputField;

    public Vector2 panelPosition = new Vector2(-500f, 0f);
    public Transform cameraTransform;  // 카메라 Transform 추가
    public float mouseSensitivity = 100f;  // 마우스 감도

    private Vector3 cameraInitialPosition;
    private Quaternion cameraInitialRotation;

    private PhotonView pv;

    public NPCMovement npcMovement; // Inspector에서 할당할 NPCMovement 스크립트

    void Start()
    {
        pressEText?.gameObject.SetActive(false);
        StartCoroutine(FindLocalPlayer());
        Cursor.lockState = CursorLockMode.None;  // 마우스 커서 항상 보이도록 설정
        Cursor.visible = true;  // 커서 항상 보임
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
            BlockMouseInput(); // UI가 활성화되면 마우스 입력 차단
        }
    }

    // 마우스 움직임에 따른 카메라 회전 처리
    void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        Vector3 currentRotation = cameraTransform.localEulerAngles;
        currentRotation.x -= mouseY;  // 상하 회전
        currentRotation.y += mouseX;  // 좌우 회전

        cameraTransform.localEulerAngles = currentRotation;
    }

    // 마우스 이동을 차단하는 함수 (입력값을 0으로)
    void BlockMouseInput()
    {
        Input.ResetInputAxes();  // 모든 입력 축을 리셋 (마우스 입력 포함)
    }

    // UI를 켤 때 NPCMovement를 비활성화
    void ToggleUI()
    {
        if (isUIActive)
        {
            DeactivateUI();  // UI가 이미 활성화되어 있으면 비활성화
        }
        else
        {
            isUIActive = true;

            // UI가 활성화되기 직전의 카메라 위치와 회전을 저장
            cameraInitialPosition = cameraTransform.position;
            cameraInitialRotation = cameraTransform.rotation;

            // Inspector에서 할당된 npcMovement가 있을 때만 비활성화
            if (npcMovement != null)
            {
                npcMovement.enabled = false;  // NPCMovement 비활성화
            }

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
        }
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

        // UI 비활성화 시 NPCMovement 다시 활성화
        if (npcMovement != null)
        {
            npcMovement.enabled = true;
        }

        // Esc로 UI 비활성화 시 마우스 입력을 다시 받도록 설정
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        EventSystem.current.SetSelectedGameObject(null);
    }

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