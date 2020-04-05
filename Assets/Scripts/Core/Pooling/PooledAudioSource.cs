#region

using UnityEngine;

#endregion

[RequireComponent(typeof(AudioSource))]
public class PooledAudioSource : PooledMonoBehaviour
{
    public AudioSource Source { get; private set; }

    private void Awake()
    {
        Source = GetComponent<AudioSource>();
    }
}