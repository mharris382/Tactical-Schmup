#region

using UnityEngine;

#endregion

public abstract class AudioEventBase : ScriptableObject
{
    public abstract void Play(AudioSource source);
}