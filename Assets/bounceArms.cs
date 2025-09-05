using Unity.VisualScripting;
using UnityEngine;

public class bounceArms : MonoBehaviour
{
    public Transform Larm, Rarm;
    public float speed = 1;

    private bool up;
    private float defaulty;

    public Transform camera;
    void Start()
    {
        defaulty = Larm.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (up)
        {
            Larm.localPosition = new Vector3(Larm.localPosition.x, Mathf.Lerp(Larm.localPosition.y, 
                defaulty + .12f, Time.deltaTime * speed), Larm.localPosition.z);
            Rarm.localPosition = new Vector3(Rarm.localPosition.x, Mathf.Lerp(Rarm.localPosition.y, 
                defaulty + .12f, Time.deltaTime * speed), Rarm.localPosition.z);
        }
        else
        {
            Larm.localPosition = new Vector3(Larm.localPosition.x, Mathf.Lerp(Larm.localPosition.y, 
                defaulty, Time.deltaTime * speed), Larm.localPosition.z);
            Rarm.localPosition = new Vector3(Rarm.localPosition.x, Mathf.Lerp(Rarm.localPosition.y, 
                defaulty, Time.deltaTime * speed), Rarm.localPosition.z);
        }

        if (Larm.localPosition.y >= defaulty + .09f || Larm.localPosition.y <= defaulty+ 0.03f)
        {
            up = !up;
        }
        
        
        
    }
}
