using System;
using UnityEngine;
using UnityEngine.UI;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform respawnPoint;

    [SerializeField] private GameObject keysObject;

    [SerializeField] private GameObject worm;
    [SerializeField] private Transform wormRespawnPoint;

    [Header("Fading")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1.0f;

    private float fadeTimer = -1;

    public void Death()
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
