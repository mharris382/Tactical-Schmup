using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleWeaponPsWrapper : MonoBehaviour
{
    [OnValueChanged("updateLifetime"),SerializeField] private float _distance = 50;
    [OnValueChanged("updateLifetime"),SerializeField] private float _speed = 50;
        
    
    
    public float Distance
    {
        get => _distance;
        set
        {
            _distance = value;
            updateLifetime();
        }
    }
    public float Speed
    {
        get => _speed;
        set
        {
            _speed = value;
            updateLifetime();
        }
    }
    
    

    void updateLifetime()
    {
        UpdateLifetime(ps);
        foreach (var ps in AllProjectileParticles())
        {
            UpdateLifetime(ps);
        }
    }
    
    void UpdateLifetime(ParticleSystem ps)
    {
        var dist = _distance;
        var speed = _speed;
        var lifetime = dist / speed;
        lifetime *= 1.125f;
        var main = ps.main;
        var startLifetime = main.startLifetime;
        startLifetime.mode = ParticleSystemCurveMode.Constant;
        startLifetime.constant = lifetime;
        main.startLifetime = startLifetime;
        var startSpeed = main.startSpeed;
        startSpeed.mode = ParticleSystemCurveMode.Constant;
        startSpeed.constant = speed;
        main.startSpeed = startSpeed;
        
    }

    
    private ParticleSystem _ps;
    
    private const string ProjectileTag = "Projectile";

    public ParticleSystem ps
    {
        get
        {
            if (_ps == null)
            {
                _ps = GetComponent<ParticleSystem>();
            }

            _ps.tag = ProjectileTag;
            return _ps;
        }
    }

    private ParticleSystem[] AllProjectileParticles()
    {
        var pss = GetComponentsInChildren<ParticleSystem>().Where(t => t.CompareTag(ProjectileTag));
        
        return pss.ToArray();
    }

    private void OnDrawGizmosSelected()
    {
        var dir = transform.forward;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + (dir * _distance), 0.5f);
    }
}
