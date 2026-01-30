using UnityEngine;

public class geiserScript3 : MonoBehaviour
{
    // Declare variables
    public float geyserJumpForce = 25f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>() != null)
        {
            print("big jump");
            collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * geyserJumpForce, ForceMode.Impulse);
        }
    }

}