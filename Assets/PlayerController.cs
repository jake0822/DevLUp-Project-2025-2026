using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.8f;

    private CharacterController _controller;
    private Vector2 _moveInput;
    private Vector3 _velocity;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update() //this function runs once every frame
    {
        _velocity.y += gravity * Time.deltaTime; //calculates gravity
        _controller.Move(_velocity * Time.deltaTime);//applies gravity

        Vector3 move = new Vector3(_moveInput.x, 0, _moveInput.y);
        _controller.Move(move * speed * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context) //detects input for movement
    {
        _moveInput = context.ReadValue<Vector2>();
        print("Move Input: " + _moveInput);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (_controller.isGrounded && context.performed)
        {
            print("Jump!");
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        else 
            print("not grounded");
    }
}
