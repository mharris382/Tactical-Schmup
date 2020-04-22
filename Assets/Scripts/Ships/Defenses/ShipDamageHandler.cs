using System;
using System.Collections.Generic;
using System.Linq;
using Core.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ships.Defenses
{
    public class ShipDamageHandler : MonoBehaviour, IDamageable, IHealthLayer
    {
        [Button]
        void SearchChildren()
        {
            defenseLayers.Clear();
            defenseLayers.AddRange(GetComponentsInChildren<DefenseLayer>());
        }
        public List<DefenseLayer> defenseLayers;
        public float maxHullHP = 100;


        
        private ObservedValue<float> currentHullHP;

        public float MaxHP => maxHullHP;

        public float CurrentHP => currentHullHP.Value;

        public event Action<float> OnCurrentHPChanged;
        
        private void Awake()
        {
            currentHullHP = new ObservedValue<float>(maxHullHP);
            currentHullHP.OnValueChanged += f => OnCurrentHPChanged?.Invoke(f);
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

            currentHullHP.Value = Mathf.Clamp(currentHullHP.Value - remainingDamage, 0, maxHullHP);
            if (currentHullHP.Value <= 0)
                Debug.Log($"The Ship {this.name} is now Dead!");
        }
        
    }




    public class UiHealthIndicator : MonoBehaviour
    {
        [ValidateInput("valTarget")]
        public GameObject healthTarget;

        #region [Editor Helpers]

        bool valTarget(GameObject go, ref string msg)
        {
            if (go == null)
            {
                msg = "Health Target is required!";
                return false;
            }

            msg = "Health Target must have IHealthLayer component attached";
            return go.GetComponent<IHealthLayer>()!=null;
        }

        #endregion


        private IHealthLayer _health;

        private void Awake()
        {
            _health = healthTarget.GetComponent<IHealthLayer>();
        }
    }
}