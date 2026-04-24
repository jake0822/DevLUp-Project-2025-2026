using UnityEngine;

public class WallsCloseInScript : MonoBehaviour
{
    public GameObject wall_1;
    public GameObject wall_2;
    public float wallCloseSpeed = 0.2f;
    public GameObject player;
    void Start()
    {
        
    }

    void Update()
    {
        //get position
        Vector3 wallPosition1 = wall_1.transform.position;
        Vector3 wallPosition2 = wall_2.transform.position;

        //update position values
        wallPosition1.x -= wallCloseSpeed * Time.deltaTime;
        wallPosition2.x += wallCloseSpeed * Time.deltaTime;

        //update
        wall_1.transform.position = wallPosition1;
        wall_2.transform.position = wallPosition2;
    }
}
