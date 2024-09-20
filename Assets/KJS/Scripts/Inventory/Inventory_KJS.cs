using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Photon.Pun;  // Photon 네임스페이스 추가

public class Inventory_KJS : MonoBehaviourPun
{
    public static Inventory_KJS instance;  // 싱글톤 인스턴스
    public InventoryUI inventoryUI;
    public List<GameObject> goods = new List<GameObject>();  // 오브젝트 리스트
    private Dictionary<GoodsType, int> goodsCounts = new Dictionary<GoodsType, int>();  // GoodsType별로 수량을 추적

    public List<InventoryItem> inventoryItems;

    private void Awake()
    {
        // panel_Inventory라는 이름의 오브젝트에서 InventoryUI 컴포넌트를 자동으로 찾고 할당

        if (instance == null)
        {
            instance = this;
        }
        else if (!photonView.IsMine)
        {
            Destroy(this);
        }

    }

    private void Start()
    {
        GameObject panelInventory = GameObject.Find("Canvas");
        if (panelInventory != null)
        {
            inventoryUI = panelInventory.transform.GetChild(0).GetComponent<InventoryUI>();
            panelInventory.transform.GetChild(0).GetComponent<PullOutObject>().SetInventory(this);
        }
        else
        {
            Debug.LogError("panel_Inventory 오브젝트를 찾을 수 없습니다.");
        }

        // 각 GoodsType의 수량을 초기화
        foreach (GoodsType type in System.Enum.GetValues(typeof(GoodsType)))
        {
            if (type != GoodsType.None)
            {
                goodsCounts[type] = 0;  // 각 GoodsType의 수량을 0으로 초기화
            }
        }

        LoadInventoryItems();

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

    // MinusGoodsCount는 이제 InventoryUI로 위임
    public void MinusGoodsCount(GoodsType goodsType)
    {
        if (inventoryUI != null)
        {
            inventoryUI.MinusCount(goodsType);
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

    public void LoadInventoryItems()
    {
        UserApi.GetUserInventoryList("test", (items) =>
        {
            inventoryItems = items;

            goodsCounts.Clear();
            goods.Clear(); // 기존 리스트 초기화

            foreach (var item in items)
            {
                // InventoryItem의 데이터를 GoodsInfo에 매핑하여 GoodsInfo 인스턴스 생성
                GoodsInfo goodsInfo = new GameObject($"Item_{item.itemId}").AddComponent<GoodsInfo>();

                // GoodsInfo의 데이터 설정
                goodsInfo.goodsType = (GoodsType)item.itemId;
                goodsInfo.count = item.amount;

                // 인벤토리에 추가 (AddGoods가 수량 업데이트 및 UI 처리를 담당)
                goodsInfo.AddToInventory();

                // goods 리스트에도 추가
                goods.Add(goodsInfo.gameObject);
            }
        });
    }
}