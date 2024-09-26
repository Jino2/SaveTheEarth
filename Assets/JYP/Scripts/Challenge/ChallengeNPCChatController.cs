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
    
    [SerializeField]
    private RectTransform chatBubbleRectTransform;

    
    private readonly string[] defaultMetChatList =
    {
        "보상으로 받은 포인트는 해적샵에서 장식품으로 교환할 수 있어요!",
        "앞으로도 바다를 지키는 여정을 함께해요!",
        "다음에도 또 만나요!",
        "아직 바다를 지킬 수 있는 챌린지가 남아있어요!"
    };

    private readonly string[] chatList =
    {
        "안녕하세요\n탐험가님!\n이 깊은 바닷속에서의 여정이 멋지죠?",
        "하지만, 우리가 지키지 않으면 이 아름다운 세상도 오염되고 말 거예요.",
        "오늘도 바다를 보호하는 챌린지에 도전해보시는 건 어떠세요?"
    };


    [SerializeField]
    private float chatDelayPerWord = 0.2f;


    private bool interactable = false;
    private Animator animator;
    public bool IsChatted { get; set; } = false;
    private bool isCurrentUserOverlapped = false;
    private bool isPlayerComing = false;
    private float distanceToPlayer = 0f;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        chatText.text = "여기에요!!";
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayer();
        if (isCurrentUserOverlapped) return;
        CheckPlayerComing();

        if (isPlayerComing)
        {
            // interpolate chatBubbleRectTransform by distance
            var targetScale = Mathf.Clamp( interactRange * 2f / distanceToPlayer, 0.5f, 1.0f);
            chatBubbleRectTransform.localScale = Vector3.one * targetScale;
        }
    }

    #region Public Methods

    public void CloseChatBubble()
    {
        
    }

    public void GreetPlayer()
    {
        IsChatted = true;
        animator.SetTrigger("Greeting");
    }

    #endregion

    #region Private Methods

    private void CheckPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, interactRange, interactLayer);

        isCurrentUserOverlapped = false;
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
            StartCoroutine(ShowChat());
        }
        else if (!isCurrentUserOverlapped && interactable)
        {
            interactable = false;
            StopAllCoroutines();
            if(IsChatted)
                StartCoroutine(ShowChatDefault());
            else 
                chatText.text = "여기에요!!";
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
                if (c == ' ') continue;
                yield return new WaitForSeconds(chatDelayPerWord);
            }

            yield return new WaitForSeconds(1.5f);
        }
    }

    private IEnumerator ShowChatDefault(int i = 0)
    {
        var sb = new StringBuilder();
        var chat = defaultMetChatList[i];
        {
            chatText.text = "";
            sb.Clear();
            foreach (char c in chat)
            {
                sb.Append(c);
                chatText.text = sb.ToString();
                if (c == ' ') continue;
                yield return new WaitForSeconds(chatDelayPerWord);
            }

            yield return new WaitForSeconds(1.5f);
        }

        StartCoroutine(ShowChatDefault((i + 1) % defaultMetChatList.Length));
    }


    private void CheckPlayerComing()
    {
        var colliders = Physics.OverlapSphere(transform.position, interactRange * 3f, interactLayer);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<PhotonView>(out var pv))
            {
                if (pv.IsMine)
                {
                    isPlayerComing = true;
                    distanceToPlayer = Vector3.Distance(transform.position, collider.transform.position);
                }
            }
        }

        if (isPlayerComing && !chatBubbleObject.activeSelf)
        {
            chatBubbleObject.SetActive(true);
            if(IsChatted)
                StartCoroutine(ShowChatDefault());
        }
        else if(!isPlayerComing)
        {
            StopAllCoroutines();
            chatBubbleObject.SetActive(false);
        }
    }
    #endregion
}