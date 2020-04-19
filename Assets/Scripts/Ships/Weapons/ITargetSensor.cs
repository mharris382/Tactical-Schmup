using System.Collections.Generic;
using UnityEngine;

namespace Ships.Weapons
{
    public interface ITargetSensor
    {
        List<Collider2D> allValidTargets { get; }
    }
}