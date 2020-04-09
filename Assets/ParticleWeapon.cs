﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleWeapon : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    private ParticleSystem _particleSystem;
    public bool _crazyFire = true;
    public Vector3 AimDirection
    {
        set { transform.up = value; }
    }
    
    private void OnParticleCollision(GameObject other)
    {
       var damageable = other.GetComponent<IDamageable>();
       if (damageable != null)
       {
           damageable.TakeDamage(_damage);
       }
    }


    public void Awake()
    {
        _particleSystem= GetComponent<ParticleSystem>();
    }

    
    public void StartFiring()
    {
        if(_crazyFire)
            _particleSystem.Play();
        else if(!_particleSystem.isPlaying)
            _particleSystem.Play();
    }
    
    
    public void StopFiring()
    {
        _particleSystem.Stop();
    }
}