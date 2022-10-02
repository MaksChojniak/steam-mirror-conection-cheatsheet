using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;

public class LobbyDataEntry : MonoBehaviour
{
    // Data
    public CSteamID lobbyID;
    public string lobbyName;
    public Text lobbyNameText;

    //add from me
    public int classes;


    public void SetLobbyData()
    {
        if(lobbyName == "")
        {
            lobbyNameText.text = "Empty";
        }
        else
        {
            lobbyNameText.text = lobbyName;
        }

    }

    public void JoinLobby()
    {
        SteamLobby.instance.JoinLobby(lobbyID, classes);
    }
}
