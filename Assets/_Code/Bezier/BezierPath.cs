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

    [SerializeField]
    //[Min(1)]
    [Range(1, 10)]
    int pointsBetween = 2;

    //[Range(0, 1)]
    //[SerializeField] 
    //float travel = 0;
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

        // Ready the List of Points
        List<Vector3> previousRoadPos = new List<Vector3>();
        List<Quaternion> previousRoadRots = new List<Quaternion>();


        // Draw Bezier Path
        for (int i = 0; i < pointCount; i++)
        {
            Handles.DrawBezier(points[i].transform.position,
                               points[(i + 1) % pointCount].transform.position,
                               points[i].control1.position,
                               points[(i + 1) % pointCount].control0.position,
                               Color.white, default, 2f
                               );


            // Modify travel
            float travel = 0f;


            // GET POINTS ON THE ROAD
            for (int u = 0; u < pointsBetween; u++)
            {
                travel += (1 / (float)pointsBetween);

                //travel = Random.Range(0f, 1f);

                // POSITION
                Vector3 tPos = GetBezierPosition(travel, points[i % points.Count], points[(i + 1) % points.Count]);

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(tPos, 0.3f);

                // ROTATIOOOON
                Vector3 tDir = GetBezierDirection(travel, points[i % points.Count], points[(i + 1) % points.Count]);
                Quaternion rot = Quaternion.LookRotation(tDir);

                Gizmos.color = new Color(100, 100, 100);



                previousRoadPos.Add(tPos);
                previousRoadRots.Add(rot);

                // ROAD CROSS SECTION
                for (int y = 0; y < road2D.vertices.Length; y++)
                {
                    Vector3 roadpoint = road2D.vertices[y].point;

                    Gizmos.color = Color.yellow;

                    Vector3 firstPoint = tPos + (rot * road2D.vertices[y].point);
                    Vector3 secondPoint = tPos + (rot * road2D.vertices[(y + 1) % road2D.vertices.Length].point);

                    Helpers.DrawLine(firstPoint, secondPoint);
                }
            }

        }

        for (int k = 0; k < previousRoadPos.Count; k++)
        {

            // LINES BETWEEN THE CROSS SECTIONS
            for (int y = 0; y < road2D.vertices.Length - 1; y++)
            {
                int b = previousRoadPos.Count;

                Vector3 p1 = previousRoadPos[k] + (previousRoadRots[k] * road2D.vertices[y].point);
                Vector3 p2 = previousRoadPos[(k + 1) % b]
                                + (previousRoadRots[(k + 1) % b]
                                * road2D.vertices[y].point);

                Helpers.DrawLine(p1, p2);
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
