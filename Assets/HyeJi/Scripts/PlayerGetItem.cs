using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetItem : MonoBehaviour
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
        if(other.CompareTag("Item"))
        {
            // 아이템을 인벤토리에 넣자 (인벤 스크립트 참조하면 될듯?)
            // 아이템의 정보를 가져오기
            //ItemComponent itemComponent = other.gameObject.GetComponent<ItemComponent>();
            //if (itemComponent != null)
            //{
            //    inventory.AddItem(itemComponent.item);
            //}

            Destroy(other.gameObject);

            print("먹었다!");
        }
    }
}
