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
    [Min(0)]
    [SerializeField] public float circleRadius = 1f;
    [Min(0)]
    [SerializeField] float arrowLength = 1f;

    [Header("Haka Angle")]
    [Min(0f)]
    [SerializeField] float hakaLength = 0.5f;
    [SerializeField] float hakaAngle = 45f;

    #endregion

    #region GIZMOS

    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position;

        // Draw Circle
        Gizmos.color = Color.white;
        //Gizmos.DrawWireSphere(Vector3.zero, circleRadius);
        Handles.color = Color.white;
        Handles.DrawWireDisc(transform.position, -Vector3.forward, circleRadius);

        // Draw Arrow Towards Handle
        Gizmos.color = Color.black;
        Gizmos.DrawLine(pos, handle.transform.position);
        Vector3 dir = (handle.transform.position - pos).normalized;
        Gizmos.DrawRay(handle.transform.position, Quaternion.AngleAxis(hakaAngle, Vector3.forward) * -dir * hakaLength);
        Gizmos.DrawRay(handle.transform.position, Quaternion.AngleAxis(-hakaAngle, Vector3.forward) * -dir * hakaLength);





        // Draw arrows away from the Radius Circle
        Gizmos.color = Color.green;
        // UP
        Gizmos.DrawRay(pos, Vector3.up * arrowLength);
        Gizmos.DrawRay(new Vector3(pos.x, pos.y + arrowLength), Quaternion.AngleAxis(-hakaAngle, Vector3.forward) * Vector3.down * hakaLength);
        Gizmos.DrawRay(new Vector3(pos.x, pos.y + arrowLength), Quaternion.AngleAxis(hakaAngle, Vector3.forward) * Vector3.down * hakaLength);

        // DOWN
        Gizmos.DrawRay(pos, Vector3.down * arrowLength);
        Gizmos.DrawRay(new Vector3(pos.x, -(pos.y + arrowLength)), Quaternion.AngleAxis(-hakaAngle, Vector3.forward) * Vector3.up * hakaLength);
        Gizmos.DrawRay(new Vector3(pos.x, -(pos.y + arrowLength)), Quaternion.AngleAxis(hakaAngle, Vector3.forward) * Vector3.up * hakaLength);



        Gizmos.color = Color.red;
        // RIGHT
        Gizmos.DrawRay(pos, Vector3.right * arrowLength);
        Gizmos.DrawRay(new Vector3(pos.x + arrowLength, pos.y), Quaternion.AngleAxis(-hakaAngle, Vector3.forward) * Vector3.left * hakaLength);
        Gizmos.DrawRay(new Vector3(pos.x + arrowLength, pos.y), Quaternion.AngleAxis(hakaAngle, Vector3.forward) * Vector3.left * hakaLength);

        // LEFT
        Gizmos.DrawRay(pos, Vector3.left * arrowLength);
        Gizmos.DrawRay(new Vector3(-(pos.x + arrowLength), 0f), Quaternion.AngleAxis(-hakaAngle, Vector3.forward) * Vector3.right * hakaLength);
        Gizmos.DrawRay(new Vector3(-(pos.x + arrowLength), 0f), Quaternion.AngleAxis(hakaAngle, Vector3.forward) * Vector3.right * hakaLength);

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
        Handles.DrawWireDisc(pos, -tr.forward, t.handleWidth);

        GUI.color = color;
        Handles.Label(pos, t.handleWidth.ToString("F1"));

    }
}

#endregion