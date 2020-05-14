using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
   // public InputAction rotateClockwise;
   // public InputAction rotateCounterClockwise;
    
   public Transform moveTransform;
    public float moveSpeed = 15;
    [Range(0,1)] public float moveSmoothing = .1f;
    public float lookTargetRadius = 100;
    
    
    public Transform zoomTransform;
    public float zoomSpeed = 25;
    [Range(0,1)] public float zoomSmoothing = 0.1f;
    public float minZoomDist = 10;
    public float maxZoomDist = 50;
    
    
  
    
  

    
    
    
    
    
    [BoxGroup("Rotation")] public float rotationSpeed = 15;
    [Range(0,1)] public float rotationSmoothing = 0.1f;
    
    private float _targetAngle;
    private float _currentAngle;
    private float _currentRotationVelocity;


    private Vector3 _targetPosition;
    private Vector3 _currPosition;
    private Vector3 _currentMoveVelocity;


    private Vector3 _zoomTargetPos, _zoomCurrPos, _currZoomVelocity;
    private int angleIncrement;

    private void Start()
    {
        StartCoroutine(CheckForRotationInput());
        angleIncrement = 45;
        //rotateClockwise.performed += context => { targetAngle += angleIncrement; };
        //rotateCounterClockwise.performed += context => { targetAngle -= angleIncrement; };
        _currPosition =_targetPosition = moveTransform.position;
        _zoomCurrPos = _zoomTargetPos = zoomTransform.position;
        _currentMoveVelocity = _currZoomVelocity = Vector3.zero;
        _currentAngle = _targetAngle = moveTransform.rotation.eulerAngles.y;
    }

    private void Update()
    {
        Move();
        Zoom();
        Rotate();
    }

    void Move()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        var move = moveTransform;
        Vector3 dir = move.forward * zInput + move.right * xInput;
        _targetPosition += dir * (moveSpeed * Time.deltaTime);
        
        
        moveTransform.position =  _currPosition = Vector3.SmoothDamp(_currPosition, _targetPosition, ref _currentMoveVelocity, moveSmoothing);

    }


    void Rotate()
    {
        _currentAngle =  Mathf.SmoothDampAngle(_currentAngle, _targetAngle, ref _currentRotationVelocity, rotationSmoothing);
        moveTransform.rotation = Quaternion.Euler(0, _currentAngle, 0);

    }

    void Zoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float dist = Vector3.Distance(moveTransform.position, zoomTransform.position);
        if (dist < minZoomDist && scrollInput > 0.0f)
            return;
        else if (dist >= maxZoomDist && scrollInput < 0.0f)
            return;

        _zoomTargetPos += zoomTransform.forward * (scrollInput * zoomSpeed);
        zoomTransform.localPosition = _zoomCurrPos = Vector3.SmoothDamp(_zoomCurrPos, _zoomTargetPos, ref _currZoomVelocity, zoomSmoothing);
        
        
    }

     IEnumerator CheckForRotationInput()
     {
         while (true)
         {
             if (Input.GetKeyDown(KeyCode.E))
             {
                 _targetAngle += angleIncrement;
                 yield return new WaitForSeconds(0.5f);
                 while (Input.GetKey(KeyCode.E))
                 {
                     _targetAngle += angleIncrement;
                     yield return new WaitForSeconds(0.125f);
                 }
             }
             else if (Input.GetKeyDown(KeyCode.Q))
             {
                 _targetAngle -= angleIncrement;
                 yield return new WaitForSeconds(0.5f);
                 while (Input.GetKey(KeyCode.Q))
                 {
                     _targetAngle -= angleIncrement;
                     yield return new WaitForSeconds(0.125f);
                 }
             }
    
             yield return null;
         }
     }
    
    
    
    public void FocusOnPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    
    
    
    [ShowInInspector]
    float curZoomDistance 
    {
        get
        {
            float dist = Vector3.Distance(moveTransform.position, zoomTransform.position);
            return dist;
        }
    }
}