using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldToLocal : MonoBehaviour
{
    #region Properties
    [SerializeField] public GameObject worldPoint;

    public float localX;
    public float localY;

    #endregion

    #region Gizmos
    private void OnDrawGizmos()
    {
        // Draw something on WorldPoint
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(worldPoint.transform.position, 0.2f);

        // Gét the Vector for WorldPoint
        Vector3 v = worldPoint.transform.position - transform.position;

        // Compute the Local coordinates
        localX = Vector3.Dot(v, transform.right);
        localY = Vector3.Dot(v, transform.up);



    }
    #endregion
}
