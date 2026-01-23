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
    [SerializeField] private List<GameObject> segments = new List<GameObject>();

    private Vector3 prevPosition;
    private Queue<(Vector3, Quaternion)> pastTransforms = new Queue<(Vector3, Quaternion)>();
    private int positionQueueSize = -1;

    void Start()
    {
        positionQueueSize = segmentMoveDelay * (segments.Count + 1);
    }

    void Update()
    {   
        // Update head position
        head.transform.position = transform.position;
        
        // Update head rotation
        Vector3 lookDirection = prevPosition - transform.position;
        lookDirection.Normalize();
        lookDirection.y = 0f;
        head.transform.eulerAngles = segmentRotationOffset;
        head.transform.Rotate(lookDirection);
    }

    void FixedUpdate()
    {
        agent.SetDestination(playerTransform.position);

        for (int i = 0; i < segments.Count; i++)
        {
            var segment = segments[segments.Count-i-1];
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
        pastTransforms.Enqueue((transform.position, head.transform.rotation));
    }
}
