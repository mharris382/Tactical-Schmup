using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

public class PlayerShips : Singleton<PlayerShips>
{
    public List<PlayerRtsShip> playerControlledShips;
    private PlayerShipSelection _selection;
    public IRtsShip ShipSelection => _selection;
    
    private void Awake()
    {
        playerControlledShips.RemoveAll(t => t == null);
        if (playerControlledShips.IsNullOrEmpty())
        {
            Debug.LogError("There are no player controlled ships in the scene! Add the PlayerRtsShip component to ships to make them player controlled");
            return;
        }
        _selection = new PlayerShipSelection();
    }

    private void Update()
    {
        for(int i = 0; i < playerControlledShips.Count; i++)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                DeselectAllShips();
                playerControlledShips[i].IsSelected = true;
                break;
            }
        }
    }


    public void DeselectAllShips()
    {
        playerControlledShips.ForEach(t => t.IsSelected = false);
    }
    
    public PlayerRtsShip GetFirstSelectedShip()
    {
        return playerControlledShips.FirstOrDefault(t => t.IsSelected);
    }

    public IEnumerable<PlayerRtsShip> GetSelectedShips()
    {
        return playerControlledShips.Where(t => t.IsSelected);
    }

    private class PlayerShipSelection : IRtsShip
    {
        public GameObject gameObject => Instance.GetFirstSelectedShip()?.gameObject;

        public Transform transform => Instance.GetFirstSelectedShip()?.transform;

        public Vector3 LookTarget
        {
            get
            {
                var ship = Instance.GetFirstSelectedShip();
              return ship != null ? ship.LookTarget : Vector3.zero ;
            }

            set
            {
                foreach (var ship in Instance.GetSelectedShips())
                {
                    ship.LookTarget = value;
                }
            }
        }

        public Vector3 MoveTarget
        {
            set
            {
                foreach (var ship in Instance.GetSelectedShips())
                {
                    ship.MoveTarget = value;
                }
            }
        }
    }

    #region [Editor Helpers]

    [Button(ButtonSizes.Small)]
    private void FindPlayerControlledShips()
    {
        playerControlledShips.Clear();
        playerControlledShips.AddRange(FindObjectsOfType<PlayerRtsShip>());
        Debug.Assert(playerControlledShips.IsNullOrEmpty() == false, "No Player ships in scene");
    }

    private void OnValidate()
    {
        playerControlledShips.RemoveAll(t => t == null);
        if (playerControlledShips == null)
        {
            FindPlayerControlledShips();
        }
    }

    #endregion

    
}