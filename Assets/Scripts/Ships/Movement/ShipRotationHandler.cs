using System;
using UnityEngine;


/// <summary>
/// rotates ship so that it always faces a single world space point (LookTarget) 
/// </summary>
[Serializable]
public class ShipRotationHandler
{
    public Vector3 worldUp = Vector3.up;
    public float _rotationSpeed = 10;
    [HideInInspector]
    public RtsShip ship;

    public  void Tick()
    {
        var lookTarget = ship.LookTarget - ship.transform.position;
        lookTarget.Normalize();
        var transform = ship.transform;

        var angle = Vector3.SignedAngle(worldUp, lookTarget, Vector3.forward);
        var targetRot = Quaternion.Euler(new Vector3(0, 0, angle));
        ship.transform.rotation = Quaternion.Slerp(ship.transform.rotation, targetRot, Time.fixedDeltaTime * _rotationSpeed);
        // ship.transform.rotation = ;
    }
}