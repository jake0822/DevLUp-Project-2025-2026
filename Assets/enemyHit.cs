using UnityEngine;

public class enemyHit : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            print("hit the player");
    }
}
