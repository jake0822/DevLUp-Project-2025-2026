using System.Collections.Generic;
using UnityEngine;

public class WanderPoint : MonoBehaviour
{
    public List<WanderPoint> neighbors = new List<WanderPoint>();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.5f);

        Gizmos.color = Color.cyan;
        foreach (var neighbor in neighbors) {
            Gizmos.DrawLine(transform.position, neighbor.transform.position);
        }
    }
}
