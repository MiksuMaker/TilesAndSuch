using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossProductPlacer : MonoBehaviour
{
    #region Properties
    [SerializeField] GameObject turretPrefab;


    [Header("Hitting")]
    [SerializeField] float maxHitDistance = 30f;

    [HideInInspector]
    RaycastHit hit;


    [SerializeField] float vectorLength = 5f;
    #endregion

    #region Gizmos
    private void OnDrawGizmos()
    {
        // Draw a Raycast
        //RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, maxHitDistance);

        // Check if hit anything
        if (hit.collider != null)
        {
            // Draw a debug ray
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(transform.position, transform.forward * hit.distance);

            CalculatePlacement();
        }
        else
        {
            // Hit nothing
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * maxHitDistance);
        }
    }
    #endregion

    #region Cross Placement
    [ContextMenu("Place a Turret")]
    public void PlaceTurret()
    {
        #region Null Checks
        if (hit.collider == null) { Debug.LogWarning("No hit detected. Can't place a turret."); return; }
        if (turretPrefab == null) { Debug.LogWarning("No turret prefab assigned. Can't place a turret."); return; }
        #endregion

        // Place the Object
        CalculateAndPlace();
    }

    public void CalculateAndPlace()
    {
        // Raycast
        Physics.Raycast(transform.position, transform.forward, out hit, maxHitDistance);

        // Get normal
        Vector3 normal = hit.normal;
        DebugDrawRay(hit.point, normal.normalized * vectorLength, Color.green);

        // Calculate Transform.Right
        Vector3 right = Vector3.Cross(normal, transform.forward);
        DebugDrawRay(hit.point, right.normalized * vectorLength, Color.red);

        // Calculate Transform.Forward
        Vector3 forward = Vector3.Cross(right, normal);
        DebugDrawRay(hit.point, forward.normalized * vectorLength, Color.blue);

        // Instantiate Object
        GameObject turret = Instantiate(turretPrefab,
                                        hit.point,
                                        Quaternion.identity) as GameObject;

        // Rotate the Turret accordingly

        // V1   (These don't work, as each one "resets" the others, so they can't be "stacked")

        //turret.transform.right = right;   
        //turret.transform.up = normal;
        //turret.transform.forward = forward;

        // V2   ( This works, as it set both UP and FORWARD accordingly)
        Quaternion rot = Quaternion.LookRotation(forward.normalized, normal.normalized);
        turret.transform.rotation = rot;
    }

    public void CalculatePlacement()
    {
        // Raycast
        Physics.Raycast(transform.position, transform.forward, out hit, maxHitDistance);

        // Get normal
        Vector3 normal = hit.normal;
        DrawRay(hit.point, normal.normalized * vectorLength, Color.green);

        // Calculate Transform.Right
        Vector3 right = Vector3.Cross(normal, transform.forward);
        DrawRay(hit.point, right.normalized * vectorLength, Color.red);

        // Calculate Transform.Forward
        Vector3 forward = Vector3.Cross(right, normal);
        DrawRay(hit.point, forward.normalized * vectorLength, Color.blue);
    }

    #endregion

    #region Helpers
    private void DrawRay(Vector3 pos, Vector3 dir, Color c)
    {
        Gizmos.color = c;
        Gizmos.DrawRay(pos, dir);
    }

    private void DebugDrawRay(Vector3 pos, Vector3 dir, Color c)
    {
        Debug.DrawRay(pos, dir, c, 3f);
    }

    [ContextMenu("Destroy ALL turrets")]
    private void DestroyAllTurrets()
    {
        FinderScript[] turrets = FindObjectsOfType<FinderScript>();

        foreach (FinderScript t in turrets)
        {
            //Destroy(t.gameObject);
            DestroyImmediate(t.gameObject);
        }
    }
    #endregion
}
