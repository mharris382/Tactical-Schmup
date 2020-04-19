using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ships.Weapons;
using UnityEngine;

public class AutoFireSensor : AutoFireSensorBase
{
    
    public SensorOnGameObject sensor;

    protected override bool IsValidTarget(Collider2D collider2D)
    {
        return true;
    }

    protected override float GetTargetScore(Collider2D t)
    {
        var transform1 = this.transform;

        bool hasDamageable = t.GetComponentInParent<IDamageable>() != null;

        return Vector2.Dot(t.transform.position - transform1.position, transform1.up)
               + ((hasDamageable) ? 100 : 0);
    }

    public override ITargetSensor GetSensor()
    {
        return sensor;
    }
}