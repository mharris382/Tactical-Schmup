using System;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.XR;

public class WeaponController : MonoBehaviour
{
    
    [SerializeField] private float _maxRange = 5;
    [SerializeField] private float _coneOfFire = .5f;

    

    public float ConeOfFire => _coneOfFire;

    [InlineEditor()]
    public ParticleWeapon weaponTransform;

    private Transform _currTarget;
    private bool _firing;

    public Vector3 CenterAngle
    {
        get{return  transform.up;}
        set { transform.up = value; }
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
#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow.WithAlpha(0.45f);
        Handles.color = Color.yellow.WithAlpha(0.5f);
        var angle = ((_coneOfFire - 1) / 2f) * 360f;
        var line =CenterAngle.normalized* _maxRange;
        var pRotated = Quaternion.AngleAxis( angle /2, Vector3.forward) * line;
        var nRotated = Quaternion.AngleAxis(-angle/2, Vector3.forward) * line;
        var pos = transform.position;
        //   Gizmos.DrawRay(pos, line);
        Handles.DrawWireArc(pos, Vector3.forward , nRotated, angle, _maxRange );
        Gizmos.DrawRay(pos, pRotated);
        Gizmos.DrawRay(pos, nRotated);


    }
#endif
}

#if UNITY_EDITOR


[CustomEditor(typeof(WeaponController))]
public class WeaponControllerEditor : OdinEditor
{
    private void OnSceneGUI()
    {
        var wep = target as WeaponController;
        Transform transform = wep.transform;
        var coneProp = serializedObject.FindProperty("_coneOfFire");
        var rangeProp = serializedObject.FindProperty("_maxRange");
        float range = rangeProp.floatValue;
        float dot = coneProp.floatValue;
        var angle = ((dot - 1) / 2f) * 360f;
        var line =wep.CenterAngle.normalized* range;
        var pRotated = Quaternion.AngleAxis( angle /2, Vector3.forward) * line;
        var nRotated = Quaternion.AngleAxis(-angle/2, Vector3.forward) * line;
        var pos = transform.position;
        Handles.color = Color.yellow;
        Handles.DrawLine(pos, pos + pRotated);
        Handles.DrawLine(pos, pos + nRotated);
        Handles.DrawWireArc(pos, Vector3.forward , nRotated, angle, range );
   
        var handlePos = pos + line;
        var handleSize = HandleUtility.GetHandleSize(pos) * 0.035f;

        var prevDist = Vector2.Distance(pos, handlePos);
        var newPos = Handles.Slider(handlePos, wep.CenterAngle.normalized, handleSize, Handles.DotHandleCap, 0);
        var newDist = Vector2.Distance(pos, newPos);
        if (Math.Abs(newDist - prevDist) > Mathf.Epsilon)
        {
            rangeProp.floatValue = newDist;
            return;
        }
        var rotation = Quaternion.LookRotation(nRotated, Vector3.forward);
        Handles.color = Color.clear;
         
        var newRotation =Handles.Disc(rotation, pos, Vector3.forward, Mathf.Max(range-1,0.1f), true, 0);
        if (newRotation != rotation)
        {
            var center = Quaternion.LookRotation(wep.CenterAngle, Vector3.forward);
            var newAngle = Quaternion.Angle(center, newRotation);
            coneProp.floatValue = Quaternion.Dot(center, newRotation);
        }
        //Handles.color = Color.yellow;
        

        serializedObject.ApplyModifiedProperties();
    }
}
#endif