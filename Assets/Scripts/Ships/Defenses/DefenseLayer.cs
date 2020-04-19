using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ships.Defenses
{
    public class DefenseLayer : MonoBehaviour, IDefenseLayer, IComparable<DefenseLayer>
    {
        [Required,InlineEditor()]
        public DefenseConfig _defenseConfig;

        public int layerPriority = 1;

        private float currentHP;


        private void Awake()
        {
            currentHP = _defenseConfig.maxHP;
        }

        public void TakeDamage(DamageType type, ref float remainingDamage)
        {
            if (!enabled)
            {
                return;
            }
            var resistance = _defenseConfig.GetResistance(type);
            var absorbed = Mathf.Min(currentHP, resistance);
            currentHP -= absorbed;
            remainingDamage -= absorbed;
        }

        public int CompareTo(DefenseLayer other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return layerPriority.CompareTo(other.layerPriority);
        }
    }
}