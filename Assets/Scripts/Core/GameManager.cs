using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public void Quit()
        {
            Application.Quit();
        }
    }
}



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
}