using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class H_TrashRewardManager : MonoBehaviour, IPunObservable
{
    PhotonView pv;

    // 미션 완료 여부 체크
    private bool missionCompleted = false;
    public int trashCount = 0;
    public int clearThreshold = 5;
    public int rewardPoint = 10;

    // AI URL
    private string aiUrl;
    // 챗봇 타입을 저장
    public ChatInfo.ChatType chatType;

    // 퀘스트 관련 이미지
    public Image TrashStart;
    public Image TrashComplete;
    // 현재 퀘스트 진행 상황 텍스트
    public GameObject panel_trashCount;
    public TMP_Text text_currTrashCount;

    ParticleSystem ps;

    // 현재 스코어를 담을 변수
    int currScore;
    public int CurrScore
    {
        get { return currScore; }
        set { AddScore(value); }
    }

    public ChatManager chatManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddScore(int addValue)
    {
        // 현재 점수를 addValue 만큼 증가
        currScore += 1;
        // 현재 점수 UI를 갱신시킨다
        text_currTrashCount.text = "현재 주운 쓰레기 : " + currScore;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // isWriting -> isMine 이 나 일때 주겠다
        if (stream.IsWriting)
        {
            stream.SendNext(trashCount);

        }
        // isReading -> isMine 이 내가 아닐 때 주겠다
        else if (stream.IsReading)
        {
            trashCount = (int)stream.ReceiveNext();

        }
    }
}
