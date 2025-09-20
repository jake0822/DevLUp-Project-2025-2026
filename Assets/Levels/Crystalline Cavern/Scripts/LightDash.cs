using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class LightDash : MonoBehaviour {
    public CharacterController playerController;
    public CinemachineCamera playerFpsCamera;
    public Transform playerFacingTransform;

    [Header("Dash Settings")]
    public int dashDuration = 40;  // The number of frames the player should dash for
    public float dashStrength = 0.3f;  // The strength of the dash
    public AnimationCurve dashStrengthCurve;  // Determines how the strength changes over the course of the dash

    public float fovChange = 5.0f;  // How many degrees the FOV changes during a dash
    public AnimationCurve fovCurve;  // Determines how the fov changes over the course of the dash

    // Dash data
    private float baseFov = 60;

    private bool hasDash = false;
    private bool isDashing = false;
    private int dashTimer = 0;
    private Vector3 dashVector;

    void Start() {
        baseFov = playerFpsCamera.Lens.FieldOfView;
    }

    void Update() {
        if (isDashing) {
            float t = dashTimer / (float) dashDuration;
            float dashStrengthT = dashStrengthCurve.Evaluate(t);
            playerController.Move(dashVector * dashStrengthT);

            float fovT = fovCurve.Evaluate(t);
            float fov = Mathf.Lerp(baseFov + fovChange, baseFov, fovT);
            playerFpsCamera.Lens.FieldOfView = fov;

            dashTimer++;
            if (dashTimer >= dashDuration) {
                isDashing = false;
            }
        }
        else if (playerController.isGrounded) {
            hasDash = true;
        }
    }

    public void Dash(InputAction.CallbackContext context) {
        if (hasDash && context.performed) {
            hasDash = false;
            isDashing = true;
            dashTimer = 0;
            dashVector = playerFacingTransform.rotation * Vector3.forward * dashStrength;
        }
    }
}
