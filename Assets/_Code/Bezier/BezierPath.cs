using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BezierPath : MonoBehaviour
{
    #region Properties
    [SerializeField]
    List<BezierPoint> points = new List<BezierPoint>();
    #endregion


    #region
    #endregion

    #region Gizmos
    private void OnDrawGizmos()
    {
        CalculatePath();
    }
    #endregion

    #region Functions
    [ContextMenu("Get All Points")]
    private void GetAllPoints()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            // Add all children to the list
            points.Add(transform.GetChild(i).GetComponent<BezierPoint>());
        }
    }

    //[ContextMenu("Delete Most Points")]
    private void DeleteAllPoints()
    {
        for (int i = transform.childCount; i > 0; i--)
        {
            // If only child, save them
            if (i == 0) { break; }

            // Destroy rest of the Children
            GameObject child = transform.GetChild(i - 1).gameObject;
            DestroyImmediate(child);
        }

        // Clear list
        points.Clear();

        GetAllPoints();
    }

    private void CalculatePath()
    {

        // Check points amount
        bool drawLoop = true;
        int pointCount = points.Count;
        if (pointCount < 3) { drawLoop = false; }

        // Draw Bezier Path
        for (int i = 0; i < pointCount; i++)
        {
            Handles.DrawBezier(points[i].transform.position,
                               //points[i + 1].transform.position,
                               points[(i + 1) % pointCount].transform.position,
                               points[i].control1.position,
                               //points[i + 1].control0.position,
                               points[(i + 1) % pointCount].control0.position,
                               Color.white, default, 2f
                               );

            // Check if last of the line
        }
    }
    #endregion
}

static class Helpers
{
    static void DrawRay(Vector3 pos, Vector3 dir, Color c)
    {
        Gizmos.color = c;
        Gizmos.DrawRay(pos, dir);
    }

    static void DrawLine(Vector3 pos, Vector3 pos2, Color c)
    {
        Gizmos.color = c;
        Gizmos.DrawLine(pos, pos2);
    }

    static void DrawLine(Vector3 pos, Vector3 pos2)
    {
        Gizmos.DrawLine(pos, pos2);
    }

    static void DebugDrawRay(Vector3 pos, Vector3 dir, Color c)
    {
        Debug.DrawRay(pos, dir, c, 3f);
    }
}
