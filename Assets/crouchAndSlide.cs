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
    public float speedToSlide = 5;
    public float slideDecay = 5f;

    private bool _isCrouched = false;
    private bool _isSliding;
    private float defaultSpeed;
    private Vector3 standingPos;
    private Vector3 crouchedPos;

    private Vector3 slideDirection;

    private void Start()
    {
        defaultSpeed = player.speed;
        standingPos = playerTransform.localPosition;
        crouchedPos = standingPos + Vector3.down * crouchHeight;
    }

    public void Crouch(InputAction.CallbackContext context) //detects input for crouch
    {
        
        if (context.performed) // button pressed
        {
            _isCrouched = true;
        }
        else if (context.canceled) // button released
        {
            _isCrouched = false;
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

            if (cc.velocity.magnitude >= speedToSlide && _isCrouched && !_isSliding)
            {
                beginSlide();
            }
        }

        if (_isSliding)
        {
            sliding();
        }
    }

    private void beginSlide()
    {
        _isSliding = true;
        slideDirection = new Vector3(cc.velocity.x, 0, cc.velocity.z).normalized;
        slideDirection *= (player.speed + 12);

        
    }

    private void sliding()
    {
        cc.Move(slideDirection * Time.deltaTime);
        
        
        
        slideDirection = Vector3.Lerp(slideDirection, Vector3.zero, slideDecay * Time.deltaTime);
        if (slideDirection.magnitude < 0.1f || !_isCrouched)
        {
            _isSliding = false;
            slideDirection = Vector3.zero;
        }
        player.SetExternalMomentum(slideDirection);
    }
    
}
