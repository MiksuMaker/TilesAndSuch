using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePlane : MonoBehaviour
{

    [Range(1.0f, 1000.0f)]
    public float size = 50.0f;

    [Range(2, 255)]
    public int segments = 10;

    private Mesh mesh;
    private MeshFilter meshFilter;

    [Header("General Constraints")]
    public float maxHeight = 10f;
    public float minHeight = -10f;
    public bool useGeneralConstraints = true;

    [Header("Perlin Noise")]
    public float noiseStrength = 10f;
    [Range(1f, 50f)]
    public float damper = 10f;

    public List<NoiseIteration> noiseIterations = new List<NoiseIteration>();
    public List<NoiseAmplifier> noiseAmplifiers = new List<NoiseAmplifier>();

    [System.Serializable]
    public class NoiseIteration
    {
        [Min(1)]
        public float divider;
        [Min(1)]
        public float amplitude;
        public bool useNoise = true;
    }

    [System.Serializable]
    public class NoiseAmplifier
    {
        [Min(1)]
        public float divider;
        [Min(0.1f)]
        public float amplitude;

        [Range(0f, 1f)]
        public float minClamp = 0f;
        [Range(0f, 1f)]
        public float maxClamp = 1f;

        [HeaderAttribute("Options")]
        public int targetIteration = 0;
        //public float strengthModifier
        public bool useAmplifier = true;
    }

    #region Setup
    private void OnEnable()
    {
        RecalculateMesh();
    }

    private void OnValidate()
    {
        RecalculateMesh();

        //TestPerlin();
    }
    #endregion

    private void RecalculateMesh()
    {
        if (mesh == null)
        {
            // Recalculate the Mesh
            mesh = new Mesh();
            if (meshFilter == null) { meshFilter = GetComponent<MeshFilter>(); }
            meshFilter.sharedMesh = mesh;
        }
        GenerateMesh();
    }

    private void GenerateMesh()
    {
        mesh.Clear();

        // Verts
        List<Vector3> verts = new List<Vector3>();
        // Triangle indices
        List<int> tris = new List<int>();

        // Change between segments
        float delta = size / (float)segments;

        // HEIGHT
        float height = 0f;


        // Make the vertices
        float xPos = 0f;
        float yPos = 0f;
        for (float x = 0f; x <= segments; x++)
        {
            xPos = (float)x * delta;

            for (float y = 0f; y <= segments; y++)
            {
                yPos = (float)y * delta;

                // Generate height
                //height = Mathf.PerlinNoise(x / damper, y / damper) * noiseStrength; // V1
                height = GeneratePerlinNoise(x, y);

                verts.Add(new Vector3(xPos, height, yPos));
            }
        }


        // Make the triangle indices
        for (int seg_x = 0; seg_x < segments; seg_x++)
        {
            for (int seg_y = 0; seg_y < segments; seg_y++)
            {

                // Create indices for every square
                int index = seg_x * (segments + 1) + seg_y;
                int index_low = index + 1;
                int index_next = index + (segments + 1);
                int index_next_low = index_next + 1;

                // Add them to the list
                tris.Add(index);
                tris.Add(index_low);
                tris.Add(index_next);

                tris.Add(index_next);
                tris.Add(index_low);
                tris.Add(index_next_low);
            }
        }

        // Actually create the mesh
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateNormals();
    }

    private float GeneratePerlinNoise(float x, float y)
    {
        //height = Mathf.PerlinNoise(x / damper, y / damper) * noiseStrength;

        if (noiseIterations.Count == 0)
        {
            return Mathf.PerlinNoise(x / damper, y / damper) * noiseStrength;
        }
        else
        {
            // Go through each Noise Iteration and sum them together
            float total = 0f;
            int currentIteration = 0;
            foreach (var i in noiseIterations)
            {
                if (i.useNoise)
                {
                    // Check for Amplifiers
                    float amplifier = 1f;
                    if (noiseAmplifiers.Count != 0)
                    {
                        foreach (var a in noiseAmplifiers)
                        {
                            // Check validity
                            if (a.targetIteration == currentIteration && a.useAmplifier)
                            {
                                amplifier = Mathf.PerlinNoise((x / a.divider), (y / a.divider));

                                // Clamp
                                amplifier = Mathf.Clamp(amplifier, a.minClamp, a.maxClamp);

                                // Amplify the Amplifier
                                amplifier *= a.amplitude;
                                //amplifier = Mathf.Clamp(amplifier, a.minClamp, a.maxClamp);
                                
                            }
                        }
                    }

                    total += (Mathf.PerlinNoise((x / i.divider), (y / i.divider)) * (i.amplitude * amplifier));
                }
                currentIteration++;
            }

            // Clamp if necessary
            if (useGeneralConstraints) { total = Mathf.Clamp(total, minHeight, maxHeight); }

            // Return total
            return total;
        }
    }

    private void TestPerlin()
    {
        for (float x = 0; x < 10; x += UnityEngine.Random.Range(0f, 1f))
        {
            for (float y = 0; y < 10; y += UnityEngine.Random.Range(0f, 1f))
            {
                Debug.Log("X: " + x + " || Y: " + y);
                Debug.Log("Result: " + Mathf.PerlinNoise(x, y));
            }
        }
    }
}
