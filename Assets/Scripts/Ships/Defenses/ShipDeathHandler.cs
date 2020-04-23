using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ships.Defenses
{
    [RequireComponent(typeof(ShipDamageHandler))]
    public class ShipDeathHandler : MonoBehaviour
    {
        [Min(0.125f)]
        public float disableGameObjectDelay = 1;
        [Required] public PooledParticle deathParticles;


        private bool _isDestroyed = false;
        private MeshRenderer _meshRenderer;
        private ShipDamageHandler _damageHandler;

        private void Awake()
        {
            _isDestroyed = false;
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
            _damageHandler = GetComponent<ShipDamageHandler>();
        }

        private void Start()
        {
            _isDestroyed = false;
            _damageHandler.OnCurrentHPChanged += f =>
            {
                if (f <= 0)
                {
                    DestroyShip();
                }
            };
        }

        private void DestroyShip()
        {
            if (_isDestroyed) return;
            _isDestroyed = true;
            SpawnDeathParticles();
            Invoke("DisableGameObject", disableGameObjectDelay);
        }

        private void SpawnDeathParticles()
        {
            var instance = deathParticles.Get<PooledParticle>();
            var shapeModule = instance.ParticleSystem.shape;
            shapeModule.meshRenderer = _meshRenderer;
            instance.Play();
        }

        private void DisableGameObject()
        {
            gameObject.SetActive(false);
        }
    }
}