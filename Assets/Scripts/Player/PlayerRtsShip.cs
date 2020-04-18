using System;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(RtsShip))]
public class PlayerRtsShip : MonoBehaviour, IRtsShip
{
    [Required]
    [SerializeField] private GameObject selection = null;

    private RtsShip _rtsShip;
    private bool _isSelected;

    public Vector3 LookTarget
    {
        set => _rtsShip.LookTarget = value;
    }

    public Vector3 MoveTarget
    {
        set => _rtsShip.MoveTarget = value;
    }

    public bool IsSelected
    {
        set => SetSelected(value);
        get => _isSelected;
    }

    private void Awake()
    {
        this._rtsShip = GetComponent<RtsShip>();
    }

    private void SetSelected(bool value)
    {
        _isSelected = value;
        selection.gameObject.SetActive(value);
    }
}