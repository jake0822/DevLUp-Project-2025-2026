using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Worm : MonoBehaviour {
    [Header("Game Objects")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private LightSprite lightSprite;
    [SerializeField] private Transform playerTransform;

    [SerializeField] private GameObject head;
    [SerializeField] private GameObject segmentPrefab;

    [Header("Segment Settings")]
    [SerializeField] private int segmentCount = 10;
    [Tooltip("The delay in physics ticks between each segment")]
    [SerializeField] private int segmentMoveDelay = 10;  // Physics ticks
    [SerializeField] private Vector3 segmentRotationOffset;
    [Tooltip("Determines how segment size changes as you get closer to the worm's tail")]
    [SerializeField] private AnimationCurve segmentSizeCurve;

    [Header("Behavior")]
    [SerializeField] private float arrivalDistanceThreshold = 1.0f;

    [Header("Wander State Config.")]
    [SerializeField] private GameObject wanderPointsParent;

    [Header("Chase State Config.")]
    [Tooltip("The amount of time the worm will wait to change state after losing its target")]
    [SerializeField] private float loseTargetWaitTime = 3.0f;
    [Tooltip("The amount of time the worm won't chase the player after exiting chase state")]
    [SerializeField] private float chaseCooldown = 5.0f;

    enum State {
        Wander,
        Chase,
        PermaChase
    }

    private List<GameObject> segments = new List<GameObject>();

    private Vector3 headPositionOffset;

    private Vector3 prevPosition;
    private Queue<(Vector3, Quaternion)> pastTransforms = new Queue<(Vector3, Quaternion)>();
    private int positionQueueSize = -1;
    private bool frozen = false;

    private State state = State.Wander;
    private Vector3 targetPosition;
    private float stateChangeTimer = 0.0f;

    private WanderPoint originWanderPoint = null;
    private WanderPoint targetWanderPoint;
    List<WanderPoint> allWanderPoints = new List<WanderPoint>();
    private float chaseCooldownTimer = 0.0f;

    public void StartChase()
    {
        state = State.PermaChase;
    }

    void Start() {
        headPositionOffset = head.transform.localPosition;
        float segmentScaleFactor = transform.localScale.x;

        // Create the segments
        for (int i = 0; i < segmentCount; i++) {
            GameObject segmentObject = Instantiate(segmentPrefab);
            segmentObject.transform.localScale = Vector3.one * segmentSizeCurve.Evaluate((float)i / segmentCount) * segmentScaleFactor;
            segments.Add(segmentObject);
        }

        positionQueueSize = segmentMoveDelay * (segmentCount + 1);

        // Choose a random starting wander point

        foreach (Transform child in wanderPointsParent.transform) {
            allWanderPoints.Add(child.GetComponent<WanderPoint>());
        }
        SetRandomWanderPoint();
    }

    void UpdateHead() {
        // Update head position
        head.transform.position = transform.position + headPositionOffset;

        // Update head rotation
        Vector3 lookDirection = transform.position - prevPosition;
        //lookDirection.y = 0f;
        if (lookDirection.sqrMagnitude > 0.0001f) {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            targetRotation *= Quaternion.Euler(segmentRotationOffset);
            head.transform.rotation = targetRotation;
        }
    }

    void UpdatePastTransforms() {
        // Update past positions queue
        if (pastTransforms.Count == positionQueueSize) {
            pastTransforms.Dequeue();
        }
        pastTransforms.Enqueue((head.transform.position, head.transform.rotation));
        prevPosition = transform.position;
    }

    void InterpolateSegments() {
        float interpolationFactor = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
        interpolationFactor = Mathf.Clamp01(interpolationFactor);
        
        for (int i = 0; i < segments.Count; i++) {
            var segment = segments[segments.Count - i - 1];
            int index = (i + 1) * segmentMoveDelay;
            
            if (index >= pastTransforms.Count) {
                continue;
            }
            
            var (currentPos, currentRot) = pastTransforms.ElementAt(index);
            int prevIndex = index - 1;
            Vector3 prevPos;
            Quaternion prevRot;
            
            if (prevIndex >= 0) {
                (prevPos, prevRot) = pastTransforms.ElementAt(prevIndex);
            } 
            else {
                prevPos = currentPos;
                prevRot = currentRot;
            }
            
            // Interpolate between previous and current target
            segment.transform.position = Vector3.Lerp(prevPos, currentPos, interpolationFactor);
            segment.transform.rotation = Quaternion.Slerp(prevRot, currentRot, interpolationFactor);
        }
    }

    void SetFrozen(bool isFrozen) {
        frozen = isFrozen;
        agent.enabled = !isFrozen;
    }

    void SetTargetPosition(Vector3 pos) {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(pos, out hit, 10f, NavMesh.AllAreas)) {
            targetPosition = hit.position;
        }
        else {
            targetPosition = pos;
        }
    }

    bool HasArrivedAtTarget() {
        return Vector3.Distance(targetPosition, transform.position) < arrivalDistanceThreshold;
    }

    void SetRandomWanderPoint() {
        targetWanderPoint = allWanderPoints[Random.Range(0, allWanderPoints.Count)];
        SetTargetPosition(targetWanderPoint.transform.position);
        originWanderPoint = null;
    }

    void NextWanderPoint() {
        var neighboringPoints = targetWanderPoint.neighbors;
        WanderPoint nextPoint;
        // Choose a random neighboring point that isn't the one that we came from
        do {
            nextPoint = neighboringPoints[Random.Range(0, neighboringPoints.Count)];
        } while (nextPoint == originWanderPoint);

        originWanderPoint = targetWanderPoint;
        targetWanderPoint = nextPoint;
    }

    void WanderUpdate() {
        if (HasArrivedAtTarget()) {
            NextWanderPoint();
            SetTargetPosition(targetWanderPoint.transform.position);
        }

        chaseCooldownTimer = Mathf.MoveTowards(chaseCooldownTimer, 0.0f, Time.fixedDeltaTime);
        if (chaseCooldownTimer == 0 && lightSprite.IsDetected(head.transform.position)) {
            ToChaseState();
        }
    }

    void ToWanderState() {
        Debug.Log("Changed to wander state");
        state = State.Wander;
        chaseCooldownTimer = chaseCooldown;
        SetFrozen(false);
        SetRandomWanderPoint();
    }

    void ChaseUpdate()
    {
        if (HasArrivedAtTarget())
        {
            SetFrozen(true);
            stateChangeTimer = Mathf.MoveTowards(stateChangeTimer, 0.0f, Time.fixedDeltaTime);
            if (stateChangeTimer == 0.0f)
            {
                ToWanderState();
            }
        }
        else if (lightSprite.IsDetected(head.transform.position) && !lightSprite.IsInCrystal())
        {
            SetTargetPosition(lightSprite.GetPosition());
            stateChangeTimer = loseTargetWaitTime;
            SetFrozen(false);
        }
    }

    void ToChaseState() {
        Debug.Log("Changed to chase sprite state");
        state = State.Chase;
    }

    void PermaChaseUpdate() {
        SetTargetPosition(playerTransform.position);
        stateChangeTimer = loseTargetWaitTime;
        SetFrozen(false);
    }

    void FixedUpdate() {
        switch (state) {
            case State.Wander:
                WanderUpdate();
                break;
            case State.Chase:
                ChaseUpdate();
                break;
            case State.PermaChase:
                PermaChaseUpdate();
                break;
                
        }


        if (!frozen) {
            agent.SetDestination(targetPosition);
            UpdatePastTransforms();
        }
    }

    void Update() {
        UpdateHead();

        if (!frozen) {
            InterpolateSegments();
        } 
    }

    private void OnDrawGizmos() {
        // Draw line to target position
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, targetPosition);

        // Draw sphere at target position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPosition, 0.5f);

        // Draw state indicator
        switch (state) {
            case State.Wander:
                Gizmos.color = Color.green;
                break;
            case State.Chase:
                Gizmos.color = Color.red;
                break;
        }
        Vector3 stateGizmoPosition = head.transform.position;
        stateGizmoPosition.y += 2.5f;
        Gizmos.DrawSphere(stateGizmoPosition, 0.25f);
    }
}
