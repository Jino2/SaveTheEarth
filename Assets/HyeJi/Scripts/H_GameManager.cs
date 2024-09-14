using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public class H_GameManager : MonoBehaviourPun
{
    void Start()
    {
        StartCoroutine(SpawnPlayer());

        // OnPhotonSerializeView 에서 데이터 전송 빈도 수 설정하기 (per seconds)
        PhotonNetwork.SerializationRate = 30;
        // 대부분의 데이터 전송 빈도 수 설정하기(per seconds)
        PhotonNetwork.SendRate = 30;
    }

    void Update()
    {
        
    }

    IEnumerator SpawnPlayer()
    {
        yield return new WaitUntil(() => { return PhotonNetwork.InRoom; });

        Vector2 randomPos = Random.insideUnitCircle * 5.0f;
        Vector3 initPosition = new Vector3(randomPos.x, 1.0f, randomPos.y);

        // 플레이어를 생성한다
        GameObject player = PhotonNetwork.Instantiate("Player", initPosition, Quaternion.identity);

        print(player != null ? "생성!@" : "생성 실패....");
    }
}
