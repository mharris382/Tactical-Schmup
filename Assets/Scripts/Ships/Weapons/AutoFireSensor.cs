using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoFireSensor : MonoBehaviour
{
    private WeaponController _weaponController;


    private void Awake()
    {
        _weaponController = GetComponent<WeaponController>();
    }


    private void Update()
    {
        var targets = _weaponController.FindAllTargets();
        var best = targets.OrderByDescending(t => Vector2.Dot(t.transform.position - transform.position, transform.up)).FirstOrDefault();
        if (best != null)
        {
            _weaponController.FireAtTarget(best.transform);
        }
        else
        {
            _weaponController.StopFiring();
        }
    }
}
