using Ships.Defenses;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ships.Mods
{
    public abstract class DefenseMod : IMod
    {
        public DefenseConfig shieldDefenseLayer;
        public UiHealthCircle healthHud;

        protected DefenseMod(DefenseConfig shieldDefenseLayer, UiHealthCircle healthHud)
        {
            this.shieldDefenseLayer = shieldDefenseLayer;
            this.healthHud = healthHud;
        }
    }

    public class Shield : DefenseMod
    {
        public Shield(DefenseConfig shieldDefenseLayer, UiHealthCircle healthHud) : base(shieldDefenseLayer, healthHud)
        {
        }
    }

    public class Armor : DefenseMod
    {
        public Armor(DefenseConfig shieldDefenseLayer, UiHealthCircle healthHud) : base(shieldDefenseLayer, healthHud)
        {
        }
    }

}