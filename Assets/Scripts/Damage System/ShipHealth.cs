using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    public int totalHullHitPoints;
    public int currentHullHitPoints;
    public ProtectiveMod shieldMod;
    public ProtectiveMod armorMod;

    private void Awake()
    {
        currentHullHitPoints = totalHullHitPoints;

        ProtectiveMod[] mods = GetComponents<ProtectiveMod>();
        foreach (ProtectiveMod mod in mods)
        {
            if (mod.modType == ModType.shield)
            {
                shieldMod = mod;
            }

            if (mod.modType == ModType.armor)
            {
                armorMod = mod;
            }
        }
    }

    public void TakeDamage(DamageType damageType, int damage)
    {
        if (currentHullHitPoints <= 0)
        {
            Debug.Log("You are DEAD! Why'd you go and die for??");
            return;
        }

        int remainingDamage = damage;

        if (shieldMod.currentDefenceHitPoints == 0)
        {
            Debug.Log("Shield: " + shieldMod.modName + " is broken and didn't block anything");
            if (armorMod.currentDefenceHitPoints == 0)
            {
                Debug.Log("Armor: " + armorMod.modName + " is broken and didn't block anything");
                Debug.Log("Hull took " + damage +" points of damage");
                currentHullHitPoints -= damage;
            }
            else
            {
                remainingDamage = armorMod.ProcessDamage(damageType, damage);
                Debug.Log("Hull took " + remainingDamage + " points of damage");
                currentHullHitPoints -= remainingDamage;
            }
        }
        else 
        {
            remainingDamage = shieldMod.ProcessDamage(damageType, damage);
            if (armorMod.currentDefenceHitPoints == 0)
            {
                Debug.Log("Hull took " + remainingDamage + " points of damage");
                currentHullHitPoints -= remainingDamage;
            }
            else
            {
                remainingDamage = armorMod.ProcessDamage(damageType, remainingDamage);
                Debug.Log("Hull took " + remainingDamage + " points of damage");
                currentHullHitPoints -= remainingDamage;
            }
        }

        if (currentHullHitPoints <= 0)
        {
            Debug.Log("You are DEAD! Why'd you go and die for??");
        }
    }
}
