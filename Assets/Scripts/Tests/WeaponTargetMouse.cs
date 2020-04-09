using UnityEngine;

public class WeaponTargetMouse : MonoBehaviour
{
    [SerializeField] private WeaponController[] _controllers;
   
    private Transform _targetPoint;
    
    private void Awake()
    {
        _targetPoint = new GameObject("WeaponAutoTargetPoint").transform;
        _targetPoint.hideFlags = HideFlags.HideInHierarchy;
    }
    
    
    private void Update()
    {
        MoveTargetPointToMousePosition();
        if (Input.GetMouseButtonDown(1))
        {
            foreach (var _controller in _controllers)
            {
                
                _controller.FireAtTarget(_targetPoint);
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            foreach (var _controller in _controllers)
            {
                _controller.StopFiring();
            }
        }
    }

    private void MoveTargetPointToMousePosition()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = _controllers[0].transform.position.z;
        _targetPoint.position = mousePosition;
    }
}