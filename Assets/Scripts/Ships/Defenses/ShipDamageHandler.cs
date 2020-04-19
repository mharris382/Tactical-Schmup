using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ships.Defenses
{
    public class ShipDamageHandler : MonoBehaviour, IDamageable
    {
        [Button]
        void SearchChildren()
        {
            defenseLayers.Clear();
            defenseLayers.AddRange(GetComponentsInChildren<DefenseLayer>());
        }
        public List<DefenseLayer> defenseLayers;
        public float maxHullHP = 100;


        
        private float currentHullHP;

        private void Awake()
        {
            currentHullHP = maxHullHP;
        }

        public void TakeDamage(DamageInfo damage)
        {
            defenseLayers.Sort();
            Debug.Log($"Ship {this.name} was hit! I hope our defenses hold!");
            float remainingDamage = damage.amount;
            DamageType type = damage.type;
            foreach (var defenseLayer in defenseLayers)
            {
                defenseLayer.TakeDamage(type, ref remainingDamage);
                if (remainingDamage <= 0) return;
            }

            currentHullHP -= remainingDamage;
            if (currentHullHP <= 0)
                Debug.Log($"The Ship {this.name} is now Dead!");
        }
        
    }
}