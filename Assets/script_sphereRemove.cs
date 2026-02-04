using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class script_sphereRemove : MonoBehaviour
{
    private float timer = 0;
    public float eraseTimer = 4f;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > eraseTimer)
        {
            Destroy(gameObject);
            timer = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
