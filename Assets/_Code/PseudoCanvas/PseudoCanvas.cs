using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudoCanvas : MonoBehaviour
{
    #region Properties

    // References
    [SerializeField] PseudoScreen screen;
    private Vector3 pivot = Vector3.zero;

    [Header("Canvas Size")]
    public float percentageWidth  = 0.75f;
    public float percentageHeight = 0.75f;


    #endregion


    #region Placement

    private void OnDrawGizmos()
    {
        // Get center
        pivot = screen.pivot;

        float cHeight = percentageHeight * screen.height;
        float cWidth = percentageWidth * screen.width;


        float xOffset = screen.width *  ((1 - percentageWidth) / 2);
        float yOffset = screen.height * ((1 - percentageHeight) / 2);

        // True pivot
        float pXpos = pivot.x + xOffset;
        float pYpos = pivot.y - yOffset;

        Vector3 cPivot = new Vector3(pXpos, pYpos);



        // Draw Canvas Border
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(cPivot.x, cPivot.y - cHeight), new Vector3(cPivot.x + cWidth, cPivot.y - cHeight)); // Bottom





        Gizmos.DrawLine(new Vector3(cPivot.x, cPivot.y), new Vector3(cPivot.x + cWidth, cPivot.y)); // Top
        Gizmos.DrawLine(cPivot, new Vector3(cPivot.x, cPivot.y - cHeight)); // Left
        Gizmos.DrawLine(new Vector3(cPivot.x + cWidth, cPivot.y), new Vector3(cPivot.x + cWidth, cPivot.y - cHeight)); // Right

    }

    #endregion
}
