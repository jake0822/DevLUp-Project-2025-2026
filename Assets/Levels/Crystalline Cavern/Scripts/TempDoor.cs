using System;
using Unity.VisualScripting;
using UnityEngine;

public class TempDoor : MonoBehaviour
{
    [SerializeField] private Transform left;
    [SerializeField] private Transform right;
    [SerializeField] private Transform leftDestination;
    [SerializeField] private Transform rightDestination;

    [SerializeField] private float moveDuration = 5.0f;

    enum OpenState
    {
        Idle, Opening, Open
    }

    private OpenState state = OpenState.Idle;
    private float openTimer = 0.0f;

    private Vector3 leftBasePosition, rightBasePosition;

    void Start()
    {
        leftBasePosition = left.position;
        rightBasePosition = right.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == OpenState.Opening)
        {
            openTimer = Mathf.MoveTowards(openTimer, moveDuration, Time.deltaTime);
            if (openTimer == moveDuration)
            {
                state = OpenState.Open;
            }

            float t = openTimer / moveDuration;
            left.position = Vector3.Lerp(leftBasePosition, leftDestination.position, t);
            right.position = Vector3.Lerp(rightBasePosition, rightDestination.position, t);
        }
    }

    public void Open()
    {
        state = OpenState.Opening;
    }
}
