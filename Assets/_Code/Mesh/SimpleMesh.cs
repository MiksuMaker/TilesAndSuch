using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMesh : MonoBehaviour
{
    #region Properties
    [SerializeField] public int edgePoints = 10;
    [SerializeField] public float radius = 3f;
    #endregion

    #region Setup
    private void Start()
    {
        //SetupMesh();
        DrawCircle(edgePoints, radius);
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
        mesh.vertices = verts;
        mesh.SetTriangles(tri_indices, 0);
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    private void OnValidate()
    {
        DrawCircle(edgePoints, radius);
    }
    #endregion
}
