using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Script_WallContact : MonoBehaviour
{
    private List<GameObject> touchedWalls = new List<GameObject>();
    public float collisionTimer = 1f;
    private float timer = 0f;
    private bool isColliding1 = false, isColliding2 = false;
    CharacterController controller;
    public GameObject wall1, wall2;

    public timer_script timer_Script;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= collisionTimer)
        {
            touchedWalls.Clear();
            timer = 0f;
            isColliding1 = false;
            isColliding2 = false;
        }

        if (isColliding1)
        {
            Debug.Log("Should be moving");
            controller.Move(Vector3.right * 100 * Time.deltaTime);
        }
        if (isColliding2) 
        {
            controller.Move(Vector3.left);
        }
        if (touchedWalls.Count == 2)
        {
            Debug.Log("End Game");
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Walls"))
        {
            if (hit.gameObject == wall1) 
            {
                isColliding1 = true;
                Debug.Log("Touches Wall1");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                timer_Script.resetText();
            }
            if (hit.gameObject == wall2)
            {
                isColliding2 = true;
                Debug.Log("Touches Wall2");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                timer_Script.resetText();
            }

            if (!touchedWalls.Contains(hit.gameObject))
            {
                touchedWalls.Add(hit.gameObject);
            }
        }
    }  
}
