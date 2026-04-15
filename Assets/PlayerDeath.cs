using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class lavaDeath : MonoBehaviour
{
    public AudioSource audio;
    public bool thisIslava;
    public lavaRise lr;
    private Transform lrT;

    public GameObject player;
    public Transform respawnPoint;
    private void Start()
    {
        if(lrT != null)
            lrT = lr.transform;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            if (thisIslava == false)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                audio.Play();
                print("restart");
            }
            else
            {
                lr.transform.position = lrT.position;
                lr.start = false;

                audio.Play();

                RespawnPlayer(player);
            }
        }
    }

    void RespawnPlayer(GameObject player)
    {
        CharacterController cc = player.GetComponent<CharacterController>();
        PlayerController pc = player.GetComponent<PlayerController>();

        // Disable controller before teleport (VERY IMPORTANT)
        if (cc != null) cc.enabled = false;

        // Move player
        player.transform.position = respawnPoint.position;
        player.transform.rotation = respawnPoint.rotation;

        // Reset player movement state
        if (pc != null)
        {
            pc._velocity = Vector3.zero;

            // These are private, so we reset via reflection workaround OR modify your script slightly
            // Recommended: make a reset function (see below)
            pc.SetExternalMomentum(Vector3.zero);
        }

        // Re-enable controller
        if (cc != null) cc.enabled = true;

        lr.ResetLava();
    }

}