using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialTrigger : Trigger
{
    #region Properties

    #endregion

    #region Functions
    protected override void IsTargetInSight()
    {
        // Test if Target is within radius
        float sqrRad = radius * radius;
        if (Vector3.SqrMagnitude(target.pos - ownPos) < sqrRad)
        {
            // Target in sight
            DoTargetInSightEffect();
        }
        else
        {
            // Target Out of reach
            DoTargetOutOfRangeEffect();
        }
    }

    #endregion
}
