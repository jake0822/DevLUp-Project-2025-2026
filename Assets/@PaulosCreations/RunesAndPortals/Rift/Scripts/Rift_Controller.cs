using System.Collections;
using UnityEngine;

public class Rift_Controller : MonoBehaviour
{
    //assigned in Inspector
    [Header("Applied to the effects at start")]
    [SerializeField] private Color effectsColor;

    [Header("Changing these might `break` the effects")]
    [Space(20)]
    [SerializeField] private Renderer meshRenderer;
    [SerializeField] private ParticleSystem[] effectsParticles;
    [SerializeField] private Light riftLight;
    [SerializeField] private AudioSource[] effectsAudio;

    private float maxIntLight = 4;
    private float transitionSpeed = 0.8f;

    private bool inTransition, activated, animating;
    private Material matInstance;
    private float fadeFloat;

    private Coroutine transitionCor, runeBlastCor;

    private void Awake()
    {
        matInstance = meshRenderer.material;
        matInstance.SetColor("_EmissionColor", effectsColor);
        matInstance.SetFloat("_EmissionStrength", 0);

        maxIntLight = riftLight.intensity;
        riftLight.intensity = 0f;
        riftLight.color = effectsColor;

        foreach (ParticleSystem part in effectsParticles)
        {
            ParticleSystem.MainModule mod = part.main;
            mod.startColor = effectsColor;
        }
    }

    public void F_ToggleRift(bool _activate)
    {
        if (inTransition || _activate == activated)
            return;

        if (_activate)//toggle on
        {
            activated = true;

            effectsParticles[0].Play();
            effectsParticles[1].Play();
            effectsParticles[2].Play();
            effectsParticles[3].Play();

            effectsAudio[0].Play();

            transitionCor = StartCoroutine(TransitionSequence());
            runeBlastCor = StartCoroutine(RuneBlasts());
        }
        else if (!_activate)//toggle off
        {
            activated = false;

            transitionCor = StartCoroutine(TransitionSequence());
            StopCoroutine(runeBlastCor);

            effectsParticles[0].Stop();
            effectsParticles[1].Stop();
            effectsParticles[2].Stop();
        }
    }

    private IEnumerator TransitionSequence()
    {
        inTransition = true;

        while (true)
        {
            if (activated)//transition to on
            {
                fadeFloat = Mathf.MoveTowards(fadeFloat, 1f, Time.deltaTime * transitionSpeed);

                if (fadeFloat >= 1f)//transition finished
                {
                    inTransition = false;
                    StopCoroutine(transitionCor);
                }
            }
            else //transition to off
            {
                fadeFloat = Mathf.MoveTowards(fadeFloat, 0f, Time.deltaTime * transitionSpeed);

                if (fadeFloat <= 0f)//transition finished
                {
                    effectsAudio[0].Stop();

                    inTransition = false;
                    StopCoroutine(transitionCor);
                }
            }

            effectsAudio[0].volume = fadeFloat * 0.8f;

            matInstance.SetFloat("_EmissionStrength", fadeFloat);

            riftLight.intensity = maxIntLight * fadeFloat;

            yield return null;
        }
    }

    private IEnumerator RuneBlasts()
    {
        ParticleSystem.MainModule partMain = effectsParticles[4].main;

        while (true)
        {
            effectsParticles[4].Stop();

            partMain.duration = Random.Range(0.8f, 1f);
            effectsParticles[4].Play();

            effectsAudio[1].pitch = Random.Range(0.85f, 0.9f);
            effectsAudio[1].Play();

            yield return new WaitForSeconds(Random.Range(2f, 6f));
        }
    }
}
