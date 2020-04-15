using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


public class RtsShip : MonoBehaviour, IRtsShip
{

    [SerializeField] private float _moveSpeed = 5;
    
    [Min(0), SerializeField] private float _arrivalDistance = 1;

    [MinValue("_arrivalDistance"), SerializeField] 
    private float _stoppingDistance = 2;

    [Min(0), SerializeField] private float _aimStopDegrees = 3;
    public bool faceMoveDirection;


    [SerializeField] private ShipRotationHandler _shipRotation;
    

    public Vector3 MoveTarget { get; set; }
    public Vector3 LookTarget { get; set; }

    private Rigidbody2D _rigidbody2D;
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        MoveTarget = transform.position;
        LookTarget = transform.up;
        _shipRotation.ship = this;
    }


    private void FixedUpdate()
    {
        _shipRotation.Tick();

        //TODO: Move this to a movement handler
        HandleShipMovement();
    }

    private void HandleShipMovement()
    {
        if (faceMoveDirection)
            LookTarget = MoveTarget;

        var distToTarget = Vector2.Distance(MoveTarget, transform.position);
        if (distToTarget < _arrivalDistance)
        {
            _rigidbody2D.velocity = Vector2.zero;
            return;
        }

        //move to position
        var speed = distToTarget < _stoppingDistance
            ? Mathf.Lerp(0, _moveSpeed, distToTarget / _stoppingDistance)
            : _moveSpeed;

        var dir = MoveTarget - transform.position;
        Debug.DrawLine(transform.position, MoveTarget, Color.red);
        dir.Normalize();

        _rigidbody2D.velocity = dir * speed;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _arrivalDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _stoppingDistance);
    }
}

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