using UnityEngine;

public class geyser2Script : MonoBehaviour
{
    // Declare variables
    public float geyserJumpForce = 25f;

    private void OnTriggerEnter(Collider other)
    {
        print("geyser!");

    }
}
