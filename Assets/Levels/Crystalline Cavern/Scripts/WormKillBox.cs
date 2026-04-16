using UnityEngine;

public class WormKillBox : MonoBehaviour
{
    [SerializeField] private DeathHandler deathHandler;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            deathHandler.Death();
        }
    }
}
