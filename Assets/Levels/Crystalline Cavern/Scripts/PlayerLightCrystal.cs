using UnityEngine;
using UnityEngine.InputSystem;

/*
    Controls player interations with light crystals.
*/
public class PlayerLightCrystal : MonoBehaviour {
    private const string LIGHT_CRYSTAL_TAG = "LightCrystal";
    private const string CRYSTAL_OUTLINE_LAYER = "CrystalOutline";

    [Header("Raycast Settings")]
    public float rayDistance = 100f;

    [Header("Animation Settings")]
    public float spriteMoveTime = 1.0f;
    public AnimationCurve spriteMoveCurve;

    private Camera playerCamera;
    private GameObject currentLookedAtObject = null;
    private GameObject inhabitedCrystal = null;
    private bool spriteIsMoving = false;

    private LightSprite spriteController;

    void Start() {
        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null) {
            playerCamera = Camera.main;
        }
        spriteController = GetComponent<LightSprite>();
    }

    void Update() {
        CastLookRay();
    }

    void CastLookRay() {
        Ray lookRay = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(lookRay, out hit, rayDistance)) {
            if (hit.collider.CompareTag(LIGHT_CRYSTAL_TAG)) {
                if (currentLookedAtObject != hit.collider.gameObject) {
                    LookAtCrystal(hit.collider.gameObject);
                    currentLookedAtObject = hit.collider.gameObject;
                }
            }
            else {
                if (currentLookedAtObject != null) {
                    StopLookAtCrystal(currentLookedAtObject);
                    currentLookedAtObject = null;
                }
            }
        }
        else {
            if (currentLookedAtObject != null) {
                StopLookAtCrystal(currentLookedAtObject);
                currentLookedAtObject = null;
            }
        }
    }

    void LookAtCrystal(GameObject targetObject) {
        targetObject.layer = LayerMask.NameToLayer(CRYSTAL_OUTLINE_LAYER);
    }

    void StopLookAtCrystal(GameObject targetObject) {
        targetObject.layer = LayerMask.NameToLayer("ground");
    }

    public void OnClick(InputAction.CallbackContext context) {
        if (context.performed) {
            if (spriteIsMoving) {
                return;
            }

            bool hasSprite = spriteController.HasSprite();
            if (hasSprite) {
                if (currentLookedAtObject != null) {
                    // Throw the sprite into the crystal
                    inhabitedCrystal = currentLookedAtObject;
                    spriteIsMoving = true;
                    spriteController.RemoveSprite();
                    spriteController.MoveSprite(inhabitedCrystal.transform, spriteMoveTime, spriteMoveCurve, () => {
                        LightCrystal crystal = inhabitedCrystal.GetComponent<LightCrystal>();
                        crystal.StartGlow();
                        spriteIsMoving = false;
                    });
                }
            }
            else {
                // Return the sprite back to the player
                spriteIsMoving = true;
                spriteController.GiveSprite();
                spriteController.ReturnSprite(spriteMoveTime, spriteMoveCurve, () => {
                    spriteIsMoving = false;
                    Debug.Log(spriteIsMoving);
                });
                LightCrystal crystal = inhabitedCrystal.GetComponent<LightCrystal>();
                crystal.StopGlow();
                inhabitedCrystal = null;
            }
        }
    }
}
