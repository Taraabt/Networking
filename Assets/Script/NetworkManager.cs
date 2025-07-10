using Mirror;
using UnityEngine;
using System.Collections.Generic;

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
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(Instance);
        }
    }


    [Server]
    public void HandleWin(int winnerId)
    {
        foreach (var player in Players)
        {
            bool isWinner = player.playerId == winnerId;
            player.TargetShowEndScreen(player.connectionToClient, isWinner);
        }
    }

    [Server]
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {

        if (conn.identity != null)
        {
            return;
        }

        Transform spawn = spawnPoints[nextIndex % spawnPoints.Count];
        nextIndex++;

        GameObject playerObj = Instantiate(playerPrefab, spawn.position, spawn.rotation);
        Player player = playerObj.GetComponent<Player>();
        player.playerId = playerId;

        NetworkServer.AddPlayerForConnection(conn, playerObj);
        Players.Add(player);
        playerId++;
    }

    [Server]
    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName == "GameScene")
        {
            spawnPoints.Clear();
            foreach (var sp in FindObjectsOfType<SpawnPoint>())
            {
                spawnPoints.Add(sp.transform);
            }
            int i = 0;
            foreach (Player player in Players)
            {
                Transform spawn = spawnPoints[i % spawnPoints.Count];
                player.transform.position = spawn.position;
                player.transform.rotation = spawn.rotation;
                i++;
            }
        }
    }

}
