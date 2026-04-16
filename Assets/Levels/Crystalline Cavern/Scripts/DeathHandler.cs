using System;
using UnityEngine;
using UnityEngine.UI;

public class DeathHandler : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject player;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private Transform doorRespawnPoint;

    [Header("Worm")]
    [SerializeField] private GameObject worm;
    [SerializeField] private Transform wormRespawnPoint;

    [Header("Other Objects")]
    [SerializeField] private GameObject keysObject;
    [SerializeField] private GameObject chaseTriggerObject;

    [Header("Fading")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1.0f;

    private float fadeTimer = -1;

    public void Death()
    {
        // Reset player keys
        PlayerKeys playerKeys = player.GetComponent<PlayerKeys>();

        Vector3 respawnPosition;
        if (playerKeys.HasAllKeys())
        {
            // Respawn in front of door (checkpoint)
            respawnPosition = doorRespawnPoint.position;
            chaseTriggerObject.SetActive(true);
        }
        else
        {
            // Respawn in front of entrance
            respawnPosition = respawnPoint.position;
            playerKeys.ResetKeys();

            // Re-enable any inactive keys
            foreach (Transform keyTransform in keysObject.transform)
            {
                keyTransform.gameObject.SetActive(true);
            }
        }
           
        // Teleport player to respawn point
        player.SetActive(false);
        player.transform.position = respawnPosition;
        player.SetActive(true);

        // Stop player dashing
        player.GetComponent<LightDash>().StopDashing();

        // Reset worm
        worm.SetActive(false);
        worm.transform.position = wormRespawnPoint.position;
        worm.SetActive(true);

        Worm wormScript = worm.GetComponent<Worm>();
        wormScript.ClearPastTransforms(wormRespawnPoint.position);
        wormScript.ToWanderState();

        // Do fade
        fadeTimer = 0;
    }

    void Update()
    {
        if (fadeTimer != -1)
        {
            fadeTimer = Mathf.MoveTowards(fadeTimer, fadeDuration, Time.deltaTime);
            float t = fadeTimer / fadeDuration;
            Color fadeColor = Color.black;
            fadeColor.a = 1 - t;
            fadeImage.color = fadeColor;

            if (fadeTimer == fadeDuration)
            {
                fadeTimer = -1;
            }
        }
    }
}
