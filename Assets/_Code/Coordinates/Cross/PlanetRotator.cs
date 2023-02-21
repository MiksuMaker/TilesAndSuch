using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotator : MonoBehaviour
{
    #region Properties
    [SerializeField] bool spin = true;
    [Space(10)]
    [SerializeField] float earthRotationSpeed = 1f;
    [SerializeField] float moonRotationSpeed = 1f;
    [Space(30)]
    [SerializeField] GameObject earth;
    [SerializeField] GameObject moon;
    #endregion

    #region Gizmos
    private void OnDrawGizmos()
    {
        // Rotate?
        if (spin && earth != null && moon != null)
        {
            earth.transform.Rotate(new Vector3(0f, earthRotationSpeed * Time.deltaTime));
            moon.transform.Rotate(new Vector3(0f, moonRotationSpeed * Time.deltaTime));
        }
    }
    #endregion
}
