using Mirror;
using UnityEngine;

public class Ball : NetworkBehaviour
{

    Rigidbody rb;
    [SerializeField] float speed;

    private void Start()
    {
        rb= GetComponent<Rigidbody>();
        int random = Random.Range(-1, 2);
        while (random==0)
        {
            random = Random.Range(-1, 2);
        }
        Debug.Log(random);
;       rb.linearVelocity = new Vector3(random*speed,0,0);
    }

}
