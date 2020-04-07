using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class RtsShip : MonoBehaviour
{

    [SerializeField] private float _moveSpeed = 5;
    
    [Min(0), SerializeField] private float _arrivalDistance = 1;
    
    [MinValue("_arrivalDistance"), SerializeField] 
    private float _stoppingDistance = 2;
    
    

    public Vector3 Target { get; set; }
    
    private Rigidbody2D _rigidbody2D;
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        Target = transform.position;
    }


    private void FixedUpdate()
    {
        var distToTarget = Vector2.Distance(Target, transform.position);
        if (distToTarget < _arrivalDistance)
        {
            _rigidbody2D.velocity = Vector2.zero;
            return;
        }

        var speed = distToTarget < _stoppingDistance
            ? Mathf.Lerp(0, _moveSpeed, distToTarget / _stoppingDistance)
            : _moveSpeed;

        var dir = Target - transform.position;
        Debug.DrawLine(transform.position, Target, Color.red);
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