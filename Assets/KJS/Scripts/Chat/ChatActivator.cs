using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

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

    public Mark markObject;  // Mark 스크립트가 부착된 오브젝트를 참조

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

        if (distance <= activationDistance && !isUIActive)
        {
            pressEText.gameObject.SetActive(true);

            // E키를 눌렀을 때 UI를 활성화하거나 비활성화
            if (Input.GetKeyDown(KeyCode.E) && pv.IsMine)
            {
                ToggleUI();
            }
        }
        else
        {
            pressEText.gameObject.SetActive(false);
        }

        if (isUIActive && Input.GetKeyDown(KeyCode.Escape))
        {
            DeactivateUI();
        }
    }

    public void ToggleUI()
    {
        if (isUIActive)
        {
            DeactivateUI();  // 이미 활성화된 상태면 UI 비활성화
        }
        else
        {
            isUIActive = true;

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

            // Mark 오브젝트를 비활성화
            if (markObject != null)
            {
                markObject.DeactivateMarkObject();  // Mark 스크립트의 비활성화 함수 호출
            }
        }
    }

    void DeactivateUI()
    {
        isUIActive = false;
        uiInstance?.SetActive(false);

        if (chatInputField != null)
        {
            chatInputField.DeactivateInputField();
            chatInputField.text = "";
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
                    pv = localPv;
                    break;
                }
            }
            yield return null;
        }
    }
}