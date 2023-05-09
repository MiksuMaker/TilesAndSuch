using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FOV : MonoBehaviour
{
    #region Properties
    [SerializeField] float viewRadius = 5f;
    [SerializeField] float viewAngle = 45f;
    [SerializeField] float dotTresholdValue = 0.5f;
    [SerializeField] GameObject target;
    #endregion

    #region GIZMOS
    private void OnDrawGizmos()
    {
        // RADIUS
        Handles.color = Color.magenta;
        Handles.DrawWireArc(transform.position, Vector3.up, Vector3.right, 360f, viewRadius);

        // SIDES
        Handles.color = Color.white;

        Vector3 viewAngleA = DirFromAngle(-viewAngle);
        Vector3 viewAngleB = DirFromAngle(viewAngle);

        Handles.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
        Handles.DrawLine(transform.position, transform.position + viewAngleB * viewRadius);

        Gizmos.color = Color.green;

        // SIGHT
        if (target != null)
        {
            // Check that target is within radius
            Vector3 v = (target.transform.position - transform.position);

            float distToTarget = Mathf.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
            Debug.Log("Distance: " + distToTarget);

            if (distToTarget <= viewRadius)
            {
                // Check if Vector3.Dot is lower than the threshold
                float dot = Vector3.Dot(transform.forward, v.normalized);
                Debug.Log(dot);

                if (dot >= dotTresholdValue) // How check would be done with DOT
                //if (Vector3.Angle(transform.forward, v) < viewAngle / 2) // How to do it with VEctor3.Angle
                {
                    // Target is seen
                    Gizmos.color = Color.red;
                }
            }
            // Draw a line to target
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
    }


    public Vector3 DirFromAngle(float angleInDegrees)
    {
        Vector3 rot = new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));

        // Rotate towards
        return rot;
    }

    #endregion
}
