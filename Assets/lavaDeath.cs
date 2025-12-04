using UnityEngine;
using UnityEngine.SceneManagement;

public class lavaDeath : MonoBehaviour
{    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}