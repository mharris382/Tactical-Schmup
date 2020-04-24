using Sirenix.OdinInspector;
using UnityEngine;
[ExecuteAlways]
[RequireComponent(typeof(WeaponController))]
public class WeaponArcVisualizer : MonoBehaviour
{
    [Required]
    public LineRenderer lr;
    private WeaponController _weapon;
    [Range(1, 360)]
    public int smoothness = 12;
    private void Awake()
    {
        this._weapon = GetComponent<WeaponController>();
    }

    private void Update()
    {
        if (lr == null) return;
        var pnts = _weapon.GetArcPoints(smoothness);
        lr.positionCount = pnts.Count;
        lr.SetPositions(pnts.ToArray());
    }
}