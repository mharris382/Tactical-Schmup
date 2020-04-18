
using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class ProtectiveMod : Mod
{
   [BoxGroup("Absorb Amounts")] public int kineticAbsorb;
   [BoxGroup("Absorb Amounts")] public int laserAbsorb;
   [BoxGroup("Absorb Amounts")] public int plasmaAbsorb;
   [BoxGroup("Absorb Amounts")] public int explosiveAbsorb;
   [BoxGroup("Absorb Amounts")] public int emotionalAbsorb;
    
    public int totalDefenceHitPoints;
    public int currentDefenceHitPoints;
    public int rechargeRate;

    private void Awake()
    {
        currentDefenceHitPoints = totalDefenceHitPoints;
    }

    //add an update or tick method that adds to currentDefenceHitPoints based on rechargeRate. 
    //Not sure if we do this on Update timer, or centrally managed by a timer on the ship itself

    public int ProcessDamage(DamageType damageType, int damage)
    {
        int remainingDamage = damage;
        int absorbAmount = 0;
        switch (damageType)
        {
            case DamageType.none:
                break;
            case DamageType.kinetic:
                absorbAmount = kineticAbsorb;
                break;
            case DamageType.explosive:
                absorbAmount = explosiveAbsorb;
                break;
            case DamageType.laser:
                absorbAmount = laserAbsorb;
                break;
            case DamageType.plasma:
                absorbAmount = plasmaAbsorb;
                break;
            case DamageType.emotional:
                absorbAmount = emotionalAbsorb;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(damageType), damageType, null);
        }

        var damageAbsorbed = Mathf.Min(currentDefenceHitPoints, absorbAmount);
        remainingDamage -= damageAbsorbed;
        currentDefenceHitPoints -= damageAbsorbed;
        return remainingDamage;
        if (damageType == DamageType.kinetic)
        {
            remainingDamage -= kineticAbsorb;
            currentDefenceHitPoints -= remainingDamage;
        }

        if (damageType == DamageType.laser)
        {
            remainingDamage -= laserAbsorb;
            currentDefenceHitPoints -= remainingDamage;
        }
        
        if (damageType == DamageType.plasma)
        {
            remainingDamage -= plasmaAbsorb;
            currentDefenceHitPoints -= remainingDamage;
        }

        if (damageType == DamageType.explosive)
        {
            remainingDamage -= explosiveAbsorb;
            currentDefenceHitPoints -= remainingDamage;
        }

        if (remainingDamage <= 0)
        {
            Debug.Log("Mod: " + modType + " " + modName + " blocked all of the " + damageType + " damage");
            return 0;
        }
        else 
        {
            Debug.Log("Mod: " + modType + " " + modName + " blocked " + (damage - remainingDamage).ToString() + " points of " + damageType + " damage");
            Debug.Log("There are: " + remainingDamage + " points of " + damageType + " damage remaining to block");
            return remainingDamage;
        }
    }

}
