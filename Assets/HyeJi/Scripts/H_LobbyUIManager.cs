using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class H_LobbyUIManager : MonoBehaviourPun
{
    private PlayerUI playerUI;

    // Start is called before the first frame update
    void Start()
    {
        playerUI = GetComponent<PlayerUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
