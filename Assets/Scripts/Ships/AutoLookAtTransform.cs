using UnityEngine;


public class AutoLookAtTransform : MonoBehaviour
{

    public Transform lookAt;
    private IRtsShip _rtsShip;

    private void Awake()
    {
        _rtsShip = GetComponent<IRtsShip>();
    }


    private void Update()
    {
        if (lookAt != null)
        {
            _rtsShip.LookTarget = lookAt.transform.position;
        }
    }
}