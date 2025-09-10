using Unity.VisualScripting;
using UnityEngine;

public class BouncePlatform : MonoBehaviour
{
    public Transform Obj;
    public float speed = 1;

    private bool up;
    private float defaulty;
    
    void Start()
    {
        defaulty = Obj.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (up)
        {
            Obj.localPosition = new Vector3(Obj.localPosition.x, Mathf.Lerp(Obj.localPosition.y, 
                defaulty + 1.2f, Time.deltaTime * speed), Obj.localPosition.z);
            
        }
        else
        {
            Obj.localPosition = new Vector3(Obj.localPosition.x, Mathf.Lerp(Obj.localPosition.y, 
                defaulty, Time.deltaTime * speed), Obj.localPosition.z);
           
        }

        if (Obj.localPosition.y >= defaulty + .9f || Obj.localPosition.y <= defaulty+ 0.3f)
        {
            up = !up;
        }
        
        
        
    }
}
