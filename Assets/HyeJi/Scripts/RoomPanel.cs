using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : MonoBehaviour
{
    public TMP_Text[] roomTexts = new TMP_Text[3];
    public Button btn_join;

    
    public void SetRoomInfo(RoomInfo room)
    {
        roomTexts[0].text = room.Name;
        roomTexts[1].text = (string)room.CustomProperties["MASTER_NAME"];
        roomTexts[2].text = $"({room.PlayerCount}/{room.MaxPlayers})";
    }
}