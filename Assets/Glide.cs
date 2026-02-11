using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Glide : MonoBehaviour
{
    public PlayerController player;
    public CharacterController cc;

    private bool glidePressed = false;
    private bool gliding;

    private float ogGravity;
    private float ogSpeed;

    private bool canGlideBoost = false;

    [Header("Glide Boost")]
    [SerializeField] private float boostForce = 5;
    [SerializeField] private float boostUpwardForce = 30f;
    [SerializeField] private float boostDuration = 0.2f;
    [SerializeField] private float boostDrag = 5f;

    private float boostTimer;
    private Vector3 boostVelocity;
    private bool boosting;


    private void Start()
    {
        ogGravity = player.gravity;
        ogSpeed = player.speed;
    }

    public void glideInput(InputAction.CallbackContext context) //detects input for crouch
    {

        if (context.performed) // button pressed
        {
            glidePressed = true;
        }
        else if (context.canceled) // button released
        {
            glidePressed=false;
        }

    }
    public void GlideBoost(InputAction.CallbackContext context) //detects input for crouch
    {

        if (context.performed && !player._grounded && canGlideBoost)
        {
            print("glide boost");
            canGlideBoost = false;
            boosting = true;
            boostTimer = boostDuration;

            // Forward direction boost
            Vector3 forward = transform.forward;

            boostVelocity = forward * boostForce;
            boostVelocity.y = boostUpwardForce; // small lift
        }
    }

    private void Update()
    {
        if (player._grounded) 
        {
            canGlideBoost = true;
           
        }


        //intput logic
        if (glidePressed && player._velocity.y < 0 && !player._grounded)
        {
            gliding = true;

        }
        else
            gliding = false;

        //print(gliding);

        //gliding logic

        if (gliding)
        {
            player._velocity.y = -1;    // Slow down falling speed
            player.gravity = 0;         // No gravity
            player.speed = ogSpeed * 2; // Increase horizontal speed when gliding
        }
        else 
        { 
            // Reset player gravity and horizontal speed
            player.gravity = ogGravity;
            player.speed = ogSpeed;
        }

        // Apply boost movement
        if (boosting)
        {
            boostTimer -= Time.deltaTime;

            // Apply boost velocity
            player._velocity += boostVelocity * Time.deltaTime;

            // Smooth decay
            boostVelocity = Vector3.Lerp(boostVelocity, Vector3.zero, boostDrag * Time.deltaTime);

            if (boostTimer <= 0f)
            {
                boosting = false;

                // 🔥 Reset horizontal velocity so no drifting remains
                player._velocity.x = 0f;
                player._velocity.z = 0f;

                // Optional: also clear boostVelocity completely
                boostVelocity = Vector3.zero;
            }
        }


    }

}
