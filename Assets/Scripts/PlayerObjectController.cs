using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using System;

public class PlayerObjectController : NetworkBehaviour
{
    // Player Data
    [SyncVar] public int conncetionID;
    [SyncVar] public int playerIdNumber;
    [SyncVar] public ulong playerSteamID;
    [SyncVar] public int classes;

    [SyncVar(hook = nameof(PlayerNameUpdate))] public string playerName;

    [SyncVar(hook = nameof(PlayerReadyUpdate))] public bool ready;


    private CustomNetworkManager manager;

    private CustomNetworkManager Manager
    {
        get
        {
            if (manager != null) { return manager; }

            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }


    //  dodatkowo multiplayer 3d
    void Start()
    {
        DontDestroyOnLoad(this.gameObject); 
        
    }

    public override void OnStartAuthority()
    {
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        gameObject.name = "LocalGamePlayer";
        classes = ManagerClasses.classes;
        LobbyController.instance.FindLocalPlayer();
        LobbyController.instance.UpdateLobbyName();
    }

    public override void OnStartClient()
    {
        Manager.gamePlayers.Add(this);
        LobbyController.instance.UpdateLobbyName();
        LobbyController.instance.UpdatePlayerList();
    }

    public override void OnStopClient()
    {
        Manager.gamePlayers.Remove(this);
        LobbyController.instance.UpdatePlayerList();
    }

    public void ChangeReady()
    {
        if (hasAuthority)
        {
            CmdSetPlayerReady();
        }
    }

    [Command]
    private void CmdSetPlayerName(string playerName)
    {
        this.PlayerNameUpdate(this.playerName, playerName);
    }

    [Command]
    private void CmdSetPlayerReady()
    {
        this.PlayerReadyUpdate(this.ready, !this.ready);
    }

    private void PlayerReadyUpdate(bool oldvalue, bool newValue)
    {
        if (isServer) // Host
        {
            this.ready = newValue;
        }
        if (isClient) // Client
        {
            LobbyController.instance.UpdatePlayerList();
        }
    }

    private void PlayerNameUpdate(string oldvalue, string newValue)
    {
        if(isServer) // Host
        {
            this.playerName = newValue;
        }
        if(isClient) // Client
        {
            LobbyController.instance.UpdatePlayerList();
        }
    }


    //  dodatkowo multiplayer 3d
    public void CanStartGame(string SceneName)
    {
        if(hasAuthority)
        {
            CmdCanStartGame(SceneName);
        }
    }
    [Command]
    public void CmdCanStartGame(string SceneName)
    {
        manager.StartGame(SceneName);   
    }

}
