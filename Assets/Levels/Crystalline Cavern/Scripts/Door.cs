using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject doorCollider;
    [SerializeField] private AudioSource openSound;
    [SerializeField] private float disableColliderAfter = 1.0f;

    private float disableColliderTimer = -1;

    public void FixedUpdate()
    {

        if (disableColliderTimer != -1)
        {
            disableColliderTimer = Mathf.MoveTowards(disableColliderTimer, 0, Time.fixedDeltaTime);
            if (disableColliderTimer == 0)
            {
                doorCollider.SetActive(false);
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
