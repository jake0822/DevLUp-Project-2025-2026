using UnityEngine;
using UnityEngine.InputSystem;


public class LightDash : MonoBehaviour
{
    public CharacterController cc;
    public Transform facingTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector3 facingVector = facingTransform.rotation * Vector3.forward;
            cc.Move(facingVector * 10);
        }
        else if (context.canceled)
        {
            Debug.Log("undash");
        }
    }
}
