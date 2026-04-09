using UnityEngine;
using UnityEngine.Events;

public class DoorOpenTrigger : MonoBehaviour
{

    [SerializeField] private UnityEvent doorOpenEvent;

    private bool canOpenDoor = false;


    public void SetCanOpenDoor()
    {
        canOpenDoor = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canOpenDoor && other.CompareTag("player"))
        {
            doorOpenEvent.Invoke();
            Destroy(gameObject);
        }
    }
}
