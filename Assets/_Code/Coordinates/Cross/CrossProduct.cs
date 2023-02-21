using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossProduct : MonoBehaviour
{
    #region Prooerties
    [SerializeField] GameObject A;
    [SerializeField] GameObject B;

    [Space(30)]

    [SerializeField] float crossLength = 0f;
    [Space(30)]
    [SerializeField] bool drawCrossSphere = false;

    #endregion

    #region Gizmos
    private void OnDrawGizmos()
    {
        // Coordinate Cross
        DrawVector(Vector3.zero, Vector3.up, Color.green);
        DrawVector(Vector3.zero, Vector3.right, Color.red);


        // Draw Vectors to GameObjects
        if (A != null && B != null)
        {
            Vector3 a = (A.transform.position - transform.position).normalized;
            Vector3 b = (B.transform.position - transform.position).normalized;

            DrawVector(A.transform.position, transform.position, Color.white);
            DrawVector(B.transform.position, transform.position, Color.white);

            Vector3 cross = Vector3.Cross(a, b);
            crossLength = cross.magnitude;
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, cross);

            if (drawCrossSphere)
            {
                Gizmos.color = cross.z > 0 ? Color.magenta : Color.cyan;
                Gizmos.DrawWireSphere(transform.position, crossLength);
            }
        }
    }

    private void DrawVector(Vector3 from, Vector3 to, Color c)
    {
        Gizmos.color = c;
        Gizmos.DrawLine(from, to);
    }
    #endregion
}
