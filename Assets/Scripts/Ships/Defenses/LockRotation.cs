using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    
    public Transform _relativeTransform;
    

    private Vector3 initialOffset;
    private void Awake()
    {
        if (_relativeTransform == null) return;
        initialOffset = transform.position-_relativeTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;
        if (_relativeTransform == null) return;
        transform.position = _relativeTransform.position + initialOffset;
    }


    // private void OnValidate()
    // {
    //     if (_relativeTransform == null)
    //     {
    //         var rb = GetComponentInParent<Rigidbody2D>();
    //         if (rb != null)
    //             _relativeTransform = rb.transform;
    //     }
    // }
}