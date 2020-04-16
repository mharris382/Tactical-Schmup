public enum DamageType { none, kinetic, explosive, laser, plasma, emotional }

public struct DamageInfo
{
    public int amount;
    public DamageType type;

    public DamageInfo(int amount, DamageType type)
    {
        this.amount = amount;
        this.type = type;
    }
    
    
    
    
}