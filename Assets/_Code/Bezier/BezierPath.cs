using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class BezierPath : MonoBehaviour
{
    #region Properties
    [SerializeField]
    Mesh2D road2D;

    [Space(20)]

    [SerializeField]
    List<BezierPoint> points = new List<BezierPoint>();

    [Range(0, 1)]
    [SerializeField] float travel = 0;
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

            // POSITION
            Vector3 tPos = GetBezierPosition(travel, points[i % points.Count], points[(i + 1) % points.Count]);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(tPos, 0.3f);

            // ROTATIOOOON
            Vector3 tDir = GetBezierDirection(travel, points[i % points.Count], points[(i + 1) % points.Count]);
            Quaternion rot = Quaternion.LookRotation(tDir);

            Gizmos.color = new Color(100, 100, 100);

            Gizmos.DrawSphere(tPos + (rot * Vector3.right), 0.2f);
            Gizmos.DrawSphere(tPos + (rot * Vector3.right * 2f), 0.2f);
            Gizmos.DrawSphere(tPos + (rot * Vector3.up), 0.2f);
            Gizmos.DrawSphere(tPos + (rot * Vector3.up * 2f), 0.2f);
            Gizmos.DrawSphere(tPos + (rot * Vector3.left), 0.2f);
            Gizmos.DrawSphere(tPos + (rot * Vector3.left * 2f), 0.2f);

            //Handles.PositionHandle(tPos, rot);

            // ROAD
            #region Old
            //for (int y = 0; y < road2D.vertices.Length; y++)
            //{
            //    Vector3 roadpoint = road2D.vertices[y].point;

            //    Gizmos.color = Color.yellow;
            //    //Gizmos.DrawSphere(tPos + rot * roadpoint, 0.25f);

            //    //Vector3 firstPoint = road2D.vertices[y].point + (Vector2)tPos;
            //    //Vector3 secondPoint = road2D.vertices[(y + 1) % road2D.vertices.Length].point + (Vector2)tPos;
            //    Vector3 firstPoint = road2D.vertices[y].point /*+ (Vector2)tPos*/;
            //    Vector3 secondPoint = road2D.vertices[(y + 1) % road2D.vertices.Length].point /*+ (Vector2)tPos*/;

            //    Helpers.DrawLine(firstPoint, secondPoint);
            //}
            #endregion

            for (int y = 0; y < road2D.vertices.Length; y++)
            {
                Vector3 roadpoint = road2D.vertices[y].point;

                Gizmos.color = Color.yellow;
                //Gizmos.DrawSphere(tPos + rot * roadpoint, 0.25f);

                Vector3 firstPoint = tPos + (rot * road2D.vertices[y].point);
                Vector3 secondPoint = tPos + (rot * road2D.vertices[(y + 1) % road2D.vertices.Length].point) /*+ (Vector2)tPos*/;

                Helpers.DrawLine(firstPoint, secondPoint);
            }
        }

    }

    Vector3 GetBezierPosition(float t, BezierPoint pt1, BezierPoint pt2)
    {
        // Lerp
        Vector3 PtX = (1 - t) * pt1.anchor + t * pt1.control1.position;
        Vector3 PtY = (1 - t) * pt1.control1.position + t * pt2.control0.position;
        Vector3 PtZ = (1 - t) * pt2.control0.position + t * pt2.anchor;

        // Lerp 2
        Vector3 PtXY = (1 - t) * PtX + t * PtY;
        Vector3 PtYZ = (1 - t) * PtY + t * PtZ;

        return (1 - t) * PtXY + t * PtYZ;
    }

    Vector3 GetBezierDirection(float t, BezierPoint pt1, BezierPoint pt2)
    {
        // Lerp
        Vector3 PtX = (1 - t) * pt1.anchor + t * pt1.control1.position;
        Vector3 PtY = (1 - t) * pt1.control1.position + t * pt2.control0.position;
        Vector3 PtZ = (1 - t) * pt2.control0.position + t * pt2.anchor;

        // Lerp 2
        Vector3 PtXY = (1 - t) * PtX + t * PtY;
        Vector3 PtYZ = (1 - t) * PtY + t * PtZ;

        return (PtXY - PtYZ).normalized;
    }

    #endregion
}

static class Helpers
{
    public static void DrawRay(Vector3 pos, Vector3 dir, Color c)
    {
        Gizmos.color = c;
        Gizmos.DrawRay(pos, dir);
    }

    public static void DrawLine(Vector3 pos, Vector3 pos2, Color c)
    {
        Gizmos.color = c;
        Gizmos.DrawLine(pos, pos2);
    }

    public static void DrawLine(Vector3 pos, Vector3 pos2)
    {
        Gizmos.DrawLine(pos, pos2);
    }

    public static void DebugDrawRay(Vector3 pos, Vector3 dir, Color c)
    {
        Debug.DrawRay(pos, dir, c, 3f);
    }


    static void DrawShape()
    {

    }

    
}
