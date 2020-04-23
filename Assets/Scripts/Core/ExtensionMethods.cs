using UnityEngine;

public static class ExtensionMethods
{
    public static Color WithAlpha(this Color color, float alpha)
    {
        var c = color;
        c.a = alpha;
        return c;
    }


    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        var comp = gameObject.GetComponent<T>();
        if (comp == null)
        {
            comp = gameObject.AddComponent<T>();
        }

        return comp;
    }


    public static Vector3 With(this Vector3 vec3, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? vec3.x, y ?? vec3.y, z ?? vec3.z);
    }
}