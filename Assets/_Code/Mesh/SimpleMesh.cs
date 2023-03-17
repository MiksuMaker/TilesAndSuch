using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMesh : MonoBehaviour
{
    #region Properties
    [SerializeField] public int edgePoints = 10;
    [SerializeField] public float radius = 3f;

    [Header("Donut")]
    [SerializeField] public float innerRadius = 1f;
    #endregion

    #region Setup
    private void Start()
    {
        //SetupMesh();
        //DrawCircle(edgePoints, radius);
        DrawDonut(edgePoints, radius, innerRadius);
    }

    private void SetupMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] verts =
        {
            new Vector3(-1, 1, 0),
            new Vector3( 1, 1, 0),
            new Vector3(-1,-1, 0),
            new Vector3( 1,-1, 0)
        };

        // Manually set triangle indices
        int[] tri_indices =
        {
            2, 1, 0,
            2, 3, 1
        };

        mesh.vertices = verts;
        mesh.SetTriangles(tri_indices, 0);
        //mesh.RecalculateNormals();

        //GetComponent<MeshRenderer>().material = ;

        GetComponent<MeshFilter>().sharedMesh = mesh;

    }
    #endregion

    #region Functions
    private void DrawCircle(int amount, float radius)
    {
        if (amount < 3) { amount = 3; }

        Mesh mesh = new Mesh();
        
        Vector3[] verts = new Vector3[amount + 1];
        verts[0] = transform.position;

        for (int i = 0; i < amount; i++)
        {
            float angle = (2 * Mathf.PI / amount) * i;

            verts[i + 1] = verts[0] + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
        }


        // Crate indices
        int[] tri_indices = new int[amount*3];

        for (int i = 0; i < amount - 1; i++)
        {
            tri_indices[i * 3] = 0;
            tri_indices[i * 3 + 1] = i + 1;
            tri_indices[i * 3 + 2] = i + 2;
        }

        tri_indices[amount*3 - 3] = 0;
        tri_indices[amount*3 - 2] = amount;
        tri_indices[amount*3 - 1] = 1;

        // Create the Mesh
        //mesh.vertices = verts;              // Vertices
        mesh.SetVertices(verts);
        mesh.SetTriangles(tri_indices, 0);  // Triangles
        mesh.RecalculateNormals();          // Normals
        //mesh.SetUVs();                      // UVs?

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    private void DrawDonut(int amount, float radius, float innerRadius)
    {
        if (amount < 3) { amount = 3; }

        Mesh mesh = new Mesh();

        // Vertices
        List<Vector3> verts = new List<Vector3>();

        // UVs
        List<Vector2> uvs = new List<Vector2>();

        // Normals
        List<Vector3> normals = new List<Vector3>();

        for (int i = 0; i < amount; i++)
        {
            float angle = Mathf.PI * 2 * i / amount;    // Angle of this round of verts

            Vector3 v = new Vector3(Mathf.Cos(angle),
                                    Mathf.Sin(angle));

            // Add verts, inner first
            verts.Add(v * innerRadius);     // Inner point
            verts.Add(v * radius);          // Outer point

            #region NEW UVs
            // Midpoint? (as a vector) + v ???
            //uvs.Add(v * innerRadius);   // Inner
            //uvs.Add(v * radius);        // Outer

            Vector2 mid = new Vector2(0.5f, 0.5f);
            Vector2 s = v * 0.5f;
            uvs.Add(mid + s * (innerRadius / radius));  // Inner UV
            uvs.Add(mid + s);                           // Outer UV
            #endregion

            #region OLD UVs
            //uvs.Add(new Vector2(i/(float)amount, 0)); // Inner UV
            //uvs.Add(new Vector2(i / (float)amount, 1)); // Outer UV
            #endregion

            // Normals
            normals.Add(Vector3.forward);
            normals.Add(Vector3.forward);
        }

        List<int> tri_indices = new List<int>();
        for (int i = 0; i < amount - 1; i++)
        {                                                    // i = 0    i = 1
            int innerFirst = i * 2;                             // 0    // 2
            int outerFirst = innerFirst + 1;    // i * 2 + 1    // 1    // 3
            int innerSecond = outerFirst + 1;   // i * 2 + 2    // 3    // 4
            int outerSecond = innerSecond + 1;  // i * 2 + 3    // 4    // 5

            // Add First Triangle indices
            tri_indices.Add(innerFirst);
            tri_indices.Add(outerFirst);
            tri_indices.Add(innerSecond);

            // Add second Triangle indices
            tri_indices.Add(outerFirst);
            tri_indices.Add(outerSecond);
            tri_indices.Add(innerSecond);
        }

        // Add Last Triangles manually
        tri_indices.Add((amount * 2 - 1) - 1);
        tri_indices.Add((amount * 2 - 1));
        tri_indices.Add(0);

        tri_indices.Add((amount * 2 - 1));
        tri_indices.Add(1);
        tri_indices.Add(0);

        // Create the Mesh
        mesh.vertices = verts.ToArray();
        mesh.SetTriangles(tri_indices, 0);
        //mesh.RecalculateNormals();
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    private void OnValidate()
    {
        //DrawCircle(edgePoints, radius);
        DrawDonut(edgePoints, radius, innerRadius);
    }
    #endregion
}
