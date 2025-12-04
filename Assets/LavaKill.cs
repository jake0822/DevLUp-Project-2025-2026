using UnityEngine;
using UnityEngine.SceneManagement;

public class LavaKill : MonoBehaviour
{
    Vector3 spawnPoint;
    public CharacterController characterController;

    private void Start()
    {
        spawnPoint = new Vector3(-0.28f, 0.92f, 0f);
    }

    private void OnTriggerEnter(Collider Object)
    {
        if (Object.CompareTag("Deadly")) characterController.Move(spawnPoint - transform.position);
    }

}
