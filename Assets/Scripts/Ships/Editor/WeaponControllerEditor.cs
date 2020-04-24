using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(WeaponController)), CanEditMultipleObjects()]
public class WeaponControllerEditor : OdinEditor
{
    private float? queuedDistance;
    private float? queuedDot;

    private void OnSceneGUI()
    {
        var wep = target as WeaponController;
        if (wep == null)
        {
            Debug.LogError("Not a weapon!");
            return;
        }


        Transform transform = wep.transform;
        DrawHandles(new SerializedObject(target), wep, transform,
            dot => UpdateAllAngles(dot),
            distance => UpdateAllDistances(distance));
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        ClearQueuedChanges();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ClearQueuedChanges();
    }

    private static Dictionary<WeaponController, CircleCollider2D> sensorColliders =
        new Dictionary<WeaponController, CircleCollider2D>();

    public override void OnInspectorGUI()
    {
        var wep = target as WeaponController;
        var ship = wep.GetComponentInParent<IRtsShip>();
        if (ship != null)
        {
            ValidateWeaponCollisionLayers(ship);
        }

        CheckIfWeaponHasSensor(wep);


        if (queuedDistance.HasValue)
        {
            UpdateAllDistances(queuedDistance.Value);
            queuedDistance = null;
        }

        if (queuedDot.HasValue)
        {
            UpdateAllAngles(queuedDot.Value);
            queuedDot = null;
        }

        base.OnInspectorGUI();
    }

    private CircleCollider2D CheckIfWeaponHasSensor(WeaponController wep)
    {
        CircleCollider2D coll;
        if (!sensorColliders.TryGetValue(wep, out coll))
        {
            var autoSensor = wep.GetComponent<AutoFireSensor>();
            if (autoSensor == null) return null;
            
            var col = autoSensor.sensor.sensor.GetComponent<CircleCollider2D>();
            if (col == null) return null;

            sensorColliders.Add(wep, col);
        }

        return coll;
    }

    private static void ValidateWeaponCollisionLayers(IRtsShip ship)
    {
        bool shipInAllyLayer = ship.gameObject.layer == LayerMask.NameToLayer("Ally");
        bool shipInEnemyLayer = ship.gameObject.layer == LayerMask.NameToLayer("Enemy");
        var weaponParticles = ship.transform.GetComponentsInChildren<ParticleWeapon>();
        if (shipInAllyLayer || shipInEnemyLayer)
        {
            var selfLayer = shipInAllyLayer ? Layers.AllyLayer : Layers.EnemyLayer;
            var hasWeaponCollidingWithSelf = weaponParticles.FirstOrDefault(t =>
                (t.GetComponent<ParticleSystem>().collision.collidesWith |
                 selfLayer) !=
                0) != null;
            if (hasWeaponCollidingWithSelf && GUILayout.Button(
                $"Fix Weapon Particles which hit self"))
            {
                foreach (var particle in weaponParticles)
                {
                    var particleSystem = particle.GetComponent<ParticleSystem>();
                    var collisionModule = particleSystem.collision;
                    collisionModule.collidesWith &= ~selfLayer;
                }
            }
        }
    }

    private void ClearQueuedChanges()
    {
        queuedDistance = null;
        queuedDot = null;
    }

    private void UpdateAllDistances(float distance)
    {
        var targets = Selection.transforms.Select(t => t.GetComponent<WeaponController>());
        foreach (var target in targets.Select(t => new Tuple<SerializedObject, CircleCollider2D>(new SerializedObject(t), CheckIfWeaponHasSensor(t))))
        {
            var rangeProp = target.Item1.FindProperty("_maxRange");
            rangeProp.floatValue = distance;
            target.Item1.ApplyModifiedProperties();
            if (target.Item2 != null && target.Item2.radius < rangeProp.floatValue)
            {
                target.Item2.radius = rangeProp.floatValue + 1;
            }
        }
    }

    private void UpdateAllAngles(float distance)
    {
        var targets = Selection.transforms.Select(t => t.GetComponent<WeaponController>());
        foreach (var target in targets.Select(t => new SerializedObject(t)))
        {
            var coneProp = target.FindProperty("_coneOfFire");
            coneProp.floatValue = distance;
            target.ApplyModifiedProperties();
        }
    }

    private void DrawHandles(SerializedObject so, WeaponController wep, Transform transform,
        Action<float> onDotChanged = null, Action<float> onDistanceChanged = null)
    {
        var coneProp = so.FindProperty("_coneOfFire");
        var rangeProp = so.FindProperty("_maxRange");

        var range = DrawHandle(wep, rangeProp, coneProp, transform, out var line, out var nRotated, out var pos);

        var sliderHandlePos = pos + line;
        var handleSize = HandleUtility.GetHandleSize(pos) * 0.035f;

        //var ps = wep.weaponTransform?.GetComponent<ParticleSystem>();
        //ps.Simulate(ps.main.loop ? Time.realtimeSinceStartup : Time.realtimeSinceStartup % ps.main.duration);

        var snapRange = 0f;
        var snapAngle = 0f;
        if (Event.current.control)
        {
            snapRange = EditorSnapSettings.move.y;
            snapAngle = EditorSnapSettings.rotate;
        }

        var prevDist = Vector2.Distance(pos, sliderHandlePos);
        var newPos = Handles.Slider(sliderHandlePos, wep.CenterAngle.normalized, handleSize, Handles.DotHandleCap,
            snapRange);
        var newDist = Vector2.Distance(pos, newPos);
        if (Math.Abs(newDist - prevDist) > Mathf.Epsilon)
        {
            rangeProp.floatValue = newDist;
            if (!Event.current.shift)
                onDistanceChanged?.Invoke(newDist);
            so.ApplyModifiedProperties();
            return;
        }


        var rotation = Quaternion.LookRotation(nRotated, Vector3.forward);
        Handles.color = Color.clear;

        var newRotation = Handles.Disc(rotation, pos, Vector3.forward, Mathf.Max(range - (range / 10), 0.1f), false,
            snapAngle);

        if (newRotation != rotation)
        {
            Debug.Log("Rotation Changed");

            var center = Quaternion.LookRotation(wep.CenterAngle, Vector3.forward);
            var newAngle = Quaternion.Angle(center, newRotation);
            coneProp.floatValue = Quaternion.Dot(center, newRotation);
            so.ApplyModifiedProperties();
            if (!Event.current.shift)
                onDotChanged?.Invoke(coneProp.floatValue);
        }
        //Handles.color = Color.yellow;


        so.ApplyModifiedProperties();
    }

    private float DrawHandle(WeaponController wep, SerializedProperty rangeProp, SerializedProperty coneProp,
        Transform transform, out Vector3 line, out Vector3 nRotated, out Vector3 pos)
    {
        var color = GetHandleColor(wep).WithAlpha(0.75f);

        float range = rangeProp.floatValue;
        float dot = coneProp.floatValue;
        var angle = Mathf.Acos(dot) * Mathf.Rad2Deg; // ((dot - 1) / 2f) * 360f;
        line = wep.CenterAngle.normalized * range;
        var pRotated = Quaternion.AngleAxis(angle, Vector3.forward) * line;
        nRotated = Quaternion.AngleAxis(-angle, Vector3.forward) * line;
        pos = transform.position;


        Handles.color = color;
        Handles.DrawLine(pos, pos + pRotated);
        Handles.DrawLine(pos, pos + nRotated);
        Handles.DrawWireArc(pos, Vector3.forward, nRotated, angle * 2, range);
        var prev = color;
        Handles.color = color.WithAlpha(0.125f);
        Handles.DrawSolidArc(pos, Vector3.forward, nRotated, angle * 2, range);
        Handles.color = prev;
        return range;
    }

    private Color GetHandleColor(WeaponController wep)
    {
        DamageType dType = DamageType.none;
        Color color = Color.gray;
        if (wep.weaponTransform != null)
        {
            color = DamageTypeColors.GetColor(wep.weaponTransform.DamageType);
            // dType = wep.weaponTransform.DamageType;
            // try
            // {
            //     var tup = typeColors.First(t => t.Item1 == dType);
            //     color = tup.Item2;
            // }
            // catch (Exception e)
            // {
            //     Debug.LogError($"No color was found for damage type {dType}");
            //     color = Color.white;
            // }
        }

        return color;
    }


    private Tuple<DamageType, Color>[] typeColors = new[]
    {
        new Tuple<DamageType, Color>(DamageType.none, Color.white),
        new Tuple<DamageType, Color>(DamageType.emotional, Color.magenta),
        new Tuple<DamageType, Color>(DamageType.laser, Color.red),
        new Tuple<DamageType, Color>(DamageType.plasma, Color.green),
        new Tuple<DamageType, Color>(DamageType.kinetic, Color.cyan),
        new Tuple<DamageType, Color>(DamageType.explosive,
            new Color(246f / 235f, 131f / 235f, 28f / 235f, 1)), //rgb(242,131,28)
    };


    public static class Layers
    {
        public const int EnemyLayer = 1 << 16;
        public const int AllyLayer = 1 << 15;
    }
}