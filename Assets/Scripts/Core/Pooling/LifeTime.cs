#region

using UnityEngine;

#endregion

public class LifeTime : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 1f;

    private float _timer;

    public void SetLifeTime(float lifeTime)
    {
        _lifeTime = lifeTime;
        _timer = 0f;
    }

    private void OnDisable()
    {
        _timer = 0f;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _lifeTime)
            gameObject.SetActive(false);
    }
}