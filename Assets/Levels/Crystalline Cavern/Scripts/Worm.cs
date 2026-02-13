using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Worm : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform playerTransform;

    [SerializeField] private int segmentMoveDelay = 10;  // Physics ticks

    [SerializeField] private Vector3 segmentRotationOffset;

    [SerializeField] private GameObject head;
    [SerializeField] private GameObject segmentModel;
    [SerializeField] private int segmentCount = 10;

    [SerializeField] private AnimationCurve segmentSizeCurve;

    [SerializeField] private GameObject wanderPointsParent;
    [SerializeField] private float wanderPointDistanceThreshold = 1.0f;

    enum State
    {
        Wander,
        ChasePlayer,
        ChaseCrystal
    }

    private List<GameObject> segments = new List<GameObject>();

    private Vector3 headPositionOffset;

    private Vector3 prevPosition;
    private Queue<(Vector3, Quaternion)> pastTransforms = new Queue<(Vector3, Quaternion)>();
    private int positionQueueSize = -1;

    private State state = State.Wander;
    private Vector3 targetPosition;
    private List<Vector3> wanderPoints = new List<Vector3>();

    void Start()
    {
        headPositionOffset = head.transform.localPosition;
        float segmentScaleFactor = transform.localScale.x;

        // Create the segments
        for (int i = 0; i < segmentCount; i++) {
            GameObject segmentObject = Instantiate(segmentModel);
            segmentObject.transform.localScale = Vector3.one * segmentSizeCurve.Evaluate((float)i / segmentCount) * segmentScaleFactor;
            segments.Add(segmentObject);
        }

        positionQueueSize = segmentMoveDelay * (segmentCount + 1);

        // Create wander points
        foreach (Transform child in wanderPointsParent.transform)
        {
            wanderPoints.Add(child.localPosition);
        }
        targetPosition = GetRandomWanderPosition();
    }

    void UpdateHead()
    {
        // Update head position
        head.transform.position = transform.position + headPositionOffset;

        // Update head rotation
        Vector3 lookDirection = transform.position - prevPosition;
        //lookDirection.y = 0f;
        if (lookDirection.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            targetRotation *= Quaternion.Euler(segmentRotationOffset);
            head.transform.rotation = targetRotation;
        }
    }

    void UpdateSegments()
    {
        for (int i = 0; i < segments.Count; i++)
        {
            var segment = segments[segments.Count - i - 1];
            int index = (i + 1) * segmentMoveDelay;
            if (index >= pastTransforms.Count)
            {
                continue;
            }

            var (pos, rot) = pastTransforms.ElementAt(index);
            segment.transform.position = pos;
            segment.transform.rotation = rot;
        }

        // Update past positions queue
        if (pastTransforms.Count == positionQueueSize)
        {
            pastTransforms.Dequeue();
        }
        pastTransforms.Enqueue((head.transform.position, head.transform.rotation));
        prevPosition = transform.position;
    }
    Vector3 GetRandomWanderPosition()
    {
        return wanderPoints[Random.Range(0, wanderPoints.Count)];
    }

    void WanderUpdate()
    {
        float distanceToTarget = Vector3.Distance(targetPosition, transform.position);
        if (distanceToTarget < wanderPointDistanceThreshold)
        {
            targetPosition = GetRandomWanderPosition();
        }
    }


    void ChasePlayerUpdate()
    {
        targetPosition = playerTransform.position;
    }


    void ChaseCrystalUpdate()
    {

    }

    void FixedUpdate()
    {
        switch (state)
        {
            case State.Wander:
                WanderUpdate();
                break;
            case State.ChasePlayer:
                ChasePlayerUpdate();
                break;
            case State.ChaseCrystal:
                ChaseCrystalUpdate();
                break;
        }

        agent.SetDestination(targetPosition);
        UpdateSegments();
    }

    void Update()
    {
        UpdateHead();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, targetPosition);
    }
}
