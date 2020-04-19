using System;
using System.Collections.Generic;
using Ships.Weapons;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable,InlineProperty]
public class SensorOnGameObject : ITargetSensor
{
    [HideLabel]
    [ValidateInput("valSensor")]
    public GameObject sensor;

    bool valSensor(GameObject go, ref string msg)
    {
        if (go == null)
        {
            msg = "Sensor is required";
            return false;
        }

        msg = "Sensor must have ITargetSensor component attached";
        return go.GetComponent<ITargetSensor>() !=null;
    }


    private ITargetSensor _sensor;

    private ITargetSensor Sensor
    {
        get
        {
            if (_sensor == null)
            {
                _sensor = sensor.GetComponent<ITargetSensor>();
            }
            return _sensor;
        }
    }

    List<Collider2D> ITargetSensor.allValidTargets => Sensor.allValidTargets;
}