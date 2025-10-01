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
            float t = 0.0f;
            if (glowState == GlowState.GLOW) {
                t = MapRange(glowAnimTimer, 0, glowAnimDuration, 0, 1);
            } else if (glowState == GlowState.UNGLOW) {
                t = MapRange(glowAnimTimer, 0, glowAnimDuration, 1, 0);
            }

            lightObject.intensity = t * lightIntensity;

            glowAnimTimer += Time.deltaTime;
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
