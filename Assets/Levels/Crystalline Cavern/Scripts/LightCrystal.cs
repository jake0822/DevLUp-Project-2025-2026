using UnityEngine;


enum GlowState {
    GLOW,
    IDLE,
    UNGLOW
}


public class LightCrystal : MonoBehaviour {

    [SerializeField] private float lightIntensity = 20.0f;
    [SerializeField] private float glowAnimDuration = 0.5f;

    [SerializeField] private Renderer crystalMesh;

    private Light lightObject;
    private GlowState glowState = GlowState.IDLE;
    private float glowAnimTimer = 0.0f;
    private float baseLightIntensity;
    
    private Color baseEmissionColor;


    float MapRange(float input, float lower, float upper, float toLower, float toUpper) {
        float t = Mathf.InverseLerp(lower, upper, input);
        return Mathf.Lerp(toLower, toUpper, t);
    }

    void Start() {
        lightObject = GetComponentInChildren<Light>();
        baseLightIntensity = lightObject.intensity;
        baseEmissionColor = crystalMesh.material.GetColor("_EmissionColor");
    }

    void Update() {
        if (glowState != GlowState.IDLE) {
            glowAnimTimer = Mathf.Clamp(glowAnimTimer + Time.deltaTime, 0, glowAnimDuration);

            if (glowState == GlowState.GLOW) {
                lightObject.intensity = MapRange(glowAnimTimer, 0, glowAnimDuration, baseLightIntensity, lightIntensity);
                float emission = MapRange(glowAnimTimer, 0, glowAnimDuration, 0, 3);
                crystalMesh.material.SetColor("_EmissionColor", baseEmissionColor * emission);
            } else if (glowState == GlowState.UNGLOW) {
                lightObject.intensity = MapRange(glowAnimTimer, 0, glowAnimDuration, lightIntensity, baseLightIntensity);
                float emission = MapRange(glowAnimTimer, 0, glowAnimDuration, 3, 0);
                crystalMesh.material.SetColor("_EmissionColor", baseEmissionColor * emission);
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
