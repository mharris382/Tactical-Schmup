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
