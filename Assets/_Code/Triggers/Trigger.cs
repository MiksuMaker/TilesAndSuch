using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    #region Properties
    protected Target target;
    protected Vector3 ownPos {  get { return transform.position; } }

    [Header("Material")]
    [SerializeField]
    protected Material base_Material;
    [SerializeField]
    protected Material inRadius_Material;
    [SerializeField]
    protected Material inSight_Material;
    protected MeshRenderer renderer;
    
    
    [Header("Radius")]
    [SerializeField] protected float radius = 20f;


    protected enum NoticeMode
    {
        not, inRadius, inSight
    }
    #endregion

    #region Setup
    protected virtual void Start()
    {
        // Get references
        target = FindObjectOfType<Target>();

        renderer = GetComponent<MeshRenderer>();
        { if (renderer == null) { renderer = GetComponentInChildren<MeshRenderer>(); } }
        { if (renderer == null) { Debug.Log("Renderer not found for Object " + gameObject.name); } }
    }

    protected virtual void FixedUpdate()
    {
        IsTargetInSight();
    }
    #endregion

    #region Functions
    protected virtual void IsTargetInSight()
    {

    }

    protected virtual void DoTargetOutOfRangeEffect()
    {
        ChangeMaterial(NoticeMode.not);
    }

    protected virtual void DoTargetInRadiusEffect()
    {
        Debug.DrawLine(transform.position, target.pos, Color.yellow, 0.1f);
        ChangeMaterial(NoticeMode.inRadius);
    }

    protected virtual void DoTargetInSightEffect()
    {
        Debug.DrawLine(transform.position, target.pos, Color.red, 0.1f);
        ChangeMaterial(NoticeMode.inSight);
    }

    protected virtual void DrawTargetLine(bool inRadius, bool inSight)
    {
        Color c = Color.white;
        if (inRadius)
        {
            c = Color.yellow;
        }
        else if (inSight)
        {
            c = Color.red;
        }
        else
        {
            // Cancel
            return;
        }

        // Drawline
        Debug.DrawLine(transform.position, target.pos, c, 0.1f);
    }

    protected virtual void ChangeMaterial(NoticeMode mode)
    {
        // Change Material accordingly
        // ...(if nothing is missing)

        if (renderer == null) { Debug.Log("Rederer for " + gameObject.name + " not found."); return; }

        if (mode == NoticeMode.not)
        {
            if (base_Material == null) { Debug.Log("BaseMaterial missing on " + gameObject.name + "!"); return; }

            renderer.material = base_Material;
        }
        else if (mode == NoticeMode.inRadius)
        {
            if (inRadius_Material == null) { Debug.Log("InRadiusMaterial missing on " + gameObject.name + "!"); return; }

            renderer.material = inRadius_Material;
        }
        else // In Sight
        {
            if (inSight_Material == null) { Debug.Log("InSightMaterial missing on " + gameObject.name + "!"); return; }

            renderer.material = inSight_Material;
        }
    }
    #endregion
}
