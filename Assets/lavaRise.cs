using UnityEngine;

public class lavaRise : MonoBehaviour
{

    public float velo = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(transform.position.x, transform.position.y + (velo * Time.deltaTime), transform.position.z);
    }
}
