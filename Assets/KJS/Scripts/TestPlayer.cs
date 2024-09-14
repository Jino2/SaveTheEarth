using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
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

            if (goodsInfo != null)
            {
                // Inventory_KJS에 아이템 추가 (수량 증가)
                Inventory_KJS.instance.AddGoods(goodsInfo);

                // 오브젝트 비활성화
                other.gameObject.SetActive(false);

                // 비활성화된 오브젝트를 Inventory_KJS의 리스트에 추가
                Inventory_KJS.instance.AddGetObject(other.gameObject);
            }
        }
    }
}