using System;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif
namespace Ships.Movement
{
    public class RtsShipController : MonoBehaviour, IRtsShip
    {
        private IShipRotation _rotationHandler;
        private IShipMovement _movementHandler;


        [SerializeField] private ShipMovement defaultShipMovement;
        [SerializeField] private ShipRotationHandler defaultShipRotation;





        public Vector3 LookTarget { get; set; }
        public Vector3 MoveTarget { get; set; }

        private Rigidbody2D _rigidbody;

        public Rigidbody2D Rigidbody => _rigidbody;

        public IShipMovement ShipMovementController
        {
            set { _movementHandler = value; }
            private get { return _movementHandler ?? defaultShipMovement; }
        }

        
        //TODO: add a bool property in IShipMovement.BlocksRotation which will replace this gross mess (can delete IShipController)
        public IShipRotation ShipRotationHandler
        {
            set => _rotationHandler = value;
            get => _rotationHandler ?? defaultShipRotation;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            MoveTarget = transform.position;
            LookTarget = transform.position + transform.up;
        }


        private void FixedUpdate()
        {
            ShipRotationHandler.HandleRotation(this);
            ShipMovementController.HandleMovement(this);
        }

        
    }
    
    
        [Serializable]
        public class ShipMovement : IShipMovement
        {
            [SerializeField] private float _moveSpeed = 5;
            [MinValue(0), SerializeField] private float _arrivalDistance = 10;

            [MinValue("_arrivalDistance"), SerializeField]
            private float _stoppingDistance = 15;


            public void HandleMovement(RtsShipController ship)
            {
                var transform = ship.transform;
                var rigidbody2D = ship.Rigidbody;


                var dirToTarget =  (Vector2) ship.MoveTarget -rigidbody2D.position;


                //more efficient way to do distance comparison
                if (dirToTarget.sqrMagnitude < (_arrivalDistance * _arrivalDistance))
                {
                    rigidbody2D.velocity = Vector2.zero;
                    return;
                }


                var distToTarget = Vector2.Distance(ship.MoveTarget, transform.position);

                var speed = distToTarget < _stoppingDistance
                    ? Mathf.Lerp(0, _moveSpeed, distToTarget / _stoppingDistance)
                    : _moveSpeed;


                dirToTarget /= distToTarget; //more efficient way to normalize the vector
                rigidbody2D.velocity = dirToTarget * speed;


                Debug.DrawLine(transform.position, ship.MoveTarget, Color.red);
            }


            #region [Gizmos]

#if UNITY_EDITOR
            public void OnDrawGizmosSelected(RtsShipController ship)
            {
                Handles.color = Color.red.WithAlpha(0.25f);
                Handles.DrawWireDisc(ship.transform.position, Vector3.forward, _arrivalDistance);

                Handles.color = Color.yellow.WithAlpha(0.25f);
                Handles.DrawWireDisc(ship.transform.position, Vector3.forward, _stoppingDistance);
            }
#endif

            #endregion
        }


        
        [Serializable]
        public class ShipRotation : IShipRotation
        {
            [Range(0, 180)] public float rotationSpeed = 15;

            public void HandleRotation(RtsShipController shipController)
            {
                
                var lookTarget = shipController.LookTarget - shipController.transform.position;
                lookTarget.Normalize();

                var angle = Vector3.SignedAngle(shipController.transform.up, lookTarget, Vector3.forward);
                var targetRot = Quaternion.Euler(new Vector3(0, 0, angle));
                shipController.transform.rotation = Quaternion.Slerp(shipController.transform.rotation, targetRot,
                    Time.fixedDeltaTime * rotationSpeed);
            }
        }
}