using System.Collections.Generic;
using Ships.Weapons;
using UnityEngine;

public class TriggerTargetSensor : MonoBehaviour, ITargetSensor
{
    private List<Collider2D> _allValidTargets=new List<Collider2D>();
    private Rigidbody2D _rb;

    public List<Collider2D> allValidTargets
    {
        get => _allValidTargets;
    }

    private void Awake()
    {
        this._rb = GetComponentInParent<Rigidbody2D>();
        _allValidTargets=new List<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody == _rb) return;
        allValidTargets.Add(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.attachedRigidbody == _rb && !allValidTargets.Contains(other)) return;
        allValidTargets.Remove(other);
    }
}