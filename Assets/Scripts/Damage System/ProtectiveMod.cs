
using UnityEngine;

public class ProtectiveMod : Mod
{
    public int kineticAbsorb;
    public int laserAbsorb;
    public int plasmaAbsorb;
    public int explosiveAbsorb;
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
