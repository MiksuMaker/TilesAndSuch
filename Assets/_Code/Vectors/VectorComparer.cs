using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorComparer : MonoBehaviour
{
    #region Properties

    [Header("Vectors to Compare:")]
    [SerializeField] Vector v1;
    [SerializeField] Vector v2;

    [Header("Calculations")]
    [SerializeField] float dotValue = 0;

    #endregion

    #region GIZMOS
    private void OnDrawGizmos()
    {
        if (v1 != null && v2 != null)
        {
            dotValue = CalculateDot(v1, v2);
        }

        // DRAW SCALAR DOT VISUALIZER

        Vector3 from = v1.transform.position;
        Vector3 to = v1.to;

        Gizmos.color = Color.magenta;

        Vector3 toPos = v1.to * dotValue;
        Gizmos.DrawLine(v1.transform.position, toPos);


        Vector3 dir = (v1.transform.position - v1.to).normalized;

        if (dotValue <= 0) { dir = -dir; }

        Gizmos.DrawRay(toPos, Quaternion.AngleAxis(45f, Vector3.forward) * dir * 1f);
        Gizmos.DrawRay(toPos, Quaternion.AngleAxis(-45f, Vector3.forward) * dir * 1f);
    }
    #endregion

    #region CALCULATIONS
    private float CalculateDot(Vector v1, Vector v2)
    {
        Vector3 first = v1.dir.normalized;
        Vector3 second = v2.dir.normalized;

        return Vector3.Dot(first, second);
    }
    #endregion
}
