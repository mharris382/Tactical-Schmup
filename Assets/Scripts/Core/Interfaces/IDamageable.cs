#region

using System;

#endregion

public interface IDamageable
{
    void TakeDamage(int amount);
    event Action OnDeath;
}
