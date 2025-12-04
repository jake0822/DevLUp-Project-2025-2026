using UnityEngine;

public class lavaRise : MonoBehaviour
{
    public bool start = false;
    public float velo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (start)
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + (velo * Time.deltaTime), this.transform.position.z);
    }
    
}
