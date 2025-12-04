using UnityEngine;

public class GeyserScript : MonoBehaviour
{
    public PlayerController player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnTriggerEnter(Collider other)
    {
        print("Collision");
        player.transform.position.Set(10, 100, 10);
    }
    private void OnTriggerExit(Collider other)
    {
        print("exit");
    }
}
