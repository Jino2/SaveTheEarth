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

    void Start()
    {
        pressEText?.gameObject.SetActive(false);
        StartCoroutine(FindLocalPlayer());
    }

    void Update()
    {
        if (playerTransform == null || pressEText == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= activationDistance)
        {
            pressEText.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E) && EventSystem.current.currentSelectedGameObject == null) ToggleUI();
        }
        else pressEText.gameObject.SetActive(false);

        if (isUIActive && Input.GetKeyDown(KeyCode.Escape)) DeactivateUI();
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
                    break;
                }
            }
            yield return null;
        }
    }

    void ToggleUI()
    {
        if (isUIActive) return;

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
                else Debug.LogError("Canvas not found!");
            }
            else Debug.LogError("UI Prefab not found!");
        }
        else
        {
            uiInstance.SetActive(true);
            chatInputField?.ActivateInputField();
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

    void DeactivateUI()
    {
        isUIActive = false;
        uiInstance?.SetActive(false);
        if (chatInputField != null)
        {
            chatInputField.DeactivateInputField();
            chatInputField.text = "";
        }
        EventSystem.current.SetSelectedGameObject(null);
    }
}