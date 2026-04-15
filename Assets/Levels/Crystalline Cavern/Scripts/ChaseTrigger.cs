using UnityEngine;
using UnityEngine.AI;

public class ChaseTrigger : MonoBehaviour
{
    [SerializeField] private Worm worm;
    [SerializeField] private NavMeshAgent wormAgent;
    [SerializeField] private Transform wormTeleportPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            Debug.Log("Chase started");
            gameObject.SetActive(false);

            wormAgent.enabled = false;
            worm.transform.position = wormTeleportPoint.position;
            wormAgent.enabled = true;
            worm.StartChase();
        }
    }
}
