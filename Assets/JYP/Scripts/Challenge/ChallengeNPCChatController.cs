using System.Collections;
using System.Collections.Generic;
using System.Text;
using Photon.Pun;
using UnityEngine;
using TMPro;

public class ChallengeNPCChatController : MonoBehaviour
{
    [Header("Interact Setting")]
    [SerializeField]
    private float interactRange = 2.0f;

    [SerializeField]
    private LayerMask interactLayer;

    [Space]
    [Header("UI Setting")]
    [SerializeField]
    private GameObject chatBubbleObject;

    [SerializeField]
    private TMP_Text chatText;

    private readonly string[] chatList =
    {
        "안녕하세요\n탐험가님!\n이 깊은 바닷속에서의 여정이 멋지죠?",
        "하지만, 우리가 지키지 않으면 이 아름다운 세상도 오염되고 말 거예요.",
        "오늘도 바다를 보호하는 챌린지에 도전해보시는 건 어떠세요?"
    };

    [SerializeField]
    private float chatDelayPerWord = 0.2f;


    private bool interactable = false;
    

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayer();
    }

    #region Public Methods

    public void CloseChatBubble()
    {
    }

    #endregion

    #region Private Methods

    private void CheckPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, interactRange, interactLayer);

        var isCurrentUserOverlapped = false;
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<PhotonView>(out var pv))
            {
                if (pv.IsMine)
                {
                    isCurrentUserOverlapped = true;
                }
            }
        }

        if (isCurrentUserOverlapped && !interactable)
        {
            interactable = true;
            chatBubbleObject.SetActive(true);
            StartCoroutine(ShowChat());
        }
        else if (!isCurrentUserOverlapped && interactable)
        {
            interactable = false;
            chatBubbleObject.SetActive(false);
            StopAllCoroutines();
        }
        
    }

    private IEnumerator ShowChat()
    {
        var sb = new StringBuilder();
        foreach (var chat in chatList)
        {
            chatText.text = "";
            sb.Clear();
            foreach (char c in chat)
            {
                sb.Append(c);
                chatText.text = sb.ToString();
                if(c == ' ') continue;
                yield return new WaitForSeconds(chatDelayPerWord);
            }

            yield return new WaitForSeconds(1.5f);
        }
    }

    #endregion
}