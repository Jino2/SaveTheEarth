using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGoods : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            GoodsInfo goodsInfo = other.gameObject.GetComponent<GoodsInfo>();
            PhotonView itemPV = other.gameObject.GetPhotonView();

            if (goodsInfo != null && itemPV != null)
            {
                // 로컬 클라이언트가 아이템의 소유자일 경우에만 인벤토리에 추가
                if (photonView.IsMine)
                {
                    // 로컬 클라이언트에서 아이템을 인벤토리에 추가
                    Inventory_KJS.instance.AddGoods(goodsInfo);
                }

                // 모든 클라이언트에 아이템 비활성화 요청 (버퍼링 없이)
                photonView.RPC("DisableItem", RpcTarget.All, itemPV.ViewID);
            }
        }
    }

    [PunRPC]
    public void DisableItem(int viewID)
    {
        PhotonView targetPV = PhotonView.Find(viewID);

        if (targetPV != null)
        {
            GameObject item = targetPV.gameObject;

            if (item != null)
            {
                // 오브젝트 삭제 전, 아이템 비활성화 처리 (로컬 데이터로만)
                Inventory_KJS.instance.AddGetObject(item);

                // 오브젝트 삭제
                Destroy(item);
            }
        }
    }
}