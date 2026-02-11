using System.Collections;
using UnityEngine;

public class Vortex_Controller : MonoBehaviour
{
    //assigned in Inspector
    [Header("Applied to the effects at start")]
    [SerializeField] private Color effectsColor;

    [Header("Changing these might `break` the effects")]
    [Space(20)]
    [SerializeField] private Renderer[] meshRenderers;
    [SerializeField] private Transform rocksBaseTF;
    [SerializeField] private ParticleSystem[] effectsParticles;
    [SerializeField] private Light vortexLight;
    [SerializeField] private AudioSource[] effectsAudio;

    private float maxIntLight = 4;
    private float transitionSpeed = 0.6f;

    //assigned when Awake
    private Transform[] rocks = new Transform[2];
    private bool inTransition, activated, animating;
    private Material matVortexInstance, matRocksInstance;
    private float fadeFloat;

    private Coroutine transitionCor, animateRocksCor;

    private void Awake()
    {
        matVortexInstance = meshRenderers[0].GetComponent<Renderer>().material;//runepile
        meshRenderers[1].material = matVortexInstance;//sandpit
        matVortexInstance.SetColor("_EmissionColor", effectsColor);
        matVortexInstance.SetFloat("_EmissionStrength", 0);

        matRocksInstance = meshRenderers[2].GetComponent<Renderer>().material;//rock1
        meshRenderers[3].material = matRocksInstance;//rock2
        matRocksInstance.SetColor("_EmissionColor", effectsColor);
        matRocksInstance.SetFloat("_EmissionStrength", 0);

        rocks[0] = rocksBaseTF.GetChild(0).transform;
        rocks[1] = rocksBaseTF.GetChild(1).transform;

        vortexLight.color = effectsColor;
        maxIntLight = vortexLight.intensity;
        vortexLight.intensity = 0f;

        foreach (ParticleSystem part in effectsParticles)
        {
            ParticleSystem.MainModule mod = part.main;
            mod.startColor = effectsColor;
        }
    }

    public void F_ToggleVortex(bool _activate)
    {
        if (inTransition || _activate == activated)
            return;

        if (_activate)//toggle on
        {
            activated = true;

            foreach (ParticleSystem part in effectsParticles)
            {
                part.Play();
            }

            foreach (AudioSource audioS in effectsAudio)
            {
                audioS.Play();
            }

            transitionCor = StartCoroutine(TransitionSequence());
            animateRocksCor = StartCoroutine(RocksAnimation());
        }
        else if (!_activate)//toggle off
        {
            activated = false;

            foreach (ParticleSystem part in effectsParticles)
            {
                part.Stop();
            }

            transitionCor = StartCoroutine(TransitionSequence());
            StopCoroutine(animateRocksCor);
        }
    }

    private IEnumerator TransitionSequence()
    {
        inTransition = true;
        Vector3 rocksBasePos = rocksBaseTF.localPosition;

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
                    foreach (AudioSource audioS in effectsAudio)
                    {
                        audioS.Stop();
                    }

                    inTransition = false;
                    StopCoroutine(transitionCor);
                }
            }

            effectsAudio[1].volume = fadeFloat * 0.8f;
            effectsAudio[2].volume = fadeFloat * 0.8f;
            effectsAudio[3].volume = fadeFloat * 0.2f;

            matVortexInstance.SetFloat("_EmissionStrength", fadeFloat * 0.4f);
            matRocksInstance.SetFloat("_EmissionStrength", fadeFloat * 0.4f);

            vortexLight.intensity = maxIntLight * fadeFloat;

            rocksBasePos[1] = fadeFloat * 0.5f;
            rocksBaseTF.localPosition = rocksBasePos;

            yield return null;
        }
    }

    private IEnumerator RocksAnimation()
    {
        Vector3[] rocksPos = new Vector3[2];
        rocksPos[0] = rocks[0].localPosition;
        rocksPos[1] = rocks[1].localPosition;

        while (true)
        {
            for (int i = 0; i < 2; i++)
            {
                rocks[i].localRotation = Random.rotation;
                rocks[i].localPosition = rocksPos[i] + Random.onUnitSphere * 0.05f;
            }

            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
        }
    }
}
