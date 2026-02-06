using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class lavaDeath : MonoBehaviour
{
    public AudioSource audio;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            audio.Play();
            print("restart");
        }
    }

   
}