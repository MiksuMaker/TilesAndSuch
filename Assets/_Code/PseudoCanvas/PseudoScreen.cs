using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudoScreen : MonoBehaviour
{
    #region Properties

    public bool centered = false;
    [HideInInspector]
    public Vector3 pivot = Vector3.zero;

    [Header("Canvas Size")]
    public float width = 800f;
    public float height = 600f;


    #endregion

    #region Setup
    private void Start()
    {
        // Set Pivot
        if (centered)
        {
            pivot = new Vector3(-(width / 2), -(height / 2));
        }
    }
    #endregion


    #region GIZMOS
    private void OnDrawGizmos()
    {
        #region Borders
        if  (centered)  { pivot = new Vector3(transform.position.x - (width / 2), transform.position.y + (height / 2)); }
        else            { pivot = new Vector3(transform.position.x, transform.position.y + height) ; }

        Gizmos.DrawWireSphere(pivot, 50f);

        // Draw Borders from pivot
        Gizmos.color = Color.white;
        Gizmos.DrawLine(new Vector3(pivot.x, pivot.y - height), new Vector3(pivot.x + width, pivot.y - height)); // Bottom
        Gizmos.DrawLine(new Vector3(pivot.x, pivot.y), new Vector3(pivot.x + width, pivot.y)); // Top
        Gizmos.DrawLine(pivot, new Vector3(pivot.x, pivot.y - height)); // Left
        Gizmos.DrawLine(new Vector3(pivot.x + width, pivot.y), new Vector3(pivot.x + width, pivot.y - height)); // Right
        #endregion


    }


    #endregion
}
