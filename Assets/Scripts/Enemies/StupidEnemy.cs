
using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Enemies
{
    public  class StupidEnemy : MonoBehaviour
    {
        [Required]
        public RtsShip ship;

        public float engagementRange = 10;
        public Transform currentTarget;

        public virtual Transform GetCurrentTarget()
        {
            return currentTarget;
        }


        private void Update()
        {
            currentTarget = GetCurrentTarget();

            if (currentTarget == null)
            {
                ship.LookTarget = ship.transform.position + ship.transform.up;
                ship.MoveTarget = ship.transform.position;
                return;
            }


            ship.LookTarget = currentTarget.position;
            var dist = Vector2.Distance(ship.transform.position, currentTarget.position);
            if (dist > engagementRange)
            {
                var dirToTarget =(ship.transform.position - currentTarget.position).normalized;
                dirToTarget *= engagementRange;
                ship.MoveTarget = currentTarget.position + dirToTarget;
            }
            else
            {
                ship.MoveTarget = ship.transform.position;
            }
        }
    }
}