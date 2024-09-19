using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGetTrash : MonoBehaviourPun
{
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

            //Destroy(other.gameObject);
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
