using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerStateBase
{
    CharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        Vector3 dir = new Vector3(h, 0, v);

        cc.Move(dir * moveSpeed * Time.deltaTime);

    }

    void Jump()
    {
        // 스페이스바를 누르면 점프한다
        if(Input.GetKeyDown(KeyCode.Space))
        {

        }
    }
}
