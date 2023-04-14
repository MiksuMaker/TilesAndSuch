using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    #region Properties
    [SerializeField]
    BezierPath bezierPath;

    [SerializeField] GameObject car;
    [Min(0f)]
    [SerializeField] float travel = 0f;
    [SerializeField] bool driveInEditor = true;

    [Header("Controls")]
    [Range(-10f, 10f)]
    [SerializeField] float speed = 1f;
    #endregion

    #region Setup
    private void Start()
    {
        // Find References
        //bezierPath = FindObjectOfType<BezierPath>();

        StartCoroutine(DriveCar());
    }

    //private void Update()
    //{
    //    UpdateCarPosition(travel);
    //}

    private void OnDrawGizmos()
    {
        if (bezierPath == null) { return; }
        if (!driveInEditor) { return; }
        UpdateCarPosition(travel);
    }
    #endregion

    private void UpdateCarPosition(float travel)
    {
        
        var PosAndRot = bezierPath.GetPosOnTheRoad(travel);

        car.transform.position = PosAndRot.pos;

        Quaternion rot = Quaternion.LookRotation(-PosAndRot.rot);

        car.transform.rotation = rot;
    }

    IEnumerator DriveCar()
    {
        WaitForSeconds wait = new WaitForSeconds(0.01f);



        while (true)
        {

            travel += speed * 0.001f;

            // Loop back to one
            if (travel > 1)
            {
                travel = travel % 1;
            }
            else if (travel < 0)
            {
                travel = travel + 1;
            }


            yield return wait;

            // Move the car
            UpdateCarPosition(travel);
        }
    }
}
