using UnityEngine;

[RequireComponent(typeof(RtsShip))]
public class AutoLookAtTransform : MonoBehaviour
{

    public Transform lookAt;
    private RtsShip _rtsShip;

    private void Awake()
    {
        _rtsShip = GetComponent<RtsShip>();
    }


    private void Update()
    {
        if (lookAt != null)
        {
            _rtsShip.LookTarget = lookAt.transform.position;
        }
    }
}