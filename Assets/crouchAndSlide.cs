using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class crouchAndSlide : MonoBehaviour
{
    public Transform playerTransform;
    public PlayerController player;
    public CharacterController cc;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f; // how far down to crouch
    public float crouchSpeed = 8f; // how fast to go between stand and crouch
    public float speedWhenCrouched = 4;

    private bool _isCrouched = false;
    private float defaultSpeed;
    private Vector3 standingPos;
    private Vector3 crouchedPos;

    private void Start()
    {
        defaultSpeed = player.speed;
        standingPos = playerTransform.localPosition;
        crouchedPos = standingPos + Vector3.down * crouchHeight;
    }

    public void Crouch(InputAction.CallbackContext context) //detects input for couch
    {
        
        if (context.performed) // button pressed
        {
            _isCrouched = true;
            player.speed = speedWhenCrouched;
        }
        else if (context.canceled) // button released
        {
            _isCrouched = false;
            player.speed = defaultSpeed;
        }
         
    }

    private void Update()
    {
        if (player._grounded) // only allow crouching while grounded
        {
            player.speed = _isCrouched ? speedWhenCrouched : defaultSpeed;
            Vector3 target = _isCrouched ? crouchedPos : standingPos; //set speed and target based on is crouched

            
            playerTransform.localPosition = Vector3.Lerp( //Lerp moves head smoothly to crouched position
                playerTransform.localPosition,
                target,
                Time.deltaTime * crouchSpeed
            );
        }

    }
}
