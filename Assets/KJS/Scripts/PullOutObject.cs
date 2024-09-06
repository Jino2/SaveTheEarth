﻿using UnityEngine;
using UnityEngine.UI;  // UI Button을 위해 필요한 네임스페이스

public class PullOutObject : MonoBehaviour
{
    public Inventory_KJS inventory;  // Inventory 참조
    public GoodsType[] targetGoodsTypes;  // 각 버튼에 해당하는 GoodsType 배열

    public Button[] activateButtons;  // 여러 버튼을 배열로 관리
    private bool isPrefabActive = true;  // 프리팹의 활성화 상태를 추적 (기본 활성화)

    void Start()
    {
        // Inventory 싱글톤 인스턴스 참조
        inventory = Inventory_KJS.instance;

        if (inventory != null)
        {
            // 아이템이 추가될 때 이벤트 구독
            //inventory.OnItemAdded += OnItemAddedToInventory;
        }
        else
        {
            Debug.LogError("Inventory 인스턴스를 찾을 수 없습니다.");
        }

        // 각 버튼에 해당하는 GoodsType 설정 및 버튼 이벤트 연결
        for (int i = 0; i < activateButtons.Length; i++)
        {
            if (activateButtons[i] != null)
            {
                int index = i;  // 클로저 문제 방지
                activateButtons[i].onClick.AddListener(() => OnActivateButtonClicked(targetGoodsTypes[index]));
            }
            else
            {
                Debug.LogError($"활성화 버튼 {i}이 할당되지 않았습니다.");
            }
        }
    }

    // 아이템이 추가되었을 때 호출될 함수
    private void OnItemAddedToInventory(GoodsInfo newItem)
    {
        // 마지막에 추가된 아이템을 추적
        Debug.Log($"아이템이 추가되었습니다: {newItem.goodsType}, 수량: {newItem.count}");
    }

    // 각 GoodsType에 맞는 버튼이 클릭되었을 때 호출되는 함수
    private void OnActivateButtonClicked(GoodsType goodsType)
    {
        // Inventory에 저장된 해당 GoodsType의 비활성화된 오브젝트들을 다시 활성화
        Inventory_KJS.instance.ActivateDisabledObject(goodsType);
        Inventory_KJS.instance.MinusGoodsCount(goodsType);  // 해당 GoodsType의 수량 감소
    }

    // OnDestroy에서 이벤트 구독 해제
    private void OnDestroy()
    {
        if (inventory != null)
        {
            //inventory.OnItemAdded -= OnItemAddedToInventory;
        }
    }
}