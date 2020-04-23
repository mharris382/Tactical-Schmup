using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

public abstract class MoveShipToMouseBase : MonoBehaviour
{
    public float selectShipCastRadius = 1;
    public PlayerRtsShip ship;


    Dictionary<Collider2D, PlayerRtsShip> shipColliders = new Dictionary<Collider2D, PlayerRtsShip>();
    private Vector2 mouseMovedPosition;

    private void Start()
    {
        if (ship != null)
            ship.IsSelected = true;
        Debug.Assert(PlayerShips.Instance != null, "<b>The PlayerShips Singleton is not present in scene!</b>");
        var allShips = PlayerShips.Instance.playerControlledShips;
        foreach (var rtsShip in allShips)
        {
            var cols = rtsShip.GetComponentsInChildren<Collider2D>();
            Debug.Assert(cols.IsNullOrEmpty() == false, "Ship has no colliders on it", rtsShip);

            foreach (var c in cols)
            {
                shipColliders.Add(c, rtsShip);
            }
        }
    }

    private void Update()
    {
        var mousePos = GetMouseWorldPosition();
        if (ship != null) mousePos.z = ship.transform.position.z;

        if (CheckForMovement(mousePos)) return;

        //pause time
        CheckForPauseToggle();

        CheckForRotation(mousePos);
    }

    public abstract Vector3 GetMouseWorldPosition();


    private static void CheckForPauseToggle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = Math.Abs(Time.timeScale) < 1 ? 1 : 0;
        }
    }


    #region [Rotation]

    private void CheckForRotation(Vector3 mousePos)
    {
        //set look position / refactor this into enemy targetting
        if (Input.GetMouseButton(1))
        {
            // //if (CheckForEnemyToTarget_2D(mousePos)) return;
            // if ((ship != null) && Vector2.Distance(mousePos, mouseMovedPosition) >= 1)
            // {
            //     var lookDirection = (mouseMovedPosition - (Vector2)mousePos).normalized;
            //     
            //     ship.LookTarget = lookDirection ; //refactor this into an enemy, perhaps interface
            // }
            ship.LookTarget = mousePos;
        }

        // else if (Input.GetMouseButton(1))
        // {
        //     if (ship != null)
        //         ship.LookTarget = mousePos; //refactor this into an enemy, perhaps interface
        // }
    }

    private bool CheckForEnemyToTarget_2D(Vector3 mousePos)
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

    #endregion


    #region [Movement]

    private bool CheckForMovement(Vector3 mousePos)
    {
        //set move destination, or select another ship
        if (Input.GetMouseButtonDown(0))
        {
            if (CheckForSelectedShip_2D(mousePos)) return true;
            mouseMovedPosition = mousePos;
            MoveShipToMouse(mousePos);
        }
        else if (Input.GetMouseButton(0))
        {
            MoveShipToMouse(mousePos);
        }

        return false;
    }

    private void MoveShipToMouse(Vector3 mousePos)
    {
        ship.MoveTarget = mousePos;
    }


    private bool CheckForSelectedShip_2D(Vector3 mousePos)
    {
        var colliders = Physics2D.OverlapCircleAll(mousePos, selectShipCastRadius);
        var c = colliders.FirstOrDefault(t => shipColliders.ContainsKey(t));
        if (c != null)
        {
            var previousShip = ship;

            if (previousShip != null)
                previousShip.gameObject.GetComponent<PlayerRtsShip>().IsSelected = false;

            ship = shipColliders[c];
            ship.IsSelected = true;
            return true;
        }

        return false;
    }

    #endregion
}