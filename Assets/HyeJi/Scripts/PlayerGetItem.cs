using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetItem : MonoBehaviour, ICollectible
{
    private GameObject inventory;

    void Start()
    {
    }

    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            // 아이템을 인벤토리에 넣자 (인벤 스크립트 참조하면 될듯?)
            // 아이템의 정보를 가져오기
            GoodsInfo goodsInfo = other.gameObject.GetComponent<GoodsInfo>();

            if (goodsInfo != null)
            {
                // Inventory_KJS에 아이템 추가 (수량 증가)
                Inventory_KJS.instance.AddGoods(goodsInfo);

                // 오브젝트 비활성화
                other.gameObject.SetActive(false);

                // 비활성화된 오브젝트를 Inventory_KJS의 리스트에 추가
                Inventory_KJS.instance.AddGetObject(other.gameObject);
            }

            //Destroy(other.gameObject);

            print("먹었다!");
        }
    }

    public void Collect(int itemId)
    {
        UserApi.AddItemToUserInventory(
            UserCache.GetInstance().Id,
            itemId,
            (t) => { print("Added to inventory"); }
        );
    }
}