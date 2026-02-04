using UnityEngine;
using UnityEngine.InputSystem;

public class ShootWater : MonoBehaviour
{
    public ParticleSystem waterParticle;

    public void shootWater(InputAction.CallbackContext context) //detects input for crouch
    {

        if (context.performed) // button pressed
        {
            print("water!");
            waterParticle.Play();
        }
        else if (context.canceled) // button released
        {
            print("no more water :(");
            waterParticle.Stop();
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
