using UnityEngine;

public class IntroDoor : MonoBehaviour
{
    [SerializeField] private GameObject doorObject;
    [SerializeField] private Transform target;
    [SerializeField] private float moveDuration;

    private Vector3 basePosition;

    private float moveTimer = -1;

    public void OpenDoor()
    {
        moveTimer = 0;
    }

    private void Start()
    {
        basePosition = doorObject.transform.position;
    }

    void Update()
    {
        if (moveTimer != -1) {
            moveTimer = Mathf.MoveTowards(moveTimer, moveDuration, Time.deltaTime);
            float t = moveTimer / moveDuration;
            doorObject.transform.position = Vector3.Lerp(basePosition, target.position, t);
        }
    }
}
