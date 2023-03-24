using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BezierPoint : MonoBehaviour
{
    #region Properties
    public Transform control0;
    public Transform control1;

    [HideInInspector]
    public Vector3 anchor;
    #endregion

    #region Setup

    #endregion

    #region GIzmos
    private void OnDrawGizmos()
    {
        anchor = transform.position;
        Vector3 c0 = control0.position;
        Vector3 c1 = control1.position;

        // Draw Lines for ControlPoints
        DrawLine(c0, anchor, Color.green);
        DrawLine(c1, anchor, Color.red);

        Handles.DrawBezier(c0, c1, anchor, anchor, Color.blue, Texture2D.whiteTexture, 1f);
    }
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
