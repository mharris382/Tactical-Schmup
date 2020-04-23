using System;

namespace Ships.Defenses
{
    public interface IHealthLayer
    {
        float MaxHP { get; }
        float CurrentHP { get; }
        event Action<float> OnCurrentHPChanged;
    }
}