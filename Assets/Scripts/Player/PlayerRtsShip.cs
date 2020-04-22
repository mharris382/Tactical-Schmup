﻿using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;


public class PlayerRtsShip : MonoBehaviour, IRtsShip
{
    [Required]
    [SerializeField] private GameObject selection = null;

    private IRtsShip _rtsShip;
    private bool _isSelected;

    public Vector3 LookTarget
    {
        set => _rtsShip.LookTarget = value;
        get => _rtsShip.LookTarget;
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
        this._rtsShip = GetComponents<IRtsShip>().FirstOrDefault(t => t != this);
        Debug.Assert(_rtsShip != null, "Player Ship has no other IRtsShip component attached!", this);
    }

    private void SetSelected(bool value)
    {
        _isSelected = value;
        selection.gameObject.SetActive(value);
    }
}