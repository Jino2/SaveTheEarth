using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

//public class ButtonHandler : MonoBehaviourPun
//{
//    public InventoryUI inventoryUI;  // InventoryUI 참조

//    // OnClick 이벤트에 따른 프리팹 생성 요청 (InventoryUI에 위임)
//    public void OnChestButtonClicked()
//    {
//        Debug.Log("Chest Button Clicked");
//        inventoryUI.CreateObject("Chest", GoodsType.Chest, true);  // InventoryUI로 요청
//    }

//    public void OnSwordButtonClicked()
//    {
//        Debug.Log("Sword Button Clicked");
//        inventoryUI.CreateObject("Sword", GoodsType.Sword, true);  // InventoryUI로 요청
//    }

//    public void OnRockButtonClicked()
//    {
//        Debug.Log("Rock Button Clicked");
//        inventoryUI.CreateObject("Rock 1", GoodsType.Rock, true);  // InventoryUI로 요청
//    }
//}