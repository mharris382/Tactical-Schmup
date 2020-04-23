using Sirenix.OdinInspector;
using UnityEngine;

public class RtsShip3D : MonoBehaviour, IRtsShip
{

    [SerializeField] private float _moveSpeed = 5;
    
    [Min(0), SerializeField] private float _arrivalDistance = 1;

    [MinValue("_arrivalDistance"), SerializeField] 
    private float _stoppingDistance = 2;

    
    


    [SerializeField] private ShipRotationHandler _shipRotation;

    private Vector2 _moveTarget;
    private Vector2 _lookTarget;
    
    public Vector3 MoveTarget
    {
        get => _moveTarget;
        set => this._moveTarget = value;
    }

    public Vector3 LookTarget
    {
        get => _lookTarget;
        set => _lookTarget = value;
    }

    private Rigidbody _rigidbody;
    
    
    
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        //align all ships on same plane
        var position = _rigidbody.position;
        position.z = 0;
        _rigidbody.MovePosition(position);
        
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionZ ;
        
        MoveTarget = transform.position;
        LookTarget = MoveTarget + transform.up;
        _shipRotation.ship = this;
    }


    private void FixedUpdate()
    {
        _shipRotation.Tick();

        //TODO: Move this to a movement handler
        HandleShipMovement();
    }

    private void HandleShipMovement()
    {
        
        var distToTarget = Vector2.Distance(MoveTarget, transform.position);
        if (distToTarget < _arrivalDistance)
        {
            _rigidbody.velocity = Vector2.zero;
            return;
        }

        //move to position
        var speed = distToTarget < _stoppingDistance
            ? Mathf.Lerp(0, _moveSpeed, distToTarget / _stoppingDistance)
            : _moveSpeed;

        var dir = MoveTarget - transform.position;
        dir.Normalize();
        dir.z = 0;
        _rigidbody.velocity = dir * speed;
        Debug.DrawLine(transform.position, MoveTarget, Color.red);
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = Color.red.WithAlpha(0.25f);
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, _arrivalDistance);
        UnityEditor.Handles.color = Color.yellow.WithAlpha(0.25f);
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, _stoppingDistance);
    }
    
#endif
    
    
}