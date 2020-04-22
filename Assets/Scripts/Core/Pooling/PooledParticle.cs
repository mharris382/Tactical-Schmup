#region

using System.Collections;
using UnityEngine;

#endregion

[RequireComponent(typeof(ParticleSystem))]
public class PooledParticle : PooledMonoBehaviour
{
    private Transform _collisionPlane;
    private ParticleSystem _particleSystem;


    public ParticleSystem ParticleSystem => _particleSystem ? _particleSystem : (_particleSystem = GetComponent<ParticleSystem>());


    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }


    public void Play()
    {
        this.enabled = true;
        
        if (!ParticleSystem.isPlaying)
            ParticleSystem.Play();
        
        StartCoroutine(disableOnComplete());
    }


    IEnumerator disableOnComplete()
    {
        yield return null;
        while (ParticleSystem.IsAlive(true))
            yield return null;
        this.enabled = false;
    }

    void InitCollisionPlane()
    {
        if (_collisionPlane == null)
        {
            var plane = new GameObject("Plane").transform;
            plane.transform.parent = transform;
            _collisionPlane = plane;
        }
    }

    public  Transform GetCollisionPlane()
    {
        InitCollisionPlane();


        return _collisionPlane;
    }
}