using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float speed = 5f;
    [SerializeField] private float jumpDelay = 0.5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] public float gravity = -9.8f;
    [SerializeField] private float groundDeceleration, airDeceleration, groundAcceleration, airAcceleration;

    public new Transform camera;

    private CharacterController _controller;
    private CapsuleCollider _collider;
    private Vector2 _moveInput;
    public Vector3 _velocity;
    private Vector3 _horizontalVelocity = Vector3.zero;

    private float castHeight;
    private float radius;

    private bool gliding = false;
    public bool canGlide = true;

    [HideInInspector] public bool _grounded = false;
    public bool coyoteJump = false; //allows buffered jump

    private Vector3 _externalMomentum = Vector3.zero;


    public void SetExternalMomentum(Vector3 momentum) // this is called from slide script
    {
        _externalMomentum = momentum;
    }
    private void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
        _controller = GetComponent<CharacterController>();
        
        castHeight = (_collider.height / 2 * transform.localScale.y)-_collider.height*0.25f;
        radius = _collider.radius;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    private bool isGrounded() //This is raycast based grounded function. It creates four raycasts below the player to detect the ground
    {
        bool isGrounded = Physics.Raycast(transform.position - new Vector3(radius, castHeight),
            Vector3.down, 0.6f, LayerMask.GetMask("ground"));
        Debug.DrawRay(transform.position - new Vector3(radius, castHeight),
            Vector3.down *0.6f, Color.green );
        if (isGrounded)
            return isGrounded;
        
        isGrounded = Physics.Raycast(transform.position - new Vector3(-radius, castHeight),
            Vector3.down, 0.6f, LayerMask.GetMask("ground"));
        Debug.DrawRay(transform.position - new Vector3(-radius, castHeight),
            Vector3.down *0.6f, Color.green );
        if (isGrounded)
            return isGrounded;
        
        isGrounded = Physics.Raycast(transform.position - new Vector3(0, castHeight, radius),
            Vector3.down, 0.6f, LayerMask.GetMask("ground"));
        Debug.DrawRay(transform.position - new Vector3(0, castHeight, radius),
            Vector3.down *0.6f, Color.green );
        if (isGrounded)
            return isGrounded;
        
        isGrounded = Physics.Raycast(transform.position - new Vector3(0, castHeight, -radius),
            Vector3.down, 0.6f, LayerMask.GetMask("ground"));
        Debug.DrawRay(transform.position - new Vector3(0, castHeight, -radius),
            Vector3.down *0.6f, Color.green );
        
        return isGrounded;
    }

    private void Update() //this function runs once every frame
    {
        // print(_externalMomentum);
        _grounded = isGrounded(); //checks if the player is grounded
        if (_grounded && gliding && canGlide)
        {
            ToggleGlide();
        }
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
            Vector3 targetVelocity = _externalMomentum.magnitude > moveDirection.magnitude ? _externalMomentum * speed : moveDirection * speed;
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
            print("condition 1");
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); //applies jump force
            coyoteJump = false;
        }
        else if (!_grounded && context.performed && canGlide)
        {
            ToggleGlide();
        }
        else if (context.performed)
        {
            StartCoroutine(CoyoteJumpTimer());
        }
    }

    public void ToggleGlide() // toggles glide!
    {
        if (gliding)
        {
            gravity = -9.8f;
        }
        else
        {
            gravity = 0f;
            _velocity.y = -1;
        }
            gliding = !gliding;
    }
    private IEnumerator CoyoteJumpTimer()
    {
        coyoteJump = true;
        yield return new WaitForSeconds(jumpDelay);
        coyoteJump = false;
        
    }
}

    
