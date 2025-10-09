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

    private void Start()
    {
        ogGravity = player.gravity;
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
            player._velocity.y = -1;
            player.gravity = 0;

        }
        else 
        { 
            player.gravity = ogGravity;
        }


    }

}
