using System;
using Core.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ships.Defenses
{
    public class DefenseLayer : MonoBehaviour, IDefenseLayer, IComparable<DefenseLayer>, IHealthLayer
    {
        [Required,InlineEditor()]
        public DefenseConfig _defenseConfig;

        public int layerPriority = 1;

        private ObservedValue<float> currentHP;

        public float MaxHP => _defenseConfig.maxHP;

        public float CurrentHP => currentHP.Value;

        public event Action<float> OnCurrentHPChanged;

        private void Awake()
        {
            currentHP  = new ObservedValue<float>(_defenseConfig.maxHP);
            currentHP.OnValueChanged += f => OnCurrentHPChanged?.Invoke(f);
        }

        public void TakeDamage(DamageType type, ref float remainingDamage)
        {
            if (!enabled )
            {
                return;
            }
            var resistance = _defenseConfig.GetResistance(type);
            if (currentHP.Value > 0) { }
            //Debug.Log($"Defenses {name} got hit with {remainingDamage}:{type} - AMOUNT RESISTED = {resistance}, HP = {currentHP.Value}");
            else
                return;
            if (remainingDamage >= resistance)
            {
                remainingDamage -= Mathf.Min(currentHP.Value, resistance);
                currentHP.Value -= Mathf.Min(currentHP.Value, resistance);
                Debug.Log(($"Some Damage got through {name}"));
            }
            else
            {
                currentHP.Value -= remainingDamage;
                remainingDamage = 0;
                Debug.Log(($"Damage fully was blocked by {name}"));
            }
            
        }

        public int CompareTo(DefenseLayer other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return layerPriority.CompareTo(other.layerPriority);
        }
    }
}