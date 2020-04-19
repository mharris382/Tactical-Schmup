using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Ships.Defenses
{
    [CreateAssetMenu(menuName = "ShipMods/Defense Config")]
    public class DefenseConfig : SerializedScriptableObject
    {
        [Min(1)] public float maxHP = 100;

        [InfoBox("$totalResistanceMessage"),ValidateInput("hasAllResistances"),TableList(AlwaysExpanded = true),SerializeField]
        private List<DamageTypeResistance> resistances = new List<DamageTypeResistance>();


        private Dictionary<DamageType, DamageTypeResistance> _resistanceLookup;

        [System.Serializable]
        private class DamageTypeResistance
        {
            [ReadOnly, TableColumnWidth(75, false)]
            public DamageType type;

            [PropertyRange(0, "rangeMax")] public float resistance;


#if UNITY_EDITOR
            float rangeMax
            {
                get
                {
                    var defenseConfig = Selection.activeObject as DefenseConfig;
                    if (defenseConfig != null)
                    {
                        return defenseConfig.maxHP;
                    }

                    return 100;
                }
            }
#endif
        }


        
        #region [Runtime Code]

        void InitResistanceLookup()
        {
            if (_resistanceLookup == null)
            {
                _resistanceLookup = new Dictionary<DamageType, DamageTypeResistance>();
                foreach (var res in resistances)
                {
                    _resistanceLookup.Add(res.type, res);
                }
            }
        }

        public float GetResistance(DamageType type)
        {
            InitResistanceLookup();
            return _resistanceLookup[type].resistance;
        }

        #endregion


        #region [Editor Code]

        string totalResistanceMessage
        {
            get
            {
                
                float sum = 0;
                foreach (var resistance in resistances)
                {
                    sum += resistance.resistance;
                }

                return $"Total Resistance = <b>{sum}</b>";
            }
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            var values = Enum.GetValues(typeof(DamageType));
            foreach (var value in values)
            {
                var type = (DamageType) value;


                var resist = resistances.FindIndex(t => t.type == type);
                if (resist == -1)
                {
                    resistances.Add(new DamageTypeResistance()
                    {
                        type = type,
                        resistance = 10
                    });
                }
            }

            resistances = resistances.Distinct(new ResistComparer()).ToList();
        }

        private class ResistComparer : IEqualityComparer<DamageTypeResistance>
        {
            public bool Equals(DamageTypeResistance x, DamageTypeResistance y)
            {
                return x.type == y.type;
            }

            public int GetHashCode(DamageTypeResistance obj)
            {
                return obj.GetHashCode();
            }
        }


        bool hasAllResistances(List<DamageTypeResistance> resistances)
        {
            OnValidate();
            return true;
        }
#endif

        #endregion
    }
}