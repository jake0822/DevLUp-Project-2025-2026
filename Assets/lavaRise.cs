using UnityEngine;

public class lavaRise : MonoBehaviour
{
    public bool start = false;
    public float velo;

    private Vector3 ogPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ogPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + (velo * Time.deltaTime), this.transform.position.z);
    }
    
    public void ResetLava()
    {
        transform.position = ogPos;
        start = false; // stop movement after reset
    }
    
}
