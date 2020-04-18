using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

[TypeInfoBox("Use for moving ships in scenes with a perspective camera, ships should still use 2D physics")]
public class MoveShipToMouse_Perspective : MoveShipToMouseBase
{
    public override Vector3 GetMouseWorldPosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 1000, 1 << 11);
        return hit.point;
    

    }
}

