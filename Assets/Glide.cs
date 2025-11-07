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

    private void Update()
    {
        //intput logic
        if (glidePressed && player._velocity.y < 0 && !player._grounded)
        {
            gliding = true;

        }
        else
            gliding = false;

        print(gliding);

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


    }

}
