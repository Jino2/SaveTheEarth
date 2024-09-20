using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class H_PlayerInfo
{
    public string userid;
    public string message;
    public int trashCount;

    public H_PlayerInfo(string userid, string message, int trashCount)
    {
        this.userid = userid;
        this.message = message;
        this.trashCount = trashCount;
    }
}
