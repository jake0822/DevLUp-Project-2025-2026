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

    [Header("Boost Decay")]
    [SerializeField] private float boostControlLock = 0.2f; // how long boost stays strong
    [SerializeField] private float airDamping = 4f;         // how fast horizontal slows after

    private float boostLockTimer;
    private bool boostActive;

    public bool inGyser;



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
            canGlideBoost = false;

            Vector3 forward = transform.forward;
            Vector3 launch = (forward * boostForce) + (Vector3.up * boostUpwardForce);

            player._velocity += launch;

            boostActive = true;
            boostLockTimer = boostControlLock;
        }
    }

    private void Update()
    {
        if (boostActive)
        {
            boostLockTimer -= Time.deltaTime;

            if (boostLockTimer <= 0f)
            {
                // Smoothly damp horizontal velocity in air
                if (!player._grounded)
                {
                    Vector3 horizontal = new Vector3(player._velocity.x, 0f, player._velocity.z);
                    horizontal = Vector3.Lerp(horizontal, Vector3.zero, airDamping * Time.deltaTime);

                    player._velocity.x = horizontal.x;
                    player._velocity.z = horizontal.z;
                }
                else
                {
                    // Fully stop once grounded
                    player._velocity.x = 0f;
                    player._velocity.z = 0f;
                    boostActive = false;
                }

                // Stop tracking once nearly zero
                if (new Vector3(player._velocity.x, 0f, player._velocity.z).magnitude < 0.1f)
                {
                    boostActive = false;
                }
            }
        }


        if (player._grounded)
        {
            canGlideBoost = true;

        }


        //intput logic
        if (glidePressed && !player._grounded)
        {
            gliding = true;

        }
        else
            gliding = false;

        //print(gliding);

        //gliding logic
        if (gliding)
        {
            if (inGyser)  // Gliding and in gyser; make gravity positive to move player upwards
            {
                player.gravity = 8;
            }
            else {  // Gliding, but not in gyser
                if (player._velocity.y < 0)  // Normal gliding behavior
                {
                    player._velocity.y = -1;    // Slow down falling speed
                    player.gravity = 0;         // No gravity
                    player.speed = ogSpeed * 2; // Increase horizontal speed when gliding
                }
                else  // Just been launched out of gyser
                {
                    player.gravity = -13;
                }
            }
        }
        else
        {
            // Reset player gravity and horizontal speed
            player.gravity = ogGravity;
            player.speed = ogSpeed;
        }

        //if (gliding && !inGyser)
        //{
        //    player._velocity.y = -1;    // Slow down falling speed
        //    player.gravity = 0;         // No gravity
        //    player.speed = ogSpeed * 2; // Increase horizontal speed when gliding
        //}

        //if (!gliding)
        //{
        //    // Reset player gravity and horizontal speed
        //    player.gravity = ogGravity;
        //    player.speed = ogSpeed;
        //}
    }

}
