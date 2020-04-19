using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace Ships.Weapons
{
    public abstract class AutoFireSensorBase : MonoBehaviour
    {
        protected WeaponController _weaponController { get; private set; }
   

   
        private void Awake()
        {
            _weaponController = GetComponent<WeaponController>();
        }

        private void Update()
        {
            var best = GetSensor().allValidTargets
                .Where(IsValidTarget).ToList()
                .OrderByDescending(GetTargetScore)
                .FirstOrDefault();


            if (best != null)
            {
                FireAtTarget(best);
            }
            else
            {
                StopFiring();
            }
        }


        private void StopFiring()
        {
            _weaponController.StopFiring();
        }

        private void FireAtTarget(Collider2D best)
        {
            _weaponController.FireAtTarget(best.transform);
        }

        protected abstract bool IsValidTarget(Collider2D collider2D);

        protected abstract float GetTargetScore(Collider2D t);
      

        public abstract ITargetSensor GetSensor();
    }
}