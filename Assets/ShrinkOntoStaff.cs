using UnityEngine;

public class ShrinkOntoStaff : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Parent")]
    public Transform parent;

    [Header("Movement Settings")]
    public float duration = 2f;
    public float heightAmplitude = 0.5f;
    public float sidewaysAmplitude = 0.3f;
    public float frequency = 2f;

    [Header("Scaling")]
    public float shrinkFactor = 0.5f;

    private Vector3 startPos;
    private Vector3 startScale;
    private float timeElapsed;

    private bool isMoving = false;

    public bool start = false;
    public CrystalMovements cm;

    public void StartMove(Transform newTarget)
    {
        target = newTarget;
        startPos = transform.position;
        startScale = transform.localScale;
        timeElapsed = 0f;
        isMoving = true;
    }

    private void Update()
    {
        if (start)
        {
            cm.enabled = false;
            target.gameObject.GetComponent<MeshRenderer>().enabled = false;
            StartMove(target);
            start = false;
        }

        if (!isMoving || target == null) return;

        timeElapsed += Time.deltaTime;
        float t = timeElapsed / duration;

        if (t >= 1f)
        {
            // Snap to final state
            transform.position = target.position;
            transform.localScale = startScale * shrinkFactor;

            // 👇 Set parent AFTER movement finishes
            if (parent != null)
            {
                transform.SetParent(parent);
            }
            
            isMoving = false;
            return;
        }

        // Base linear movement
        Vector3 basePos = Vector3.Lerp(startPos, target.position, t);

        // Whimsical offset
        float wave = Mathf.Sin(t * Mathf.PI * frequency);
        float waveSide = Mathf.Cos(t * Mathf.PI * frequency * 0.5f);

        Vector3 offset = Vector3.up * wave * heightAmplitude +
                         Vector3.right * waveSide * sidewaysAmplitude;

        transform.position = basePos + offset;

        // Smooth shrinking
        transform.localScale = Vector3.Lerp(startScale, startScale * shrinkFactor, t);
    }
}
