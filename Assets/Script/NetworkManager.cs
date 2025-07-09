using Mirror;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

[DefaultExecutionOrder(-1)]
public class CustomNetworkManager : NetworkManager
{

    int playerId = 0;
    public static CustomNetworkManager Instance;
    public List<Player> Players = new List<Player>();
    public List<Transform> spawnPoints = new List<Transform>();
    int nextIndex = 0;

    private void Awake()
    {
        Instance=this;
    }

    [Server]
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Transform spawn = spawnPoints[nextIndex % spawnPoints.Count];
        nextIndex++;

        GameObject playerObj = Instantiate(playerPrefab, spawn.position, spawn.rotation);
        Player player = playerObj.GetComponent<Player>();
        player.playerId = playerId;

        NetworkServer.AddPlayerForConnection(conn, playerObj);
        Players.Add(player);
        playerId++;
    }

}
