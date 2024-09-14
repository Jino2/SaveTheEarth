using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerStateBase : MonoBehaviourPun
{
    public float moveSpeed;
    public float rotSpeed = 200;

    public float walkSpeed = 5;
    public float runSpeed = 10;



    void Start()
    {
        if(photonView.IsMine)
        {
            gameObject.AddComponent<Inventory_KJS>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
