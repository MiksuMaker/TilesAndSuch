using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalToWorld : MonoBehaviour
{
    #region Properties
    public float localX = 0f;
    public float localY = 0f;
    #endregion

    #region Gizmos
    private void OnDrawGizmos()
    {
        DrawVector(Vector3.zero, Vector3.up, Color.green);
        DrawVector(Vector3.zero, Vector3.right, Color.red);

        // Draw Axels
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.up);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right);


        // Local to World
        Vector3 worldPos = transform.position + localX * transform.right + localY * transform.up;

        //float worldX = worldPos.x;
        //float worldY = worldPos.y;

        Debug.Log(transform.localToWorldMatrix.ToString());

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(worldPos, 0.1f);

    }

    private void DrawVector(Vector3 from, Vector3 to, Color c)
    {
        Gizmos.color = c;
        Gizmos.DrawLine(from, to);
    }
    #endregion
}
