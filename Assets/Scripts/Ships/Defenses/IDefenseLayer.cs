namespace Ships.Defenses
{
    public interface IDefenseLayer
    {
        void TakeDamage(DamageType type, ref float remainingDamage);
    }
}