using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

public class MoveShipToMousePosition : MonoBehaviour
{
    public float selectShipCastRadius = 1;
    public RtsShip ship;

    public RtsShip[] allShips;

    Dictionary<Collider2D, RtsShip> shipColliders = new Dictionary<Collider2D, RtsShip>();
    private void Awake()
    {
        ship.GetComponent<PlayerRtsShip>().IsSelected = true;
        foreach (var rtsShip in allShips)
        {
            var cols = rtsShip.GetComponentsInChildren<Collider2D>();
            
            Debug.Assert(cols.IsNullOrEmpty()==false, "Ship has no colliders on it", rtsShip);
            Debug.Assert(rtsShip.GetComponent<PlayerRtsShip>()!=null, "Ship missing PlayerRtsShip Component", rtsShip);
            
            foreach (var c in cols)
            {
                shipColliders.Add(c, rtsShip);
            }
        }
    }

    private void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = ship.transform.position.z;

        if (CheckForMovement(mousePos)) return;

        //pause time
        CheckForPauseToggle();

        CheckForRotation(mousePos);
    }

    private void CheckForRotation(Vector3 mousePos)
    {
        //set look position / refactor this into enemy targetting
        if (Input.GetMouseButtonDown(1))
        {
            if (CheckForEnemyToTarget(mousePos)) return;

            ship.LookTarget = mousePos; //refactor this into an enemy, perhaps interface
        }
        else if (Input.GetMouseButton(1))
        {
            ship.LookTarget = mousePos; //refactor this into an enemy, perhaps interface
        }
    }

    private static void CheckForPauseToggle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = Math.Abs(Time.timeScale) < 1 ? 1 : 0;
        }
    }

    private bool CheckForMovement(Vector3 mousePos)
    {
        //set move destination, or select another ship
        if (Input.GetMouseButtonDown(0))
        {
            if (CheckForSelectedShip(mousePos)) return true;

            ship.MoveTarget = mousePos;
        }
        else if (Input.GetMouseButton(0))
        {
            ship.MoveTarget = mousePos;
        }

        return false;
    }

    private bool CheckForEnemyToTarget(Vector3 mousePos)
    {
        var colliders = Physics2D.OverlapCircleAll(mousePos, selectShipCastRadius);
        var c = colliders.FirstOrDefault(t => shipColliders.ContainsKey(t));
        if (c != null && shipColliders[c] != ship)
        {
            
            ship.gameObject.GetOrAddComponent<AutoLookAtTransform>().lookAt = c.transform;
            //if the ship is an enemy, set as the target and update ui
            return false; //update this to return true when targetting
        }
        else
        {
            ship.gameObject.GetOrAddComponent<AutoLookAtTransform>().lookAt = null;
        }

        return false;
    }

    private bool CheckForSelectedShip(Vector3 mousePos)
    {
        var colliders = Physics2D.OverlapCircleAll(mousePos, selectShipCastRadius);
        var c = colliders.FirstOrDefault(t => shipColliders.ContainsKey(t));
        if (c != null)
        {
            var previousShip = ship;

            if (previousShip != null)
                previousShip.GetComponent<PlayerRtsShip>().IsSelected = false;

            ship = shipColliders[c];
            ship.GetComponent<PlayerRtsShip>().IsSelected = true;
            return true;
        }

        return false;
    }
}



public class FireAtMouse : MonoBehaviour
{
    public WeaponController weapon;


   
}