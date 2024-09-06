using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Inventory_KJS : MonoBehaviour
{
    public static Inventory_KJS instance;  // 싱글톤 인스턴스
    public InventoryUI inventoryUI;    
    public List<GameObject> goods = new List<GameObject>();  //오브젝트 리스트
    private Dictionary<GoodsType, int> goodsCounts = new Dictionary<GoodsType, int>();  // GoodsType별로 수량을 추적

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // 각 GoodsType의 수량을 초기화
        foreach (GoodsType type in System.Enum.GetValues(typeof(GoodsType)))
        {
            if (type != GoodsType.None)
            {
                goodsCounts[type] = 0;  // 각 GoodsType의 수량을 0으로 초기화
            }
        }
    }

    // 특정 GoodsType의 수량을 반환하는 함수 (데이터 참조용)
    public int CurrentGoodsCount(GoodsType goodsType)
    {
        if (goodsCounts.ContainsKey(goodsType))
        {
            return goodsCounts[goodsType];
        }
        return 0;
    }

    // 외부에서 수량을 직접 설정하는 함수 (InventoryUI에서 사용)
    public void SetGoodsCount(GoodsType goodsType, int count)
    {
        goodsCounts[goodsType] = count;
    }

    // 비활성화된 오브젝트 리스트를 반환하는 함수 (데이터 참조용)
    public List<GameObject> GetDisabledObjects()
    {
        return goods;
    }

    // 비활성화된 오브젝트를 리스트에 추가하는 함수
    public void AddGetObject(GameObject obj)
    {
        if (obj != null && !goods.Contains(obj))
        {
            goods.Add(obj);
        }
    }

    //  MinusGoodsCount는 이제 InventoryUI로 위임
    public void MinusGoodsCount(GoodsType goodsType)
    {
        if (inventoryUI != null)
        {
            inventoryUI.MinusCount(goodsType);
        }
    }

    // ActivateDisabledObject는 이제 InventoryUI로 위임
    public void ActivateDisabledObject(GoodsType goodsType)
    {
        if (inventoryUI != null)
        {
            inventoryUI.ActivateObject(goodsType);
        }
    }
    public void AddGoods(GoodsInfo goodsInfo)
    {
        if (goodsInfo != null)
        {
            // GoodsType에 따라 수량을 증가
            if (goodsCounts.ContainsKey(goodsInfo.goodsType))
            {
                goodsCounts[goodsInfo.goodsType] += goodsInfo.count;
            }
            else
            {
                goodsCounts[goodsInfo.goodsType] = goodsInfo.count;
            }

            // UI 업데이트 (필요한 경우)
            if (inventoryUI != null)
            {
                inventoryUI.UpdateText(goodsInfo.goodsType);
            }

            Debug.Log($"{goodsInfo.goodsType}이를 획득했습니다.");
        }
        
    }
}
