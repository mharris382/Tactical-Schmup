using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    
    [SerializeField] private float _maxRange = 5;
    [SerializeField] private float _coneOfFire = .5f;

    

    public float ConeOfFire => _coneOfFire;

    [InlineEditor()]
    public ParticleWeapon weaponTransform;

    private Transform _currTarget;
    private bool _firing;

    private Vector3 CenterAngle => transform.up;

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
        
        if (dot < _coneOfFire)
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, CenterAngle.normalized * _maxRange);
        
        
    }
}