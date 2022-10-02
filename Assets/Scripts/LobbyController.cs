using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Steamworks;
using System.Linq;
using System;

public class LobbyController : MonoBehaviour
{
    public static LobbyController instance;

    // UI Elements
    public Text lobbyNameText;

    // Player Data
    public GameObject playerListViewContent;
    public GameObject playerListItemPrefab;
    public GameObject localPlayerObject;

    // Other Data
    public ulong currentLobbyID;
    public bool playerItemCreated = false;
    private List<PlayerListItem> playerListItems = new List<PlayerListItem>();
    public PlayerObjectController localPlayerController;

    // Ready
    public Button startGameButton;
    public Text readyButtonText;


    // Manager
    private CustomNetworkManager manager;

    private CustomNetworkManager Manager
    {
        get
        {
            if (manager != null) { return manager; }

            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }


    void Awake()
    {
        if(instance == null) { instance = this; }
    }


    public void ReadyPlayer()
    {
        localPlayerController.ChangeReady();
    }

    public void UpdateButton()
    {
        if(localPlayerController.ready)
        {
            readyButtonText.text = "Unready";
        }
        else
        {
            readyButtonText.text = "Ready"; 
        }
    }

    public void CheckIfAllReady()
    {
        bool allReady = false;

        foreach(PlayerObjectController player in Manager.gamePlayers)
        {
            if(player.ready)
            {
                allReady = true;
            }
            else
            {
                allReady = false;
                break;
            }
        }

        if(allReady)
        {
            if(localPlayerController.playerIdNumber == 1)
            {
                startGameButton.interactable = true;
            }
            else
            {
                startGameButton.interactable = false;
            }
        }
        else
        {
            startGameButton.interactable = false;
        }

    }


    public void UpdateLobbyName()
    {
        currentLobbyID = manager.GetComponent<SteamLobby>().currentLobbyId;
        lobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(currentLobbyID), "name");
    }

    public void UpdatePlayerList()
    {
        if(!playerItemCreated) { CreateHostPlayerItem(); }  // Host
        if(playerListItems.Count < manager.gamePlayers.Count) {  CreateClientPlayerItem(); }
        if (playerListItems.Count > manager.gamePlayers.Count) { RemovePlayerItem(); }
        if (playerListItems.Count == manager.gamePlayers.Count) { UpdatePlayerItem(); }

    }

    public void FindLocalPlayer()
    {
        localPlayerObject = GameObject.Find("LocalGamePlayer");
        localPlayerController = localPlayerObject.GetComponent<PlayerObjectController>();
    }


    public void CreateHostPlayerItem()
    {
        foreach(PlayerObjectController player  in Manager.gamePlayers)
        {
            GameObject newPlayerItem = Instantiate(playerListItemPrefab) as GameObject;
            PlayerListItem newPlayerItemScript = newPlayerItem.GetComponent<PlayerListItem>();

            newPlayerItemScript.playerName = player.playerName;
            newPlayerItemScript.connectionID = player.conncetionID;
            newPlayerItemScript.playerSteamID = player.playerSteamID;
            newPlayerItemScript.ready = player.ready;   
            newPlayerItemScript.SetPlayerValues();

            newPlayerItem.transform.SetParent(playerListViewContent.transform);
            newPlayerItem.transform.localScale = Vector3.one;

            playerListItems.Add(newPlayerItemScript);
        }
        playerItemCreated = true;
    }

    public void CreateClientPlayerItem()
    {
        foreach(PlayerObjectController player in Manager.gamePlayers)
        {
            if (!playerListItems.Any(b => b.connectionID == player.conncetionID))
            {
                GameObject newPlayerItem = Instantiate(playerListItemPrefab) as GameObject;
                PlayerListItem newPlayerItemScript = newPlayerItem.GetComponent<PlayerListItem>();

                newPlayerItemScript.playerName = player.playerName;
                newPlayerItemScript.connectionID = player.conncetionID;
                newPlayerItemScript.playerSteamID = player.playerSteamID;
                newPlayerItemScript.ready = player.ready;
                newPlayerItemScript.SetPlayerValues();

                newPlayerItem.transform.SetParent(playerListViewContent.transform);
                newPlayerItem.transform.localScale = Vector3.one;

                playerListItems.Add(newPlayerItemScript);
            }
        }
    }


    public void RemovePlayerItem()
    {
        List<PlayerListItem> playerListItemsToRemove = new List<PlayerListItem>();

        foreach (PlayerListItem playerListItem in playerListItems)
        {
            if (!Manager.gamePlayers.Any(b => b.conncetionID == playerListItem.connectionID))
            {
                playerListItemsToRemove.Add(playerListItem);
            }
        }
        if (playerListItemsToRemove.Count > 0)
        {
            foreach (PlayerListItem playerlistItemsToRemove in playerListItemsToRemove)
            {
                GameObject ObjectToRemove = playerlistItemsToRemove.gameObject;
                playerListItems.Remove(playerlistItemsToRemove);
                Destroy(ObjectToRemove);
                ObjectToRemove = null;
            }
        }
    }

    public void UpdatePlayerItem()
    {
        foreach (PlayerObjectController player in Manager.gamePlayers)
        {
            foreach (PlayerListItem playerListItemScript in playerListItems)
            {
                if (playerListItemScript.connectionID == player.conncetionID)
                {
                    playerListItemScript.playerName = player.playerName;
                    playerListItemScript.ready = player.ready;
                    playerListItemScript.SetPlayerValues();

                    if(player == localPlayerController)
                    {
                        UpdateButton();
                    }
                }
            }
        }
        CheckIfAllReady();
    }


    //  dodatkowo multiplayer 3d
    public void StartGame(string SceneName)
    {
        localPlayerController.CanStartGame(SceneName);
    }
}
