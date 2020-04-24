using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(WeaponController))]
public class WeaponArcVisualizer : MonoBehaviour
{
    [Required]
    public LineRenderer lineRenderer;
    private WeaponController wep;
    [Range(10, 90)]
    public int smoothness = 24;
    private void Awake()
    {
        wep = GetComponent<WeaponController>();
    }
    [Button]
    private void Update()
    {
        if (lineRenderer == null) return;
        float radius = wep.MaxRange;
        var origin = transform.position;
        var center =  wep.CenterAngle;
        List<Vector3> points = new List<Vector3>();
        float totalAngle = wep.GetArcAngle() * 2;
        points.Add(center);
        var angle = Mathf.Acos(wep.ConeOfFire) * Mathf.Rad2Deg;
        var line = wep.CenterAngle.normalized;

        var pRotated = Quaternion.AngleAxis(-angle, Vector3.forward) * line;
        lineRenderer.transform.up = pRotated;
        float ang = Vector2.SignedAngle(Vector2.right, wep.CenterAngle);
        for (int i = 0; i <= smoothness; i++)
        {
            float x = radius * Mathf.Cos(ang * Mathf.Deg2Rad);
            float y = radius * Mathf.Sin(ang * Mathf.Deg2Rad);

            points.Add(new Vector2(x, y));
            ang += (float)totalAngle / smoothness;
        }
        points.Add(center);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }
}
