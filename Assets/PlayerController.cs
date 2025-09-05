using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float jumpDelay = 0.5f;
    [SerializeField] protected float jumpHeight = 2f;
    [SerializeField] protected float gravity = -9.8f;
    [SerializeField] protected float groundDeceleration, airDeceleration, groundAcceleration, airAcceleration;

    public new Transform camera;

    protected CharacterController _controller;
    protected CapsuleCollider _collider;
    private Vector2 _moveInput;
    private Vector3 _velocity;
    private Vector3 _horizontalVelocity = Vector3.zero;

    private float castHeight;
    private float radius;

    protected bool _grounded = false;
    public bool coyoteJump = false; //allows buffered jump


    private void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
        _controller = GetComponent<CharacterController>();
        
        castHeight = (_collider.height / 2 * transform.localScale.y)-_collider.height*0.25f;
        radius = _collider.radius;

    }

    private bool isGrounded() //This is raycast based grounded function. It creates four raycasts below the player to detect the ground
    {
        bool isGrounded = Physics.Raycast(transform.position - new Vector3(radius, castHeight),
            Vector3.down, 0.6f, LayerMask.GetMask("ground"));
        Debug.DrawRay(transform.position - new Vector3(radius, castHeight),
            Vector3.down *0.6f, Color.lawnGreen );
        if (isGrounded)
            return isGrounded;
        
        isGrounded = Physics.Raycast(transform.position - new Vector3(-radius, castHeight),
            Vector3.down, 0.6f, LayerMask.GetMask("ground"));
        Debug.DrawRay(transform.position - new Vector3(-radius, castHeight),
            Vector3.down *0.6f, Color.lawnGreen );
        if (isGrounded)
            return isGrounded;
        
        isGrounded = Physics.Raycast(transform.position - new Vector3(0, castHeight, radius),
            Vector3.down, 0.6f, LayerMask.GetMask("ground"));
        Debug.DrawRay(transform.position - new Vector3(0, castHeight, radius),
            Vector3.down *0.6f, Color.lawnGreen );
        if (isGrounded)
            return isGrounded;
        
        isGrounded = Physics.Raycast(transform.position - new Vector3(0, castHeight, -radius),
            Vector3.down, 0.6f, LayerMask.GetMask("ground"));
        Debug.DrawRay(transform.position - new Vector3(0, castHeight, -radius),
            Vector3.down *0.6f, Color.lawnGreen );
        
        return isGrounded;
    }

    private void Update() //this function runs once every frame
    {
        _grounded = isGrounded(); //checks if the player is grounded
        if (_grounded && coyoteJump) //checks if need to do coyote jump
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); //applies jump force for coyote jump
        }
        if (!_grounded)
            _velocity.y += gravity * Time.deltaTime; //calculates gravity
        _controller.Move(_velocity * Time.deltaTime); //applies gravity
        

        Vector3 forward = camera.forward; //code for keeping the same direction forward when you turn
        Vector3 right = camera.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        
        Vector3 moveDirection = forward * _moveInput.y + right * _moveInput.x;
        
        float accel = _grounded ? groundAcceleration : airAcceleration;
        float decel = _grounded ? groundDeceleration : airDeceleration;
        
        if (moveDirection.magnitude > 0f)
        {
            // Accelerate toward target velocity
            Vector3 targetVelocity = moveDirection * speed;
            _horizontalVelocity = Vector3.MoveTowards(_horizontalVelocity, targetVelocity, accel * Time.deltaTime);
        }
        else
        {
            // Decelerate to stop
            _horizontalVelocity = Vector3.MoveTowards(_horizontalVelocity, Vector3.zero, decel * Time.deltaTime);
        }

        // --- Combine horizontal + vertical ---
        Vector3 finalVelocity = _horizontalVelocity + Vector3.up * _velocity.y;

        // --- Move controller ---
        _controller.Move(finalVelocity * Time.deltaTime);
        
        //_controller.Move(moveDirection * speed * Time.deltaTime);

        // Rotate player to face same direction as camera 
        transform.rotation = Quaternion.Euler(0f, camera.eulerAngles.y, 0f);
    }


    public void Move(InputAction.CallbackContext context) //detects input for movement
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context) //detects input for jump
    {
        
        if (_grounded && context.performed)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); //applies jump force
            coyoteJump = false;
        }
        else if (context.performed)
        {
            StartCoroutine(CoyoteJumpTimer());
        }
    }

    private IEnumerator CoyoteJumpTimer()
    {
        coyoteJump = true;
        yield return new WaitForSeconds(jumpDelay);
        coyoteJump = false;
        
    }
}

    
