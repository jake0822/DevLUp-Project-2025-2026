using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFade : MonoBehaviour
{
    [Header("Text")]
    public TMP_Text textMesh;
    [TextArea]
    public List<string> messages = new List<string>();

    [Header("Timing")]
    public DialogManager manager;
    public float fadeDuration = 1f;
    public float waitTime = 2f;

    [Header("Audio")]
    public AudioClip fadeOutSound;
    private AudioSource audioSource;

    private bool isPlaying = false;
    bool didit;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // Start fully invisible
        Color c = textMesh.color;
        textMesh.color = new Color(c.r, c.g, c.b, 0f);
    }
    private void Update()
    {
        if (manager.hasCompletedDialgue() && didit == false)
        {
            PlaySequence();
            didit = true;
        }
    }

    // 👇 Call THIS from another script
    public void PlaySequence()
    {
       
        if (!isPlaying && messages.Count > 0)
        {
            StartCoroutine(SequenceCoroutine());
        }
    }

    IEnumerator SequenceCoroutine()
    {
        yield return new WaitForSeconds(1.9f);
        isPlaying = true;

        for (int i = 0; i < messages.Count; i++)
        {
            // Set text
            textMesh.text = messages[i];

            // Fade in
            yield return StartCoroutine(FadeText(0f, 1f));

            // Wait
            yield return new WaitForSeconds(waitTime);

            // Play sound before fade out
            if (fadeOutSound != null)
            {
                audioSource.PlayOneShot(fadeOutSound);
            }

            // Fade out
            yield return StartCoroutine(FadeText(1f, 0f));
        }

        isPlaying = false;
    }

    IEnumerator FadeText(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color color = textMesh.color;

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            textMesh.color = new Color(color.r, color.g, color.b, alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure final value
        textMesh.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}
