using Mirror;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : NetworkBehaviour
{

    Vector2 move;
    Input inputAction;
    Rigidbody rb;
    [SerializeField] GameObject Ball;
    [SerializeField] float speed;
    TMP_Text scoreText;
    public static Action<int> OnUpdateScore;
    [SyncVar(hook = nameof(OnScoreChanged))]
    public int score = 0;
    [SyncVar] public int playerId;

    private void Awake()
    {
       Debug.Log(playerId);
       inputAction=new Input();
       inputAction.Enable();
       rb = GetComponent<Rigidbody>();
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        inputAction.Gameplay.Movement.started += Move;
        inputAction.Gameplay.Movement.canceled += CancelMove;
        OnUpdateScore += UpdateScore;
    }

    private void OnDisable()
    {
        inputAction.Gameplay.Movement.started -= Move;
        inputAction.Gameplay.Movement.canceled -= CancelMove;
        OnUpdateScore -= UpdateScore;
    }

    private void Move(InputAction.CallbackContext value)
    {
        if (!isLocalPlayer) return;
        move = new Vector2(0, value.ReadValue<float>() * speed);
        rb.linearVelocity = move;
    }

    [TargetRpc]
    public void TargetShowEndScreen(NetworkConnection target, bool isWinner)
    {

        if (isWinner)
        {
            GameManager.Instance.ShowWin();
        }
        else
        {
            GameManager.Instance.ShowLose();
        }
    }

    [Server]
    void CheckWinCondition()
    {
        if (score >= 5)
        {
            CustomNetworkManager.Instance.HandleWin(playerId);
        }
    }

    void OnScoreChanged(int oldvalue,int newValue)
    {
        if (scoreText == null)
        {
            scoreText = GameManager.Instance.scoreText[playerId];
        }
        scoreText.text = newValue.ToString();
    }

    private void UpdateScore(int playerid)
    {
        foreach (var player in CustomNetworkManager.Instance.Players)
        {
            if (player.playerId == playerid&&this.playerId==playerid)
            {
                player.score++;
                player.CheckWinCondition();
            }
        }
    }

    private void CancelMove(InputAction.CallbackContext value)
    {
        if (!isLocalPlayer) return;
        move = new Vector2(0, value.ReadValue<float>() * speed);
        rb.linearVelocity = move;
    }

}
