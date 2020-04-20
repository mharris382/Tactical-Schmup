
using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Enemies
{
    public  class StupidEnemy : MonoBehaviour
    {
        [Required, ValidateInput("valShipObject")]
        public GameObject shipGameObject;


        public float engagementRange = 10;
        public Transform currentTarget;
        private IRtsShip ship;
        

        private void Awake()
        {
            ship = shipGameObject.GetComponent<IRtsShip>();
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


        public virtual Transform GetCurrentTarget()
        {
            return currentTarget;
        }

        #region [Editor Helpers]

#if UNITY_EDITOR
        private bool valShipObject(GameObject go, ref string msg)
        {
            if (go == null)
            {
                msg = "Ship is required!";
                return false;
            }

            msg = "Ship must have an IRtsShip component attached";
            return go.GetComponent<IRtsShip>() != null;
        }
#endif

        #endregion
    }
}