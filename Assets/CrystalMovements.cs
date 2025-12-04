using UnityEngine;

public class CrystalMovements : MonoBehaviour
{
    private Vector3 startPosition;

    public float bounceSpeed = 2f;    // Speed of bounce
    public float rotationSpeed = 50f; // Speed of rotation
    public float bounceDistance = 2f; // How high and low it bounces

    private void Start()
    {
        // Store the initial position of the object
        startPosition = transform.position;
    }

    private void Update()
    {
        // Rotate the object around its local Y axis
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        // Calculate the bounce position using sine wave (for smooth up and down motion)
        float newY = startPosition.y + Mathf.Sin(Time.time * bounceSpeed) * bounceDistance;

        // Apply the new Y position while keeping the original X and Z positions
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

    }
}
