using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject doorCollider;
    [SerializeField] private AudioSource openSound;
    [SerializeField] private float disableColliderAfter = 1.0f;

    [SerializeField] private UnityEvent onOpenEvent;

    private float disableColliderTimer = -1;
    private bool isDoorOpen = false;

    public void FixedUpdate()
    {
        if (!isDoorOpen && disableColliderTimer != -1)
        {
            disableColliderTimer = Mathf.MoveTowards(disableColliderTimer, 0, Time.fixedDeltaTime);
            if (disableColliderTimer == 0)
            {
                doorCollider.SetActive(false);
            }

            if (disableColliderTimer == 0)
            {
                isDoorOpen = true;
                onOpenEvent.Invoke();
            }
        }
    }

    private IEnumerator PlayDoorSound()
    {
        yield return new WaitForSeconds(5);
        openSound.Play();
    }

    public void OpenDoor()
    {
        anim.SetTrigger("PlayAnim");
        disableColliderTimer = disableColliderAfter;
        StartCoroutine(PlayDoorSound());
    }
}
