using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WedgeTrigger : Trigger
{
    #region Properties
    [Header("FOV")]
    [Range(1f, 180f)]
    [SerializeField] float viewAngle = 90f;
    #endregion

    #region Functions
    protected override void IsTargetInSight()
    {
        // Test if Target is within radius
        Vector3 dirToTarget = target.pos - ownPos;
        float sqrDist = Vector3.SqrMagnitude(dirToTarget);
        float sqrRad = radius * radius;
        if (sqrDist < sqrRad)
        {
            // Target within radius
            float dotProduct = Vector3.Dot(transform.forward, dirToTarget.normalized);
            float angleToTarget = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;

            //Debug.Log("Angle to target: " + angleToTarget);

            if (angleToTarget < viewAngle)
            {
                // Target within View Angle too!
                DoTargetInSightEffect();
                return;
            }

            DoTargetInRadiusEffect();
            return;
        }

        // Target out of range
        DoTargetOutOfRangeEffect();
    }

    protected virtual void DrawRange()
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // RADIUS
        Handles.color = Color.yellow;
        Handles.DrawWireArc(transform.position, Vector3.up, Vector3.right, 360f, radius);

        // SIDES
        Handles.color = Color.white;

        Vector3 viewAngleA = DirFromAngle(-viewAngle);
        Vector3 viewAngleB = DirFromAngle(viewAngle);

        Handles.DrawLine(transform.position, transform.position + viewAngleA * radius);
        Handles.DrawLine(transform.position, transform.position + viewAngleB * radius);

    }

    private void OnDrawGizmos()
    {
        DrawRange();
    }

    public Vector3 DirFromAngle(float angleInDegrees)
    {
        //Vector3 rot = new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        angleInDegrees += transform.eulerAngles.y;

        Vector3 rot = new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));

        return rot;
    }
    #endregion
}
