using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

//TODO add an interface to abstract weapons
public class WeaponController : MonoBehaviour
{
    [FoldoutGroup("Weapon Info")]
    public string weaponName;
    [FoldoutGroup("Weapon Info"),TextArea(2,8)]
    public string description;
    [Range(-1,1)]
    [SerializeField] private float _coneOfFire = .5f;

    private Transform _currTarget;
    private bool _firing;

    [SerializeField] private float _maxRange = 5;

    [InlineEditor] public ParticleWeapon weaponTransform;


    public float ConeOfFire => _coneOfFire;

    
    public Vector3 CenterAngle
    {
        get => transform.up;
        set => transform.up = value;
    }

    
    public void FireAtTarget(Transform target)
    {
        _currTarget = target;
    }

    
    public void StopFiring()
    {
        weaponTransform.StopFiring();
        _firing = false;
        _currTarget = null;
    }

    
    private void Update()
    {
        if (_currTarget == null) return;


        var dir = _currTarget.position - transform.position;
        var dot = Vector2.Dot(CenterAngle, dir.normalized);

        if (IsTargetOutsideAngle(_currTarget))
        {
            _firing = false;
            weaponTransform.AimDirection = CenterAngle;
            weaponTransform.StopFiring();
        }
        else
        {
            weaponTransform.AimDirection = dir.normalized;
            var dist = dir.magnitude;
            _firing = dist <= _maxRange;
            weaponTransform.StartFiring();
        }

        if (_firing)
        {
            weaponTransform.StartFiring();
        }
        else
        {
            weaponTransform.StopFiring();
        }
    }

    
    private bool IsTargetOutsideAngle(Transform target)
    {
        var dir = target.position - transform.position;
        var dot = Vector2.Dot(CenterAngle, dir.normalized);
        return dot < _coneOfFire;
    }


    //TODO: Move this to a seperate class
    public List<Collider2D> FindAllTargets()
    {
        var colls = Physics2D.OverlapCircleAll(transform.position, _maxRange).ToList();
        colls.RemoveAll(t => IsTargetOutsideAngle(t.transform));
        return colls;
    }


    public List<Vector3> GetArcPoints(int smoothness)
    {
        var lop = new List<Vector3>();

        smoothness = Mathf.Clamp(1, 360, smoothness);
        var angle = Mathf.Acos(_coneOfFire) * Mathf.Rad2Deg ;
        var line = CenterAngle.normalized * _maxRange;
        line = line.With(z: 0);
        
        var pos = transform.position;
        
        float curAng = -angle;
        float increment = (angle * 2) / (float)smoothness;
        
        lop.Add(pos .With(z:0));
        for (int i = 0; i < smoothness; i++)
        {
            var cur = Quaternion.AngleAxis(curAng, Vector3.forward) * line;
            lop.Add(transform.position + cur.With(z:0));
            curAng += increment;
        }
        lop.Add(pos .With(z:0));
        return lop;
    }

    #region [EDITOR]

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Color color = Color.grey;
        if (weaponTransform != null)
        {
            color = DamageTypeColors.GetColor(weaponTransform.DamageType);
        }

        Gizmos.color = color.WithAlpha(0.35f);
        Handles.color = color.WithAlpha(0.35f);
        var angle = ((_coneOfFire - 1) / 2f) * 360f;
        var line = CenterAngle.normalized * _maxRange;
        var pRotated = Quaternion.AngleAxis(angle / 2, Vector3.forward) * line;
        var nRotated = Quaternion.AngleAxis(-angle / 2, Vector3.forward) * line;
        var pos = transform.position;
        //   Gizmos.DrawRay(pos, line);
        Handles.DrawWireArc(pos, Vector3.forward, nRotated, angle, _maxRange);
        Gizmos.DrawRay(pos, pRotated);
        Gizmos.DrawRay(pos, nRotated);
    }
#endif

    #endregion
}


public static class DamageTypeColors
{
    public static Tuple<DamageType, Color>[] typeColors = new[]
    {
        new Tuple<DamageType, Color>(DamageType.none, Color.white),
        new Tuple<DamageType, Color>(DamageType.emotional, Color.magenta),
        new Tuple<DamageType, Color>(DamageType.laser, Color.red),
        new Tuple<DamageType, Color>(DamageType.plasma, Color.green),
        new Tuple<DamageType, Color>(DamageType.kinetic, Color.cyan),
        new Tuple<DamageType, Color>(DamageType.explosive,
            new Color(246f / 235f, 131f / 235f, 28f / 235f, 1)), //rgb(242,131,28)
    };

    public static Color GetColor(DamageType type)
    {
        DamageType dType = type;
        Color color = Color.white;
        try
        {
            var tup = typeColors.First(t => t.Item1 == dType);
            color = tup.Item2;
        }
        catch (Exception e)
        {
            Debug.LogError($"No color was found for damage type {dType}");
            color = Color.white;
        }

        return color;
    }
}