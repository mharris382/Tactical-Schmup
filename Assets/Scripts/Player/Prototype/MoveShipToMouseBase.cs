using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

public abstract class MoveShipToMouseBase : MonoBehaviour
{
    public float selectShipCastRadius = 1;
    public bool enableClickToSelect = true;
    public bool enableClickToTargetEnemy = true;
    public bool enablePauseTime = false;
    public LayerMask enemyLayers = 1 << 0;
    public LayerMask allyLayers = 1 << 0;
    Dictionary<Collider2D, PlayerRtsShip> shipColliders = new Dictionary<Collider2D, PlayerRtsShip>();
    private Vector2 mouseMovedPosition;

    private void Start()
    {
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

        if (WasShipSelected(mousePos)) return;
        if (WasEnemyTargeted(mousePos))
        {
            Debug.Log("Enemy Targeted");
            return;
        }

        if (enablePauseTime)
            CheckForPauseToggle();//pause time

        MoveSelectedShip(mousePos);
        RotateSelectedShip(mousePos);
    }

    public abstract Vector3 GetMouseWorldPosition();


    private static void CheckForPauseToggle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = Math.Abs(Time.timeScale) < 1 ? 1 : 0;
        }
    }


    #region [Rotation & Movement]

    private void RotateSelectedShip(Vector3 mousePos)
    {
        if (Input.GetMouseButton(1))
        {
            PlayerShips.Instance.ShipSelection.LookTarget = mousePos;
        }
    }
    private void MoveSelectedShip(Vector3 mousePos)
    {
        if (Input.GetMouseButton(0))
        {
            mouseMovedPosition = mousePos;
            PlayerShips.Instance.ShipSelection.MoveTarget = mousePos;
        }
    }
    private bool WasEnemyTargeted(Vector3 mousePos)
    {
        return enableClickToTargetEnemy && Input.GetMouseButtonDown(1) && CheckForEnemyToTarget_2D(mousePos);
    }
    private bool CheckForEnemyToTarget_2D(Vector3 mousePos)
    {
        var colliders = Physics2D.OverlapCircleAll(mousePos, selectShipCastRadius, enemyLayers);
        var c = colliders.FirstOrDefault(t => !shipColliders.ContainsKey(t) && t.attachedRigidbody != null && t.attachedRigidbody.GetComponent<IRtsShip>() != null);
        if (c != null   )
        {
            foreach (var ship in PlayerShips.Instance.GetSelectedShips().Where(t => t.gameObject != null))
            {
                ship.gameObject.GetOrAddComponent<AutoLookAtTransform>().lookAt = c.attachedRigidbody.transform;
            }
            return false; //update this to return true when targetting
        }
        else
        {
            foreach (var ship in PlayerShips.Instance.GetSelectedShips().Where(t => t.gameObject != null))
            {
                ship.gameObject.GetOrAddComponent<AutoLookAtTransform>().lookAt = null;
            }

        }

        return false;
    }

    #endregion




    #region [Ship Selection]

    private bool WasShipSelected(Vector3 mousePos)
    {
        return enableClickToSelect && Input.GetMouseButtonDown(0) && CheckForSelectedShip_2D(mousePos);
    }

    private bool CheckForSelectedShip_2D(Vector3 mousePos)
    {
        Collider2D c = FindPlayerShipColliderAtPosition(mousePos);

        if (c != null)
        {
            return SelectShipWithCollider(c);
        }

        return false;
    }

    private Collider2D FindPlayerShipColliderAtPosition(Vector3 mousePos)
    {
        var colliders = Physics2D.OverlapCircleAll(mousePos, selectShipCastRadius, allyLayers);
        var c = colliders.FirstOrDefault(t => shipColliders.ContainsKey(t));
        return c;
    }


    private bool SelectShipWithCollider(Collider2D c)
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            foreach (var sp in PlayerShips.Instance.GetSelectedShips())
            {
                if (shipColliders[c] != sp)
                    sp.IsSelected = false;
            }
        }

       var ship = shipColliders[c];
        ship.IsSelected = true;
        return true;
    }
    #endregion
}