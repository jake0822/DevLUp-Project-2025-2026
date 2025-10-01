using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class LightDash : MonoBehaviour {
    public CharacterController characterController;
    public CinemachineCamera playerFpsCamera;
    public Transform playerFacingTransform;

    [Header("Dash Settings")]
    public float dashDuration = 1.0f;  // The amount of time the player should dash for
    public float dashStrength = 0.1f;  // The strength of the dash
    public AnimationCurve dashStrengthCurve;  // Determines how the strength changes over the course of the dash

    public float fovChange = 5.0f;  // How many degrees the FOV changes during a dash
    public AnimationCurve fovCurve;  // Determines how the fov changes over the course of the dash

    // Dash data
    private float baseFov = 60;

    private bool canDash = false;
    private bool isDashing = false;
    private float dashTimer = 0.0f;
    private Vector3 dashVector;

    void Start() {
        baseFov = playerFpsCamera.Lens.FieldOfView;
    }

    void Update() {
        if (isDashing) {
            float t = dashTimer / dashDuration;
            float dashStrengthT = dashStrengthCurve.Evaluate(t) * Time.deltaTime;
            characterController.Move(dashVector * dashStrengthT);

            float fovT = fovCurve.Evaluate(t);
            float fov = Mathf.Lerp(baseFov + fovChange, baseFov, fovT);
            playerFpsCamera.Lens.FieldOfView = fov;

            dashTimer += Time.deltaTime;
            if (dashTimer >= dashDuration) {
                isDashing = false;
            }
        }
        else if (characterController.isGrounded) {
            canDash = true;
        }
    }

    public void Dash(InputAction.CallbackContext context) {
        if (canDash && context.performed) {
            canDash = false;
            isDashing = true;
            dashTimer = 0.0f;
            dashVector = playerFacingTransform.rotation * Vector3.forward * dashStrength;
        }
    }
}
