using UnityEngine;
using UnityEngine.Windows;


enum GlowState {
    GLOW,
    IDLE,
    UNGLOW
}


public class LightCrystal : MonoBehaviour {

    [SerializeField] private float lightIntensity = 20.0f;
    [SerializeField] private float glowAnimDuration = 0.5f;
    [ColorUsage(true, true)]
    [SerializeField] private Color emissionColor = Color.white;
    [SerializeField] private float lightRange = 25.0f;

    [SerializeField] private Renderer crystalMesh;


    private Light lightObject;
    private GlowState glowState = GlowState.IDLE;
    private float glowAnimTimer = 0.0f;
    private float baseLightIntensity;
    private float baseLightRange;
    
    private Color baseEmissionColor;

    void Start() {
        lightObject = GetComponentInChildren<Light>();
        baseLightIntensity = lightObject.intensity;
        baseEmissionColor = crystalMesh.material.GetColor("_EmissionColor");
        baseLightRange = lightObject.range;
    }

    void Update() {
        if (glowState != GlowState.IDLE) {
            glowAnimTimer = Mathf.Clamp(glowAnimTimer + Time.deltaTime, 0, glowAnimDuration);
            float t = Mathf.InverseLerp(0, glowAnimDuration, glowAnimTimer);

            if (glowState == GlowState.GLOW) {
                lightObject.intensity = Mathf.Lerp(baseLightIntensity, lightIntensity, t);
                lightObject.range = Mathf.Lerp(baseLightRange, lightRange, t);
                Color newEmissionColor = Color.Lerp(baseEmissionColor, emissionColor, t);
                crystalMesh.material.SetColor("_EmissionColor", newEmissionColor);
            } else if (glowState == GlowState.UNGLOW) {
                lightObject.intensity = Mathf.Lerp(lightIntensity, baseLightIntensity, t);
                lightObject.range = Mathf.Lerp(lightRange, baseLightRange, t);
                Color newEmissionColor = Color.Lerp(emissionColor, baseEmissionColor, t);
                crystalMesh.material.SetColor("_EmissionColor", newEmissionColor);
            }

            if (glowAnimTimer >= glowAnimDuration) {
                glowState = GlowState.IDLE;
            }
        }
    }

    public void StartGlow() {
        glowState = GlowState.GLOW;
        glowAnimTimer = 0.0f;
    }

    public void StopGlow() {
        glowState = GlowState.UNGLOW;
        glowAnimTimer = 0.0f;
    }
}
