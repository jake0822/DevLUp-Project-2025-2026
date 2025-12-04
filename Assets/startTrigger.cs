using UnityEngine;

public class startTrigger : MonoBehaviour
{
    public lavaRise lava;

    private void OnTriggerEnter(Collider other)
    {
        lava.start = true;
        print("Triggered by: " + other.name);
    }
}
