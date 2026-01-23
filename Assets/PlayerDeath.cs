using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class lavaDeath : MonoBehaviour
{
    public AudioSource audio;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
           // StartCoroutine(playDeathSound());
            audio.Play();
            print("restart");
        }
    }

    private IEnumerator playDeathSound()
    {
        print("started");
        yield return new WaitForSeconds(0.1f);
        audio.Play();
        print("death audio");
    }
}