using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGoods : MonoBehaviourPun
{
    private List<GoodsInfo> goodsToPickup = new List<GoodsInfo>();  // 픽업 가능한 아이템 목록
    private List<PhotonView> itemsPV = new List<PhotonView>();      // 아이템의 PhotonView 목록

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // G 키를 누르면 모든 아이템을 인벤토리에 추가하고 비활성화
        if (goodsToPickup.Count > 0 && Input.GetKeyDown(KeyCode.G))
        {
            // 로컬 클라이언트가 소유한 아이템만 인벤토리에 추가
            for (int i = 0; i < goodsToPickup.Count; i++)
            {
                GoodsInfo goodsInfo = goodsToPickup[i];
                PhotonView itemPV = itemsPV[i];

                if (goodsInfo != null && itemPV != null && photonView.IsMine)
                {
                    // 로컬 클라이언트에서 아이템을 인벤토리에 추가
                    Inventory_KJS.instance.AddGoods(goodsInfo);

                    // 모든 클라이언트에 아이템 비활성화 요청
                    photonView.RPC("DisableItem", RpcTarget.All, itemPV.ViewID);
                }
            }

            // 아이템 픽업이 완료되었으므로 리스트를 초기화
            goodsToPickup.Clear();
            itemsPV.Clear();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            GoodsInfo goodsInfo = other.gameObject.GetComponent<GoodsInfo>();
            PhotonView itemPV = other.gameObject.GetPhotonView();

            // 아이템과 충돌 시 리스트에 추가
            if (goodsInfo != null && itemPV != null)
            {
                goodsToPickup.Add(goodsInfo);
                itemsPV.Add(itemPV);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            GoodsInfo goodsInfo = other.gameObject.GetComponent<GoodsInfo>();
            PhotonView itemPV = other.gameObject.GetPhotonView();

            // 아이템과의 충돌이 끝났을 때 리스트에서 제거
            if (goodsInfo != null && itemPV != null)
            {
                goodsToPickup.Remove(goodsInfo);
                itemsPV.Remove(itemPV);
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