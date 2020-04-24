using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(WeaponController))]
public class WeaponArcVisualizer : MonoBehaviour
{
    [Required] public LineRenderer lr;
    private WeaponController _weapon;
    [Range(1, 360)] public int smoothness = 12;

    public bool _useFancyArc = true;

    public LineRenderer left;

    private void Awake()
    {
        this._weapon = GetComponent<WeaponController>();
    }

    private void OnEnable()
    {
        if (left != null) left.enabled = true;
        lr.enabled = true;
    }

    private void OnDisable()
    {
        if (left != null) left.enabled = false;
        lr.enabled = false;
    }

    private void Update()
    {
        if (lr == null) return;
        if (_useFancyArc)
        {
            if (left == null)
            {
                left = GameObject.Instantiate(lr, lr.transform.parent);
            }
            if(left .gameObject.activeSelf==false)left.gameObject.SetActive(true);
            var lop = new List<Vector3>();
            var arcPointsNegative = GetArcPointsNegative(smoothness);
            var arcPointsPositive = GetArcPointsPositive(smoothness);
            if (lr.useWorldSpace == false)
            {
               arcPointsNegative = arcPointsNegative.Select(t => lr.transform.InverseTransformPoint(t)).ToList();
            }
            if (left.useWorldSpace == false)
            {
                arcPointsPositive = arcPointsPositive.Select(t => lr.transform.InverseTransformPoint(t)).ToList();
            }
            lr.positionCount = arcPointsNegative.Count;
            lr.SetPositions(arcPointsNegative.ToArray());
            left.positionCount = arcPointsPositive.Count;
            left.SetPositions(arcPointsPositive.ToArray());
        }
        else
        {
            if(left != null)left.gameObject.SetActive(false);
            var pnts = _weapon.GetArcPoints(smoothness);
            lr.useWorldSpace = true;
            lr.positionCount = pnts.Count;
            lr.SetPositions(pnts.ToArray());
        }
    }


    public List<Vector3> GetArcPointsNegative(int smoothness)
    {
        var lop = new List<Vector3>();

        smoothness = Mathf.Clamp(1, 360, smoothness);
        var angle = Mathf.Acos(_weapon.ConeOfFire) * Mathf.Rad2Deg;
        var line = _weapon.CenterAngle.normalized * _weapon.MaxRange;
        line = line.With(z: 0);

        var pos = transform.position;

        float curAng = -angle;
        float increment = (angle) / (float) smoothness;

        lop.Add(pos.With(z: 0));
        for (int i = 0; i < smoothness; i++)
        {
            var cur = Quaternion.AngleAxis(curAng, Vector3.forward) * line;
            lop.Add(transform.position + cur.With(z: 0));
            curAng += increment;
        }

        return lop;
    }

    public List<Vector3> GetArcPointsPositive(int smoothness)
    {
        var lop = new List<Vector3>();

        smoothness = Mathf.Clamp(1, 360, smoothness);
        var angle = Mathf.Acos(_weapon.ConeOfFire) * Mathf.Rad2Deg;
        var line = _weapon.CenterAngle.normalized * _weapon.MaxRange;
        line = line.With(z: 0);

        var pos = transform.position;

        float curAng = angle;
        float increment = (-angle) / (float) smoothness;

        
        lop.Add(pos.With(z: 0));
        for (int i = 0; i < smoothness; i++)
        {
            var cur = Quaternion.AngleAxis(curAng, Vector3.forward) * line;
            lop.Add(transform.position + cur.With(z: 0));
            curAng += increment;
        }
        return lop;
    }
}