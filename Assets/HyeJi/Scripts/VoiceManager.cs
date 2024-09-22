using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.Unity;
using Photon.Pun;

public class VoiceManager : MonoBehaviourPun
{
    // 레코더를 먼저 캐싱
    Recorder recoder;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        recoder = GetComponent<Recorder>();

        if (PhotonNetwork.InRoom)
        {
            Debug.Log("방에 정상적으로 연결되었습니다.");
        }
        else
        {
            Debug.LogError("Photon Room에 연결되지 않았습니다.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            // 전송을 false 시킨다 (값 거꾸로 넣어주기)
            recoder.TransmitEnabled = !recoder.TransmitEnabled;
        }
    }
}
