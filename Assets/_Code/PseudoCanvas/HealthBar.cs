using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    #region Properties

    // References
    [SerializeField] PseudoScreen screen;
    private Vector3 pivot = Vector3.zero;

    [Header("HealthBar Size")]
    [Range(0, 1)]
    public float percentageWidth = 0.75f;
    [Range(0, 1)]
    public float percentageHeight = 0.75f;

    [Header("Offset")]
    [Range(0, 1)]
    public float widthOffset = 0.05f;
    [Range(0, 1)]
    public float heightOffset = 0.05f;

    // Inner Healthbar size
    [Header("Health Amount")]
    [Range(0, 1)]
    public float fill = 0.5f;
    [Range(0, 1)]
    public float innerBarWidth= 0.99f;
    [Range(0, 1)]
    public float innerBarHeight= 0.99f;

    [Header("Inner Bar Offset")]
    [Range(0, 1)]
    public float innerWidthOffset = 0.05f;
    [Range(0, 1)]
    public float innerHeightOffset = 0.05f;

    #endregion


    #region Placement

    private void OnDrawGizmos()
    {
        #region Bar Outer Limits
        // Get center
        pivot = screen.pivot;

        float barHeight = percentageHeight * screen.height;
        float barWidth = percentageWidth * screen.width;


        float xOffset = screen.width * widthOffset;
        float yOffset = screen.height * heightOffset;

        // True pivot
        float pXpos = pivot.x + xOffset;
        float pYpos = pivot.y - yOffset;

        Vector3 cPivot = new Vector3(pXpos, pYpos);



        // Draw Canvas Border
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(cPivot.x, cPivot.y - barHeight), new Vector3(cPivot.x + barWidth, cPivot.y - barHeight)); // Bottom

        Gizmos.DrawLine(new Vector3(cPivot.x, cPivot.y), new Vector3(cPivot.x + barWidth, cPivot.y)); // Top
        Gizmos.DrawLine(cPivot, new Vector3(cPivot.x, cPivot.y - barHeight)); // Left
        Gizmos.DrawLine(new Vector3(cPivot.x + barWidth, cPivot.y), new Vector3(cPivot.x + barWidth, cPivot.y - barHeight)); // Right
        #endregion

        #region INNER HEALTHBAR FILL



        //pivot = screen.pivot;
        pivot = cPivot;

        float fillBarHeight = barHeight * innerBarHeight;
        float fillBarWidth = barWidth * innerBarWidth;


        //float fillxOffset = barWidth * widthOffset;

        //float fillyOffset = fillBarHeight * innerHeightOffset;

        //float fillxOffset = fillBarWidth * (innerWidthOffset * (1 - ) );



        float fillyOffset = fillBarHeight * ((1 - innerBarHeight) / 2);
        float fillxOffset = fillBarWidth * ((1 - innerBarWidth) / 2);


        // True pivot
        float fillYpos = cPivot.y - fillyOffset;
        float fillXpos = cPivot.x + fillxOffset;

        Vector3 fillPivot = new Vector3(fillXpos, fillYpos);



        // Draw Canvas Border

        // Fill End positions
        Vector3 topEnd      = new Vector3(fillPivot.x + (fillBarWidth * fill), fillPivot.y);
        Vector3 bottomEnd   = new Vector3(fillPivot.x + (fillBarWidth * fill), fillPivot.y - fillBarHeight);


        if (fill < 0.2f) { Gizmos.color = Color.red; }
        Gizmos.DrawLine(new Vector3(fillPivot.x, fillPivot.y - fillBarHeight), bottomEnd); // Bottom

        Gizmos.DrawLine(new Vector3(fillPivot.x, fillPivot.y), topEnd); // Top
        Gizmos.DrawLine(fillPivot, new Vector3(fillPivot.x, fillPivot.y - fillBarHeight)); // Left
        Gizmos.DrawLine(bottomEnd, topEnd); // Right
        #endregion
    }
    #endregion
}
