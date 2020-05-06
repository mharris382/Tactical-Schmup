using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ships.Mods
{
    public class ShipModManager : MonoBehaviour
    {
        [HideInPlayMode]
        public bool isTemplate = true;

        public Transform hudParent;
        
        private Dictionary<Type, List<ModSlot>> _unusedSlots = new Dictionary<Type, List<ModSlot>>();

        
        
        
    }
}