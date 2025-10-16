using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AirPowers : MonoBehaviour
{
    //Dash Vars
    [SerializeField] private float maxDistance = 5.0f;
    [SerializeField] private float dashDuration = 2.0f;
    [SerializeField] private int maxDashCount = 3;
    [SerializeField] private float dashFloatTime = 1.5f;
    [SerializeField] private float dashCoolDown = 2.2f;

    private int curDashCount = 0;
    private float dashProgress = 0.0f;
    private bool dashInProgress = false;
    private bool dashInCoolDown = false;
    private bool floating = false;

    //Time tracking
    private float lastDashTime = 0.0f;

    //other comps
    private PlayerInput playerInput;
    private CharacterController _controller;
    private PlayerController _playerController;
    public new Transform camera;

    //This can be changed to "OnEnabled()" once the level selector has been completed 
    void Start()
    {

        //Get components
        _controller = GetComponent<CharacterController>();
        _playerController = GetComponent<PlayerController>();

        //Swap to Volcano-Escape input map
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("No PlayerInput component found on this GameObject!");
        }

        playerInput.SwitchCurrentActionMap("Volcano Escape");

    }


    public void upDraft(InputAction.CallbackContext context)
    {
        if (dashInProgress || dashInCoolDown || !context.performed) { return; }
        dashInProgress = true;
        floating = true;
        print("Dash started");
    }

    void LateUpdate()
    {

        Vector3 step = Vector3.zero;

        //Disable grav
        if (dashInProgress)
        {
            Vector3 targetVector = camera.forward * maxDistance;

            float stepDistance = maxDistance / dashDuration * Time.deltaTime;
            float stepPercent = Math.Clamp(stepDistance / maxDistance, 0, 1);
            dashProgress += stepPercent;

            step = Vector3.Lerp(Vector3.zero, targetVector, stepPercent);

            if (dashProgress >= 1)
            {
                //End current dash
                dashInProgress = false;
                dashProgress = 0.0f;
                curDashCount++;

                //Time float
                lastDashTime = Time.time; //float

                //float grace periodx
                if (curDashCount >= maxDashCount)
                {
                    curDashCount = 0;
                    floating = false;

                    //Handle dash cooldown
                    dashInCoolDown = true;
                    StartCoroutine(resetDash());
                    print("All dashes used");
                }


            }
        }

        //Remove float
        // if (floating && Time.time - lastDashTime > dashFloatTime)
        // {
        //     floating = false;
        // }

        _playerController.gravity = floating ? 0 : -9.8f;
        _controller.Move(step);
    }

    private IEnumerator resetDash()
    {
        yield return new WaitForSeconds(dashCoolDown);
        dashInCoolDown = false;
        print("dashes reset");

    }

}

        // if (floating)
        // {
        //     // step += new Vector3(0, -_controller.velocity.y, 0) * Time.deltaTime; //Opposite Y Vector to float
        // }
