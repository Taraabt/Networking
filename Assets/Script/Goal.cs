using UnityEngine;

public class Goal : MonoBehaviour
{

    [SerializeField] int playerId;

    private void OnTriggerEnter(Collider other)
    {
        Player.OnUpdateScore?.Invoke(playerId);
    }
}

