using UnityEngine;

public class lightFollow_script : MonoBehaviour
{
    public CharacterController characterController;
    Vector3 offset = new Vector3(0, 5f, 0);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = characterController.transform.position + offset;
    }
}
