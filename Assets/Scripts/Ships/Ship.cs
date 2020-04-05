using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Ship : SerializedMonoBehaviour
{
    [Range(0,0.5f)]
    [SerializeField] private float inputThreshold = 0.125f;
    [SerializeField,Required] private IShipThruster forwardThruster= new ForceThruster();
    [SerializeField,Required] private IShipThruster lateralThrusters = new ForceThruster();
    [SerializeField,Required] private IShipThruster angularThrusters = new AngularThruster();
    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float yaw = Input.GetAxis("Rotation");
        if (Mathf.Abs(yaw) > inputThreshold)
        {
            angularThrusters.EngageThruster(yaw);
        }
        else
        {
            angularThrusters.DisengageThruster();
        }
        
        if (Mathf.Abs(horizontal) > inputThreshold)
        {
            lateralThrusters.EngageThruster(horizontal);
        }
        else
        {
            lateralThrusters.DisengageThruster();
        }

        if (Mathf.Abs(vertical) > inputThreshold)
        {
            forwardThruster.EngageThruster(vertical);
        }
        else
        {
            forwardThruster.DisengageThruster();
        }
    }
}


public interface IShipThruster
{
    void EngageThruster(float input);
    void DisengageThruster();
}

[Serializable]
public class AngularThruster : IShipThruster
{
    [SerializeField, Required] private Rigidbody2D rb;
    [SerializeField] private float maxTorque = 10;
    [SerializeField,ToggleGroup("adjustAngularDrag")] private bool adjustAngularDrag = true;
    [SerializeField,ToggleGroup("adjustAngularDrag")] private float engagedAngularDrag = 0;
    [SerializeField,ToggleGroup("adjustAngularDrag")] private float disengagedAngularDrag = 0.5f;

    public void EngageThruster(float input)
    {
        rb.AddTorque(maxTorque * input, ForceMode2D.Force);
        rb.angularDrag = engagedAngularDrag;
    }

    public void DisengageThruster()
    {
        rb.angularDrag = disengagedAngularDrag;
    }
}
[Serializable]
public class ForceThruster : IShipThruster
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float maxForce = 10;
    [SerializeField] Vector2 thrusterDirection = Vector2.up;
    
    [HideReferenceObjectPicker]
    [SerializeField] private ThrusterParticles thrusterParticles;
    public void EngageThruster(float input)
    {
        if (input == 0)
        {
            DisengageThruster();
            return;
        }
        
        var force = rb.transform.TransformDirection(thrusterDirection);
        force *= maxForce * input;
        
        Debug.DrawRay(rb.position, force , Color.blue);
        rb.AddForce(force);

            thrusterParticles?.EngageThruster(input);
        

    }

    public void DisengageThruster()
    {
      thrusterParticles?.DisengageThruster();
    }
}
