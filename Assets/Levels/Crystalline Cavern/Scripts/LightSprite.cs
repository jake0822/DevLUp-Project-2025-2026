using System;
using UnityEngine;


/*
    Handles the state of the light sprite and controls
    the intensity of its light.
*/
public class LightSprite : MonoBehaviour {
    private bool hasSprite = true;

    [SerializeField] private Transform spriteParent;
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private Transform spriteReturnTransform;

    [Tooltip("The radius at which the sprite will be detected by the worm")]
    [SerializeField] private float spriteDetectionRadius = 10.0f;

    private Light spriteLight;

    private Vector3 animStartPos;
    private Transform animEndTransform;
    private float animTimer = -1f;
    private float animDuration;
    private AnimationCurve animCurve;
    private Action animFinishCallback = null;

    private float lightAnimTimer = -1f;
    private float lightAnimBaseIntensity;
    private float lightAnimTargetIntensity;
    private float lightAnimDuration;
    private AnimationCurve lightAnimCurve;

    public bool HasSprite() {
        return hasSprite;
    }

    public void GiveSprite() {
        hasSprite = true;
    }

    public void RemoveSprite() {
        hasSprite = false;
    }

    public void MoveSprite(Transform destination, float duration, AnimationCurve curve, Action arriveCallback = null) {
        animTimer = 0f;
        animStartPos = spriteTransform.position;
        animEndTransform = destination;
        animDuration = duration;
        animCurve = curve;
        animFinishCallback = arriveCallback;
        spriteTransform.rotation = destination.rotation;
        spriteTransform.SetParent(null);  // Detach the sprite from the player
    }

    // Returns the sprite back to the player
    public void ReturnSprite(float duration, AnimationCurve curve, Action returnCallback = null) {
        MoveSprite(spriteReturnTransform, duration, curve, () => {
            spriteTransform.SetParent(spriteParent);  // Reattach the sprite to the player
            returnCallback?.Invoke();
        });
    }

    public void GlowSprite(float intensity, float duration, AnimationCurve curve) {
        lightAnimTimer = 0f;
        lightAnimBaseIntensity = spriteLight.intensity;
        lightAnimTargetIntensity = intensity;
        lightAnimDuration = duration;
        lightAnimCurve = curve;
    }

    public bool IsDetected(Vector3 point) {
        return Vector3.Distance(point, spriteTransform.position) <= spriteDetectionRadius;
    }

    public Vector3 GetPosition() {
        return spriteTransform.position;
    }

    void Start() {
        spriteLight = GetComponentInChildren<Light>();
    }

    void Update() {
        if (animTimer != -1f) {
            float t = animCurve.Evaluate(animTimer / animDuration);
            spriteTransform.position = Vector3.Lerp(animStartPos, animEndTransform.position, t);

            animTimer += Time.deltaTime;
            if (animTimer >= animDuration) {
                animTimer = -1f;
                animFinishCallback?.Invoke();
            }
        }

        if (lightAnimTimer != -1f) {
            float t = lightAnimCurve.Evaluate(lightAnimTimer / lightAnimDuration);
            float intensityDifference = lightAnimTargetIntensity - lightAnimBaseIntensity;
            spriteLight.intensity = t * intensityDifference + lightAnimBaseIntensity;

            lightAnimTimer += Time.deltaTime;
            if (lightAnimTimer >= lightAnimDuration) {
                lightAnimTimer = -1f;
                spriteLight.intensity = lightAnimBaseIntensity;
            }
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = new Color(1.0f, 1.0f, 0.3f, 0.2f);
        Gizmos.DrawSphere(spriteTransform.position, spriteDetectionRadius);
    }
}
