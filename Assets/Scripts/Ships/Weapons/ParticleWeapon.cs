using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Abstract this into a ship weapon base class or interface 
public class ParticleWeapon : MonoBehaviour
{
    [SerializeField] private DamageType _damageType;
    [SerializeField] private int _damage = 1;
    private ParticleSystem _particleSystem;
    public bool _crazyFire = true;
    public Vector3 AimDirection
    {
        set { transform.up = value; }
    }

    public DamageType DamageType => _damageType;




    private void OnParticleCollision(GameObject other)
    {
       var damageable = other.GetComponent<IDamageable>();
       if (damageable != null)
       {
           damageable.TakeDamage(new DamageInfo()
           {
               amount = this._damage,
               type =  this._damageType
           });
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

    public Vector3 GetAimPosition(Rigidbody2D targetRigidbody)
    {
        throw new NotImplementedException();
    }
}