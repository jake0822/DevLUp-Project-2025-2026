using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject doorCollider;
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

    public void OpenDoor()
    {
        anim.SetTrigger("PlayAnim");
        disableColliderTimer = disableColliderAfter;
    }
}
