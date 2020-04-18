using UnityEngine;

public interface IRtsShip
{
    #region [MonoBehaviour Properties]

    GameObject gameObject { get; }
    Transform transform { get; }

    #endregion


    Vector3 LookTarget { set; }
    Vector3 MoveTarget { set; }
}