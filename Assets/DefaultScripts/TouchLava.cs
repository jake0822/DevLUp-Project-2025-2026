using UnityEngine;

public class TouchLava : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject targetObject = GameObject.FindWithTag("Lava");

    }
    private void OnTriggerEnter(Collider Player)
    {
        Debug.Log("Triggered with: " + Player.name);

        if (Player.CompareTag("Target"))
        {
            // Do something when this collider enters another collider tagged "Target"
            Debug.Log("Box Colliders have collided!");

            // Move palyer back to checkpoint


            // Reset Lava to y coordinate


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
