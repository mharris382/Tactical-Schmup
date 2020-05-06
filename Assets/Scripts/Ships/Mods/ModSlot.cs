using System;
using UnityEngine;

namespace Ships.Mods
{
    public abstract class ModSlot : MonoBehaviour
    {
        public bool HasMod => Mod != null;

        public IMod Mod { get; private set; }
        

        public void AddMod(IMod mod)
        {
            if (!HasMod && mod != null)
            {
                Mod = mod;
                AddModToSlot(mod);
            }    
        }
        public void ReplaceMod(IMod newMod)
        {
            if (HasMod) RemoveModFromSlot(Mod);
            
            Mod = null;
            
            if (newMod != null) AddMod(newMod);
        }

        public abstract Type GetModType();
        public abstract bool IsModValidForSlot(IMod mod);

        protected abstract void AddModToSlot(IMod mod);
        
        protected abstract void RemoveModFromSlot(IMod mod);
    }


    public abstract class ModSlot<T> : ModSlot where T : class, IMod
    {
        public new T Mod => base.Mod as T;
        
        public sealed override Type GetModType() => typeof(T);

        

        protected sealed override void AddModToSlot(IMod mod) => AddModToSlot((T)mod);
        protected sealed override void RemoveModFromSlot(IMod mod) => RemoveModFromSlot((T) mod);
        
        
        public override bool IsModValidForSlot(IMod mod) => mod is T;
        
        protected  abstract void AddModToSlot(T mod);
        
        protected  abstract void RemoveModFromSlot(T mod);
    }
}