using UnityEngine;
using UnityEngine.SceneManagement;

public class lavaDeath : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            print("restart");
        }
    }
}