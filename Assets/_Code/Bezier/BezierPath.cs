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

    [SerializeField]
    bool gizmosOn = false;

    //[SerializeField]
    //bool fullCircle = true;
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

    #region Old Calculate Path
    private void OLDCalculatePath()
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

    private void CalculatePath()
    {

        // Check points amount
        int pointCount = points.Count;

        // Ready the List of Points
        List<Vector3> previousRoadPos = new List<Vector3>();
        List<Quaternion> previousRoadRots = new List<Quaternion>();


        // Prime Mesh
        Mesh mesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<int> tri_indices = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

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

                    
                    if (gizmosOn) { Helpers.DrawLine(firstPoint, secondPoint); }
                }
            }

        }

        for (int k = 0; k < previousRoadPos.Count; k++)
        {

            // LINES BETWEEN THE CROSS SECTIONS
            for (int y = 0; y < road2D.vertices.Length; y++)
            //for (int y = 0; y < road2D.vertices.Length - 1; y += 2)
            {
                int b = previousRoadPos.Count;

                Vector3 p1 = previousRoadPos[k] + (previousRoadRots[k] * road2D.vertices[y].point);
                Vector3 p2 = previousRoadPos[(k + 1) % b]
                                + (previousRoadRots[(k + 1) % b]
                                * road2D.vertices[y].point);

                if (gizmosOn) { Helpers.DrawLine(p1, p2); }

                // Add Verts
                verts.Add(p1);
            }
        }

        int last = previousRoadPos.Count * (road2D.vertices.Length);
        for (int u = 0; u < previousRoadPos.Count - 1; u++)
        {
            int r = u * (road2D.vertices.Length);

            #region Squares
            // Square 1
            tri_indices.Add(r + 0);
            tri_indices.Add(r + 2);
            tri_indices.Add(r + 16);

            tri_indices.Add(r + 2);
            tri_indices.Add(r + 18);
            tri_indices.Add(r + 16);


            // 2
            tri_indices.Add(r + 2);
            tri_indices.Add(r + 4);
            tri_indices.Add(r + 18);

            tri_indices.Add(r + 4);
            tri_indices.Add(r + 20);
            tri_indices.Add(r + 18);

            // 3
            tri_indices.Add(r + 4);
            tri_indices.Add(r + 6);
            tri_indices.Add(r + 20);

            tri_indices.Add(r + 6);
            tri_indices.Add(r + 22);
            tri_indices.Add(r + 20);

            // 4 --------------------
            tri_indices.Add(r + 6);
            tri_indices.Add(r + 8);
            tri_indices.Add(r + 22);

            tri_indices.Add(r + 8);
            tri_indices.Add(r + 24);    // ODD
            tri_indices.Add(r + 22);

            // 5 --------------------
            tri_indices.Add(r + 8);
            tri_indices.Add(r + 10);
            tri_indices.Add(r + 24);    // ODD

            tri_indices.Add(r + 10);
            tri_indices.Add(r + 26);
            tri_indices.Add(r + 24);    // ODD

            // 6 --------------------
            tri_indices.Add(r + 10);
            tri_indices.Add(r + 12);
            tri_indices.Add(r + 26);    // ODD

            tri_indices.Add(r + 12);
            tri_indices.Add(r + 28);
            tri_indices.Add(r + 26);    // ODD

            // 7 --------------------
            tri_indices.Add(r + 12);
            tri_indices.Add(r + 14);
            tri_indices.Add(r + 28);

            tri_indices.Add(r + 14);
            tri_indices.Add(r + 30);    // ODD
            tri_indices.Add(r + 28);

            // 8 --------------------
            tri_indices.Add(r + 14);
            tri_indices.Add(r + 0);
            tri_indices.Add(r + 16);

            tri_indices.Add(r + 14);
            tri_indices.Add(r + 16);
            tri_indices.Add(r + 30);    // ODD
            #endregion
        }

        // Add last ones manually

        #region Last Square
        // Square 1
        tri_indices.Add(last - 16);
        tri_indices.Add(last - 14);
        tri_indices.Add(0);

        tri_indices.Add(last - 14);
        tri_indices.Add(2);
        tri_indices.Add(0);


        //// 2
        tri_indices.Add(last - 14);
        tri_indices.Add(last - 12);
        tri_indices.Add(2);

        tri_indices.Add(last - 12);
        tri_indices.Add(4);
        tri_indices.Add(2);

        // 3
        tri_indices.Add(last - 12);
        tri_indices.Add(last - 10);
        tri_indices.Add(4);

        tri_indices.Add(last - 10);
        tri_indices.Add(6);
        tri_indices.Add(4);

        // 4 --------------------
        tri_indices.Add(last - 10);
        tri_indices.Add(last - 8);
        tri_indices.Add(6);

        tri_indices.Add(last - 8);
        tri_indices.Add(8);    // ODD
        tri_indices.Add(6);

        // 5 --------------------
        tri_indices.Add(last - 8);
        tri_indices.Add(last - 6);
        tri_indices.Add(8);    // ODD

        tri_indices.Add(last - 6);
        tri_indices.Add(10);
        tri_indices.Add(8);    // ODD

        // 6 --------------------
        tri_indices.Add(last - 6);
        tri_indices.Add(last - 4);
        tri_indices.Add(10);    // ODD

        tri_indices.Add(last - 4);
        tri_indices.Add(12);
        tri_indices.Add(10);    // ODD

        //7--------------------
        tri_indices.Add(last - 4);
        tri_indices.Add(last - 2);
        tri_indices.Add(12);

        tri_indices.Add(last - 2);
        tri_indices.Add(14);    // ODD
        tri_indices.Add(12);

        // 8 --------------------
        tri_indices.Add(last - 2);
        tri_indices.Add(last - 16);
        tri_indices.Add(14);

        tri_indices.Add(last - 16);
        tri_indices.Add(0);
        tri_indices.Add(14);    // ODD
        #endregion

        #region UVs
        // Add UVs
        for (int i = 0; i < previousRoadPos.Count; i++)
        {
            // Add posiion on the UV map

            int o = i % 2 == 0 ? 0 : 1;

            uvs.Add(new Vector2(0.2f, o));  // Begin
            uvs.Add(new Vector2(0.2f, o));  // Begin

            uvs.Add(new Vector2(0.1f, o));
            uvs.Add(new Vector2(0.1f, o));

            uvs.Add(new Vector2(0f, o));    // Left corner top
            uvs.Add(new Vector2(0f, o));    // Left corner top

            uvs.Add(new Vector2(0f, o));
            uvs.Add(new Vector2(0f, o));

            uvs.Add(new Vector2(0f, o));    // Right corner bottom
            uvs.Add(new Vector2(0f, o));    // Right corner bottom

            uvs.Add(new Vector2(1f, o));    // Right corner top
            uvs.Add(new Vector2(1f, o));    // Right corner top

            uvs.Add(new Vector2(0.9f, o));
            uvs.Add(new Vector2(0.9f, o));

            uvs.Add(new Vector2(0.8f, o)); // End?
            uvs.Add(new Vector2(0.8f, o)); // End?

            #region Old
            //uvs.Add(new Vector2(0.2f, 0));  // Begin
            //uvs.Add(new Vector2(0.1f, 0));
            //uvs.Add(new Vector2(0f, 0));    // Left corner top
            //uvs.Add(new Vector2(0f, 0));
            //uvs.Add(new Vector2(0f, 0));    // Right corner bottom
            //uvs.Add(new Vector2(1f, 0));    // Right corner top
            //uvs.Add(new Vector2(0.9f, 0));
            //uvs.Add(new Vector2(0.8f, 0)); // End?


            //uvs.Add(new Vector2(0.2f, 1));  // Begin
            //uvs.Add(new Vector2(0.1f, 1));
            //uvs.Add(new Vector2(0f, 1));    // Left corner top
            //uvs.Add(new Vector2(0f, 1));
            //uvs.Add(new Vector2(0f, 1));    // Right corner bottom
            //uvs.Add(new Vector2(1f, 1));    // Right corner top
            //uvs.Add(new Vector2(0.9f, 1));
            //uvs.Add(new Vector2(0.8f, 1)); // End?

            //uvs.Add(new Vector2(0.2f, 0));  // Begin
            //uvs.Add(new Vector2(0.2f, 1));  // Begin

            //uvs.Add(new Vector2(0.1f, 0));
            //uvs.Add(new Vector2(0.1f, 1));

            //uvs.Add(new Vector2(0f, 0));    // Left corner top
            //uvs.Add(new Vector2(0f, 1));    // Left corner top

            //uvs.Add(new Vector2(0f, 0));
            //uvs.Add(new Vector2(0f, 1));

            //uvs.Add(new Vector2(0f, 0));    // Right corner bottom
            //uvs.Add(new Vector2(0f, 1));    // Right corner bottom

            //uvs.Add(new Vector2(1f, 0));    // Right corner top
            //uvs.Add(new Vector2(1f, 1));    // Right corner top

            //uvs.Add(new Vector2(0.9f, 0));
            //uvs.Add(new Vector2(0.9f, 1));

            //uvs.Add(new Vector2(0.8f, 0)); // End?
            //uvs.Add(new Vector2(0.8f, 1)); // End?
            #endregion

        }
        #endregion


        Debug.Log("Verts: " + verts.Count);
        Debug.Log("UVs  : " + uvs.Count);

        mesh.SetVertices(verts);
        mesh.SetTriangles(tri_indices, 0);
        mesh.RecalculateNormals();
        mesh.SetUVs(0, uvs);

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
}
#endregion

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
