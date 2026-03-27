using UnityEngine;

public class geyser2Script : MonoBehaviour
{
    // Declare variables
    public float geyserJumpForce = 25f;
    public PlayerController player;

    public bool flyUp = false;
    private float ogGravity;

    public Glide glide;

    private void Start()
    {
        ogGravity = player.gravity;
        print(ogGravity);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            print("geyser!");
            flyUp = true;
            glide.inGyser = true;
        }
      
    }

    private void OnTriggerExit(Collider other)
    {
        flyUp = false;
        glide.inGyser = false;
    }



    private void LateUpdate()
    {
        if(flyUp)
            player.gravity = 8;
        else
            player.gravity = ogGravity;
    }
}
