using System;
using System.Collections;
using System.Collections.Generic;
using Ships.Movement;
using UnityEngine;

public class EngineMovement : MonoBehaviour, IShipController
{
    public float engineForce = 100;
    
    public float thrusterForce = 25;
    public float arrivalDistance = 10;

    private Rigidbody2D _rigidbody2D;
    private RtsShipController _controller;
    private bool _hasReachedDestination;
    
    
    private Vector2 MoveTarget => _controller.MoveTarget;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _controller = GetComponent<RtsShipController>();
    }


    private void OnEnable()
    {
        SetMovementOverriding(true);
    }

    private void OnDisable()
    {
        SetMovementOverriding(false);
        SetRotationOverriding(false);
    }

    private void FixedUpdate()
    {
        _hasReachedDestination = Vector2.Distance(MoveTarget, _rigidbody2D.position) < arrivalDistance && 
                                 _rigidbody2D.velocity.magnitude < thrusterForce;
        
        if (_hasReachedDestination)
        {
            SetRotationOverriding(false);
            _rigidbody2D.drag = 10000;
        }
        else
        {
            SetRotationOverriding(true);
            var directionToTarget = _rigidbody2D.position - MoveTarget;
            directionToTarget.Normalize();
            _rigidbody2D.AddForce(directionToTarget * engineForce, ForceMode2D.Force);
            _rigidbody2D.drag = 0;
        }

   
        // var velocityDirection = _rigidbody2D.velocity;
        
    }

    private void SetRotationOverriding(bool overrideRotation) => _controller.ShipRotationHandler = overrideRotation ? this : null;

    private void SetMovementOverriding(bool overrideMovement) => _controller.ShipMovementController = overrideMovement ? this : null;

    
    public void HandleMovement(RtsShipController ship)
    {
        
    }
    

    public void HandleRotation(RtsShipController shipController)
    {
        
    }
}
