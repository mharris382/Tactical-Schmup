using System;
using Ships.Movement;
using UnityEngine;


/// <summary>
/// rotates ship so that it always faces a single world space point (LookTarget) 
/// </summary>
[Serializable]
public class ShipRotationHandler :  IShipRotation
{
    public Vector3 worldUp = Vector3.up;
    public float _rotationSpeed = 10;
    [HideInInspector]
    public IRtsShip ship;

    private Vector3 localLookTarget;
    private Vector3 lastWorldTarget;
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


    public void HandleRotation(RtsShipController shipController)
    {
        ship = shipController;
        var autoLookTarget = shipController.GetComponent<AutoLookAtTransform>();
        if (ship.LookTarget != lastWorldTarget || (autoLookTarget != null && autoLookTarget.lookAt != null))
        {
            localLookTarget = shipController.LookTarget - shipController.transform.position;
            lastWorldTarget = ship.LookTarget;
        }
        var lookTarget = localLookTarget;
        lookTarget.Normalize();
        var transform = shipController.transform;

        var angle = Vector3.SignedAngle(worldUp, lookTarget, Vector3.forward);
        var targetRot = Quaternion.Euler(new Vector3(0, 0, angle));
        shipController.transform.rotation = Quaternion.Slerp(shipController.transform.rotation, targetRot, Time.fixedDeltaTime * _rotationSpeed);
    }
}