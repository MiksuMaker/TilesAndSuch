using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CoordinateDrawer : MonoBehaviour
{
    #region Properties

    [Header("Handle")]
    [SerializeField] public GameObject handle;
    [SerializeField] public float handleWidth = 1f;


    [Header("Coordinates")]
    [SerializeField] float circleRadius = 1f;
    [SerializeField] float arrowLength = 1f;

    [Header("Haka Angle")]
    [SerializeField] float hakaMultiplier = 0.5f;
    [SerializeField] float hakaAngle = 45f;

    #endregion

    #region GIZMOS

    private void OnDrawGizmos()
    {
        // Draw Circle
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(Vector3.zero, circleRadius);

        // Draw Arrow Towards Handle
        Gizmos.color = Color.black;
        Gizmos.DrawLine(Vector3.zero, handle.transform.position);
        Vector3 dir = (handle.transform.position - transform.position).normalized;
        Gizmos.DrawRay(handle.transform.position, Quaternion.AngleAxis(hakaAngle, Vector3.forward) * -dir * hakaMultiplier);
        Gizmos.DrawRay(handle.transform.position, Quaternion.AngleAxis(-hakaAngle, Vector3.forward) * -dir * hakaMultiplier);





        // Draw arrows away from the Radius Circle
        Gizmos.color = Color.green;
        // UP
        Gizmos.DrawRay(new Vector3(0f, circleRadius), Vector3.up * arrowLength);
        Gizmos.DrawRay(new Vector3(0f, circleRadius + arrowLength), Quaternion.AngleAxis(-hakaAngle, Vector3.forward) * Vector3.down * hakaMultiplier);
        Gizmos.DrawRay(new Vector3(0f, circleRadius + arrowLength), Quaternion.AngleAxis(hakaAngle, Vector3.forward) * Vector3.down * hakaMultiplier);

        // DOWN
        Gizmos.DrawRay(new Vector3(0f, -circleRadius), Vector3.down * arrowLength);
        Gizmos.DrawRay(new Vector3(0f, -(circleRadius + arrowLength)), Quaternion.AngleAxis(-hakaAngle, Vector3.forward) * Vector3.up * hakaMultiplier);
        Gizmos.DrawRay(new Vector3(0f, -(circleRadius + arrowLength)), Quaternion.AngleAxis(hakaAngle, Vector3.forward) * Vector3.up * hakaMultiplier);



        Gizmos.color = Color.red;
        // RIGHT
        Gizmos.DrawRay(new Vector3(circleRadius, 0f), Vector3.right * arrowLength);
        Gizmos.DrawRay(new Vector3((circleRadius + arrowLength), 0f), Quaternion.AngleAxis(-hakaAngle, Vector3.forward) * Vector3.left * hakaMultiplier);
        Gizmos.DrawRay(new Vector3((circleRadius + arrowLength), 0f), Quaternion.AngleAxis(hakaAngle, Vector3.forward) * Vector3.left * hakaMultiplier);

        // LEFT
        Gizmos.DrawRay(new Vector3(-circleRadius, 0f), Vector3.left * arrowLength);
        Gizmos.DrawRay(new Vector3(-(circleRadius + arrowLength), 0f), Quaternion.AngleAxis(-hakaAngle, Vector3.forward) * Vector3.right * hakaMultiplier);
        Gizmos.DrawRay(new Vector3(-(circleRadius + arrowLength), 0f), Quaternion.AngleAxis(hakaAngle, Vector3.forward) * Vector3.right * hakaMultiplier);

    }

    #endregion

}

// Editor TOOL
#region Handle

[CustomEditor(typeof(CoordinateDrawer))]
public class HandleEditor : Editor
{
    public void OnSceneGUI()
    {

        var t = target as CoordinateDrawer;
        var tr = t.handle.transform;
        var pos = tr.position;

        var color = Color.blue;
        Handles.color = color;
        Handles.DrawWireDisc(pos, -tr.forward, 1f);

        GUI.color = color;
        Handles.Label(pos, t.handleWidth.ToString("F1"));

    }
}

#endregion