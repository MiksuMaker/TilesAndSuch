using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Mesh2D : ScriptableObject
{
    #region Properties

    #endregion

    [System.Serializable]

    public class Vertex
    {
        public Vector2 point;
        public Vector2 normal;

        // UV?
    }

    public Vertex[] vertices;
    public int[] lineIndices;
}
