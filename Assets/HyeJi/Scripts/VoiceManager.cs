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
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            // 전송을 false 시킨다 (값 거꾸로 넣어주기)
            recoder.TransmitEnabled = !recoder.TransmitEnabled;
        }
    }
}
