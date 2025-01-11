using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class H_LobbyUIManager : MonoBehaviourPun
{
    private PlayerUI playerUI;

    void Start()
    {
        playerUI = GetComponent<PlayerUI>();
    }
}
