using System;
using TreeEditor;
using UnityEngine;


/*
    Handles the state of the light sprite and controls the intensity
    of the light attached to the player.
*/
public class LightSprite : MonoBehaviour {
    private bool hasSprite = true;

    private Transform spriteTransform;
    private Transform spriteParent;
    private Transform spriteReturnTransform;

    private Vector3 animStartPos;
    private Transform animEndTransform;
    private float animTimer = -1f;
    private float animDuration;
    private AnimationCurve animCurve;
    private Action animFinishCallback = null;

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
        spriteTransform.SetParent(null);  // Detach the sprite from the player
    }

    // Returns the sprite back to the player
    public void ReturnSprite(float duration, AnimationCurve curve) {
        MoveSprite(spriteReturnTransform, duration, curve, () => {
            spriteTransform.SetParent(spriteParent);  // Reattach the sprite to the player
        });
    }

    void Start() {
        spriteParent = GetComponent<Transform>();
        spriteTransform = transform.Find("Sprite");
        spriteReturnTransform = transform.Find("SpritePosition");
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
    }
}
