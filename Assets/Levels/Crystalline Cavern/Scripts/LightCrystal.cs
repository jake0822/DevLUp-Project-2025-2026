using UnityEngine;


enum GlowState {
    GLOW,
    IDLE,
    UNGLOW
}


public class LightCrystal : MonoBehaviour {

    public float lightIntensity = 20.0f;
    public float glowAnimDuration = 0.5f;

    private Light lightObject;
    private GlowState glowState = GlowState.IDLE;
    private float glowAnimTimer = 0.0f;


    float MapRange(float input, float lower, float upper, float toLower, float toUpper) {
        float t = Mathf.InverseLerp(lower, upper, input);
        return Mathf.Lerp(toLower, toUpper, t);
    }

    void Start() {
        lightObject = GetComponentInChildren<Light>();
    }

    void Update() {
        if (glowState != GlowState.IDLE) {
            glowAnimTimer = Mathf.Clamp(glowAnimTimer + Time.deltaTime, 0, glowAnimDuration);

            if (glowState == GlowState.GLOW) {
                lightObject.intensity = MapRange(glowAnimTimer, 0, glowAnimDuration, 0, lightIntensity);
            } else if (glowState == GlowState.UNGLOW) {
                lightObject.intensity = MapRange(glowAnimTimer, 0, glowAnimDuration, lightIntensity, 0);
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
