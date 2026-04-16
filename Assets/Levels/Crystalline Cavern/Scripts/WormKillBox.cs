using UnityEngine;

public class WormKillBox : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform respawnPoint;

    [SerializeField] private GameObject keysObject;

    [SerializeField] private GameObject worm;
    [SerializeField] private Transform wormRespawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            // Move player to respawn point
            player.SetActive(false);
            player.transform.position = respawnPoint.position;
            player.SetActive(true);

            // Stop player dashing
            player.GetComponent<LightDash>().StopDashing();

            // Reset player keys
            PlayerKeys playerKeys = player.GetComponent<PlayerKeys>();
            playerKeys.ResetKeys();

            // Re-enable any inactive keys
            foreach (Transform keyTransform in keysObject.transform)
            {
                keyTransform.gameObject.SetActive(true);
            }

            // Reset worm
            worm.SetActive(false);
            worm.transform.position = wormRespawnPoint.position;
            worm.SetActive(true);

            Worm wormScript = worm.GetComponent<Worm>();
            wormScript.ClearPastTransforms(wormRespawnPoint.position);
            wormScript.ToWanderState();
        }
    }
}
