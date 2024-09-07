using UnityEngine;
using TMPro;
using System.Collections.Generic;
using static GoodsInfo;  // GoodsInfo의 enum GoodsType을 사용하기 위해 static import

public class InventoryUI : MonoBehaviour
{
    public List<GameObject> prefabList;
    public Transform spawnPoint;
    public TextMeshProUGUI[] itemCountTexts;  // GoodsType별로 수량을 표시할 TextMeshPro 배열

    private bool isCountReduced = false;  // 수량 감소 중복 방지를 위한 플래그

    void Update()
    {
        // 숫자 키 입력에 따른 프리팹 생성
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CreateObjectByKey(0, GoodsType.Chest);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CreateObjectByKey(1, GoodsType.Sword);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CreateObjectByKey(2, GoodsType.Rock);
        }
    }

    // 숫자 키 입력에 따른 프리팹 생성 메서드
    public void CreateObjectByKey(int prefabIndex, GoodsType goodsType)
    {
        if (prefabIndex >= 0 && prefabIndex < prefabList.Count)
        {
            CreateObject(prefabList[prefabIndex], goodsType, false);  // false: 버튼 클릭 아님
        }
    }

    // GoodsType에 대응하는 메서드들 (OnClick 이벤트에 연결 가능)
    public void OnChestButtonClicked()
    {
        if (!isCountReduced)  // 수량 감소가 한 번도 처리되지 않았을 때만 실행
        {
            CreateObject(prefabList[0], GoodsType.Chest, true);  // true: 버튼 클릭
        }
    }

    public void OnSwordButtonClicked()
    {
        if (!isCountReduced)
        { 
            CreateObject(prefabList[1], GoodsType.Sword, true);  // true: 버튼 클릭
        }
    }

    public void OnRockButtonClicked()
    {
        if (!isCountReduced)
        {
            CreateObject(prefabList[2], GoodsType.Rock, true);  // true: 버튼 클릭
        }
    }

    // 프리팹을 생성하는 메서드 (키 입력 또는 버튼 클릭 여부에 따라 수량 감소 처리)
    public void CreateObject(GameObject prefab, GoodsType goodsType, bool isButtonClick)
    {
        int currentCount = Inventory_KJS.instance.CurrentGoodsCount(goodsType);

        // 수량이 0보다 클 경우에만 생성
        if (currentCount > 0)
        {
            if (prefab != null)
            {
                Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : new Vector3(0, 1, 0);
                Instantiate(prefab, spawnPosition, Quaternion.identity);


                // 키 입력과 버튼 클릭에 따라 다른 메서드로 수량 감소 처리
                if (isButtonClick)
                {

                    MinusCount(goodsType);
                    isCountReduced = true;  // 수량 감소 후 플래그 설정
                    Invoke(nameof(ResetCountReduced), 0.1f);  // 일정 시간 후에 플래그 초기화
                }
                else
                {
                    MinusCountByKey(goodsType);  // 숫자 키 입력 시 별도 처리
                }
            }
        }
    }

    // 플래그 초기화 메서드 (일정 시간이 지난 후 수량 감소 플래그를 초기화)
    private void ResetCountReduced()
    {
        isCountReduced = false;
    }

    // 버튼 클릭 시 호출되는 GoodsType의 수량을 1씩 감소시키고 UI를 업데이트하는 메서드
    public void MinusCount(GoodsType goodsType)
    {
        int currentCount = Inventory_KJS.instance.CurrentGoodsCount(goodsType);

        // 플래그로 중복 호출 방지
        if (!isCountReduced)
        {

            if (currentCount > 0)
            {
                Inventory_KJS.instance.SetGoodsCount(goodsType, currentCount - 1);

                // UI 업데이트
                UpdateText(goodsType);
            }
        }
    }

    // 숫자 키 입력 시 호출되는 GoodsType의 수량을 1씩 감소시키고 UI를 업데이트하는 메서드
    public void MinusCountByKey(GoodsType goodsType)
    {
        int currentCount = Inventory_KJS.instance.CurrentGoodsCount(goodsType);
        

        if (currentCount > 0)
        {
            Inventory_KJS.instance.SetGoodsCount(goodsType, currentCount - 1);
            

            // UI 업데이트
            UpdateText(goodsType);
        }
       
    }

    // 특정 GoodsType의 수량을 TextMeshPro에 업데이트하는 함수
    public void UpdateText(GoodsType goodsType)
    {
        int index = (int)goodsType - 6;

        if (index >= 0 && index < itemCountTexts.Length)
        {
            int count = Inventory_KJS.instance.CurrentGoodsCount(goodsType);
            itemCountTexts[index].text = $"{count}";
        }
    }
}