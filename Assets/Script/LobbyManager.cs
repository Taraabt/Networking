using Mirror;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public Button startGameButton;
    public TMP_InputField ipInputField;
    public Button hostButton;
    public Button joinButton;

    private void Start()
    {
        hostButton.onClick.AddListener(HostGame);
        joinButton.onClick.AddListener(JoinGame);
        startGameButton.onClick.AddListener(StartGame);
        startGameButton.gameObject.SetActive(false);
    }

    void HostGame()
    {
        CustomNetworkManager.Instance.StartHost();
        StartCoroutine(ShowStartButtonIfHost());
    }

    void JoinGame()
    {
        string ip = ipInputField.text;
        CustomNetworkManager.Instance.networkAddress = ip;
        CustomNetworkManager.Instance.StartClient();
    }
    IEnumerator ShowStartButtonIfHost()
    {
        yield return new WaitUntil(() => NetworkClient.isConnected);

        if (NetworkServer.active && NetworkClient.isConnected)
        {
            startGameButton.gameObject.SetActive(true);
        }
    }

    void StartGame()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            CustomNetworkManager.Instance.ServerChangeScene("GameScene");
        }
    }

}