using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    public GameObject winUI;
    public GameObject loseUI;
    public static GameManager Instance;
    public TMP_Text[] scoreText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    public void ShowWin()
    {
        winUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowLose()
    {
        loseUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void BackToLobby()
    {
        SceneManager.LoadScene(0);
    }

}
