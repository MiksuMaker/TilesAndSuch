using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector : MonoBehaviour
{
    #region Properties

    [Header("Freehand")]
    [SerializeField] public Vector3 dir;
    [SerializeField] public float dist = 1;

    [Header("GameObject set")]
    [SerializeField] public GameObject A;
    [SerializeField] public GameObject B;

    [HideInInspector]
    public Vector3 from;
    [HideInInspector]
    public Vector3 to;

    #endregion

    #region GIZMOS
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        // Check if GameObjects are null
        if (A == null && B == null)
        {
            // Draw according to Freehand distances and directions

            from = transform.position;

            to = from + (dir * dist);
        }




        // DRAW VECTOR
        Gizmos.DrawLine(from, to);

    }
    #endregion

    #region Calculate Vector

    #endregion
}
