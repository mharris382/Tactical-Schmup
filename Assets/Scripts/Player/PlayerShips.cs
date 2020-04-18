using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

public class PlayerShips : Singleton<PlayerShips>
{
    public List<PlayerRtsShip> playerControlledShips;

   
    
    private void Awake()
    {
        playerControlledShips.RemoveAll(t => t == null);
        if (playerControlledShips.IsNullOrEmpty())
        {
            Debug.LogError("There are no player controlled ships in the scene! Add the PlayerRtsShip component to ships to make them player controlled");
            return;
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