using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GoodsType
{
    None = -1,
    Item1,
    Item2,
    Item3
}

[System.Serializable]
public class GoodsInfo : MonoBehaviour
{
    public GoodsType goodsType;  // 아이템의 종류
    public int count = 1;        // 기본 아이템 수량

    // 인벤토리에 아이템을 추가하는 메서드
    public void AddToInventory()
    {
        // Inventory에 해당 GoodsType의 아이템을 추가
        if (Inventory_KJS.instance != null)
        {
            Inventory_KJS.instance.AddGoods(this);  // this는 현재 GoodsInfo 객체를 의미
        }
        else
        {
            Debug.LogWarning("Inventory 인스턴스가 존재하지 않습니다.");
        }
    }
}