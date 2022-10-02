using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Steamworks;

public class SteamLobby : MonoBehaviour
{
    public static SteamLobby instance;

    // Callbacks
    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> joinRequest;
    protected Callback<LobbyEnter_t> lobbyEntered;

    // Lobbies Callback
    protected Callback<LobbyMatchList_t> lobbyList;
    protected Callback<LobbyDataUpdate_t> lobbyDataUpdate;

    public List<CSteamID> lobbyIDs = new List<CSteamID>();

    // Variables
    public ulong currentLobbyId;
    private const string hostAdressKey = "HostAdress";
    private CustomNetworkManager manager;


    void Start()
    {
        if (!SteamManager.Initialized) { return; }
        if (instance == null) { instance = this; }

        manager = GetComponent<CustomNetworkManager>();

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        joinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

        lobbyList = Callback<LobbyMatchList_t>.Create(OnGetLobbyList);
        lobbyDataUpdate = Callback<LobbyDataUpdate_t>.Create(OnGetLobbyData);
    }

    public void HostLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, manager.maxConnections);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK) return;

        Debug.Log("Lobby Created Succesfully");

        manager.StartHost();
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), hostAdressKey, SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name", SteamFriends.GetPersonaName().ToString() + " 'S LOBBY");

        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "classes", ManagerClasses.classes.ToString());
    }

    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        Debug.Log("Request To Join Lobby");
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callabck)
    {
        // Everyone
        ///hostButton.SetActive(false);
        currentLobbyId = callabck.m_ulSteamIDLobby;
        ///lobbyNameText.gameObject.SetActive(true);
        ///lobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(callabck.m_ulSteamIDLobby), "name");

        // Client
        if (NetworkServer.active) { return; }

        manager.networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callabck.m_ulSteamIDLobby), hostAdressKey);

        manager.StartClient();

    }


    public void GetLobbiesList()
    {
        if(lobbyIDs.Count > 0) { lobbyIDs.Clear(); }

        SteamMatchmaking.AddRequestLobbyListResultCountFilter(6);
        SteamMatchmaking.RequestLobbyList();
    }

    private void OnGetLobbyList(LobbyMatchList_t result)
    {
        if (LobbyListManager.instance.listOfLobbies.Count > 0) { LobbyListManager.instance.DestroyLobbies(); }

        for(int i = 0; i < result.m_nLobbiesMatching; i++)
        {
            CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
            lobbyIDs.Add(lobbyID);
            SteamMatchmaking.RequestLobbyData(lobbyID);
        }
    }

    private void OnGetLobbyData(LobbyDataUpdate_t result)
    {
        LobbyListManager.instance.DisplayLobbies(lobbyIDs, result);
    }

    public void JoinLobby(CSteamID lobbyID, int classes)
    {
        if(classes == ManagerClasses.classes) { print("klasa pasuje i wynosi: " + classes); }

        SteamMatchmaking.JoinLobby(lobbyID);
    }

}
