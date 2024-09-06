using UnityEngine;
using TMPro;
using System.Collections.Generic;
using static GoodsInfo;  // GoodsInfo의 enum GoodsType을 사용하기 위해 static import

public class InventoryUI : MonoBehaviour
{
    public TextMeshProUGUI[] itemCountTexts;  // GoodsType별로 수량을 표시할 TextMeshPro 배열

    // GoodsType의 수량을 1씩 감소시키고 UI를 업데이트
    public void MinusCount(GoodsType goodsType)
    {
        // Inventory에서 해당 GoodsType의 현재 수량을 가져옴
        int currentCount = Inventory_KJS.instance.CurrentGoodsCount(goodsType);

        // 수량을 1 감소시키고 Inventory에 업데이트
        if (currentCount > 0)
        {
            Inventory_KJS.instance.SetGoodsCount(goodsType, currentCount - 1);  // 수량 감소

            // UI 업데이트
            UpdateText(goodsType);
        }
        else
        {
            Debug.LogWarning($"더 이상 {goodsType}의 수량이 없습니다.");
        }
    }

    // 특정 GoodsType의 오브젝트를 활성화하고 UI를 업데이트
    public void ActivateObject(GoodsType goodsType)
    {
        List<GameObject> disabledObjects = Inventory_KJS.instance.GetDisabledObjects();

        // 하나의 오브젝트만 활성화하도록 루프를 break
        for (int i = 0; i < disabledObjects.Count; i++)
        {
            GameObject obj = disabledObjects[i];
            GoodsInfo goodsInfo = obj.GetComponent<GoodsInfo>();

            if (goodsInfo != null && goodsInfo.goodsType == goodsType)
            {
                obj.SetActive(true);  // 오브젝트 활성화
                obj.transform.position = Vector3.zero;  // 오브젝트 좌표를 (0, 0, 0)으로 설정

                goodsInfo.count = 1;  // 활성화 시 count 초기화

                // 활성화된 오브젝트는 리스트에서 제거
                disabledObjects.RemoveAt(i);

                break;  // 한 번에 하나의 오브젝트만 활성화
            }
        }
    }

    // 특정 GoodsType의 수량을 TextMeshPro에 업데이트하는 함수
    public void UpdateText(GoodsType goodsType)
    {
        int index = (int)goodsType;

        if (index >= 0 && index < itemCountTexts.Length)
        {
            // Inventory에서 해당 GoodsType의 수량을 가져옴
            int count = Inventory_KJS.instance.CurrentGoodsCount(goodsType);

            // TextMeshPro에 수량을 표시
            itemCountTexts[index].text = $"{count}";
        }
    }
}
