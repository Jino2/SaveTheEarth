using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGetTrash : MonoBehaviourPun
{
    H_RewardManager rewardManager;

    // 쓰레기의 카운트를 관하는 텍스트
    public TMP_Text currTrashCount;
    public TMP_Text maxTrashCount;

    void Start()
    {
        // 오브젝트 찾아서 참조시켜
        rewardManager = FindObjectOfType<H_RewardManager>();
    }

    // 쓰레기 줍기
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("trash"))
        {
            // 충돌한 오브젝트 내에서 PhotonView 
            PhotonView trashPV = other.gameObject.GetComponent<PhotonView>();
            if(trashPV != null)
            {
                // 소유권을 이전 후 오브젝트 파괴 호출하기
                photonView.RPC("DestroyTrash", RpcTarget.AllBuffered, trashPV.ViewID);
                          
            }
            // 쓰줍 카운트 업데이트
            rewardManager.AddTrashCount();
        }
    }

    [PunRPC]
    void DestroyTrash(int trashViewID)
    {
        PhotonView trashPV = PhotonView.Find(trashViewID);
        if(trashPV != null)
        {
            Destroy(trashPV.gameObject);
        }
    }
}
