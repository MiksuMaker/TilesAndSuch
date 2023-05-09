using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTrigger : Trigger
{
    [Header("Dot Product")]
    [SerializeField] protected float dotTreshold = 0.5f;
    [SerializeField] protected LayerMask obstacleMask;

    protected override void IsTargetInSight()
    {
        // Is the target in the looking direction?
        Vector3 dirTOTarget = target.pos - transform.position;

        // Get Dot Product
        float dot = Vector3.Dot(transform.forward, dirTOTarget.normalized);
        //Debug.Log("Dot: " + dot);

        float sqrDist = radius * radius;
        if ((Vector3.SqrMagnitude(dirTOTarget) < sqrDist) && dot >= dotTreshold)
        {
            // Check if nothing is blocking the view
            RaycastHit hit;
            if (!Physics.Raycast(transform.position,
                dirTOTarget.normalized,
                out hit,
                dirTOTarget.magnitude,
                obstacleMask))
            {
                //Debug.DrawRay(transform.position, dirTOTarget.normalized * Mathf.Min(dirTOTarget.magnitude, radius), Color.red, 1f);
                //Debug.Log("Target in sight");

                // Target in sight
                DoTargetInSightEffect();
                return;
            }
            //Debug.DrawRay(transform.position, dirTOTarget.normalized * Mathf.Min(dirTOTarget.magnitude, radius), Color.yellow, 1f);
            DoTargetInRadiusEffect();
            return;
        }

        //Debug.DrawRay(transform.position, dirTOTarget.normalized * Mathf.Min(dirTOTarget.magnitude, radius), Color.green, 1f);

        //Debug.Log("Not target");

        //// No target found
        DoTargetOutOfRangeEffect();

    }
}
