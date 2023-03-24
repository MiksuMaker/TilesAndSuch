using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    #region Properties
    public GameObject A;
    public GameObject B;
    public GameObject C;
    public GameObject D;
    [Space(20)]
    [Range(0,1)]
    [SerializeField] float t = 0f;
    [Space(20)]
    [SerializeField] float indicatorRadius = 0.1f;

    public bool turnGizmosOn = false;
    #endregion

    #region Gizmos
    private void OnDrawGizmos()
    {
        if (A == null || B == null || C == null || D == null) { return; }

        if (!turnGizmosOn) { return; }

        #region Setup
        Vector3 a = A.transform.position;
        Vector3 b = B.transform.position;
        Vector3 c = C.transform.position;
        Vector3 d = D.transform.position;

        // Draw lines between the Points
        DrawLine(a, b, Color.cyan);
        DrawLine(b, c);
        DrawLine(c, d);
        #endregion

        // Lerp
        Vector3 PtX = (1 - t) * a + t * b;
        Vector3 PtY = (1 - t) * b + t * c;
        Vector3 PtZ = (1 - t) * c + t * d;

        float rad = indicatorRadius;
        Gizmos.DrawSphere(PtX, rad);
        Gizmos.DrawSphere(PtY, rad);
        Gizmos.DrawSphere(PtZ, rad);

        // Draw lines between the Pt- points
        DrawLine(PtX, PtY, Color.white);
        DrawLine(PtY, PtZ);

        // Lerp between the according points
        Vector3 r_point = (1 - t) * PtX + t * PtY;
        Vector3 s_point = (1 - t) * PtY + t * PtZ;

        DrawLine(r_point, s_point, Color.magenta);
        Gizmos.DrawSphere(r_point, rad);
        Gizmos.DrawSphere(s_point, rad);

        // Final Lerp for the BEZIER CURVE POINT
        Vector3 bezier_point = (1 - t) * r_point + t * s_point;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(bezier_point, rad * 2);
    }
    #endregion

    #region BEZIER CALCULATION

    #endregion

    #region Helpers
    private void DrawRay(Vector3 pos, Vector3 dir, Color c)
    {
        Gizmos.color = c;
        Gizmos.DrawRay(pos, dir);
    }

    private void DrawLine(Vector3 pos, Vector3 pos2, Color c)
    {
        Gizmos.color = c;
        Gizmos.DrawLine(pos, pos2);
    }

    private void DrawLine(Vector3 pos, Vector3 pos2)
    {
        Gizmos.DrawLine(pos, pos2);
    }

    private void DebugDrawRay(Vector3 pos, Vector3 dir, Color c)
    {
        Debug.DrawRay(pos, dir, c, 3f);
    }
    #endregion
}
