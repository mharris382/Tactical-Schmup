using System.Collections;
using UnityEngine;

public class OrbitMotion : MonoBehaviour
{
    public Transform orbitingObject;
    public Ellipse orbitPath;
    
    [Range(0f,1f)] public float orbitProgress = 0f;
    public float orbitPeriod = 3f;
    public bool orbitActive = true;


    void Start()
    {
        if (orbitingObject == null)
        {
            orbitActive = false;
            return;
        }
        else
        {
            SetOrbitingObjectPosition();
            StartCoroutine(AnimateOrbit());
        }
    }

    private void SetOrbitingObjectPosition()
    {
        Vector2 orbitPos = orbitPath.Evaluate(orbitProgress);
        orbitingObject.localPosition = new Vector3(orbitPos.x, 0f, orbitPos.y);
    }

    IEnumerator AnimateOrbit()
    {
        if (orbitPeriod < 0.5f)
        {
            orbitPeriod = 0.5f;
        }
        float orbitSpeed = 1f / orbitPeriod;
        while (orbitActive)
        {
            orbitProgress += Time.deltaTime * orbitSpeed;
            orbitProgress %= 1f;
            SetOrbitingObjectPosition();
            yield return null;
        }
    }
}
