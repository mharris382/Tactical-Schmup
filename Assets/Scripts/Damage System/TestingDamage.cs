using UnityEngine;

public class TestingDamage : MonoBehaviour
{
    public GameObject testPlayer;

    //refactor these into a more generic version
    public void DealKineticDamage(int damage)
    {
        testPlayer.GetComponent<ShipHealth>().TakeDamage(DamageType.kinetic, damage);
    }

    public void DealLaserDamage(int damage)
    {
        testPlayer.GetComponent<ShipHealth>().TakeDamage(DamageType.laser, damage);
    }

    public void DealPlasmaDamage(int damage)
    {
        testPlayer.GetComponent<ShipHealth>().TakeDamage(DamageType.plasma, damage);
    }

    public void DealExplosiveDamage(int damage)
    {
        testPlayer.GetComponent<ShipHealth>().TakeDamage(DamageType.explosive, damage);
    }
}
