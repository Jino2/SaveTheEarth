using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 박스 콜라이더와 충돌한다면
        if(other.CompareTag("Player"))
        {

            // 일단은 오브젝트 사라지게 하기
            Destroy(this.gameObject);

            print("먹었다!");
        }

    }
}
