using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.8f;

    public new Transform camera;

    private CharacterController _controller;
    private Vector2 _moveInput;
    private Vector3 _velocity;


    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update() //this function runs once every frame
    {
        print(_controller.isGrounded);
        if (!_controller.isGrounded)
        {
            _velocity.y += gravity * Time.deltaTime; //calculates gravity
            _controller.Move(_velocity * Time.deltaTime); //applies gravity
        }

        Vector3 forward = camera.forward;
        Vector3 right = camera.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * _moveInput.y + right * _moveInput.x;
        _controller.Move(moveDirection * speed * Time.deltaTime);

        // Rotate player to face same direction as camera 
        transform.rotation = Quaternion.Euler(0f, camera.eulerAngles.y, 0f);
    }


    public void Move(InputAction.CallbackContext context) //detects input for movement
    {
        _moveInput = context.ReadValue<Vector2>();
       // print("Move Input: " + _moveInput);
    }

    public void Jump(InputAction.CallbackContext context) //detects input for jump
    {
        if (_controller.isGrounded && context.performed)
        {
           // print("Jump!");
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); //applies jump force
            //_controller.attachedRigidbody.AddForce(0, 2, 0, ForceMode.Impulse);
        }
        else
            print("not grounded");
    }
}

    
