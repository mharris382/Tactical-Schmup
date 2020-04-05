#region

using System.Collections;
using UnityEngine;

#endregion

public class AudioManager : MonoBehaviour
{
    private static AudioManager _audioManager;
    private PooledAudioSource _audioSourcePrefab;

    private static AudioManager Instance
    {
        get
        {
            if (_audioManager == null)
            {
                _audioManager = CreateAudioManager();
            }
            return _audioManager;
        }
    }

    private static AudioManager CreateAudioManager()
    {
        var amGo = new GameObject("AudioManager", typeof(AudioManager));
        var asGO = new GameObject("Audio Source Prefab", typeof(PooledAudioSource), typeof(AudioSource));
        var am = amGo.GetComponent<AudioManager>();
        asGO.transform.parent = amGo.transform;
        asGO.transform.localPosition = Vector3.zero;
        am._audioSourcePrefab = asGO.GetComponent<PooledAudioSource>();
        DontDestroyOnLoad(am.gameObject);
        return am;
    }

    public static void PlayAudioEvent(AudioEventBase audioEvent, Vector2 position)
    {
        Instance.INTERNAL_PlayAudioEvent(audioEvent, position);
    }

    public static void PlayAudioEvent(AudioEventBase audioEvent, Transform parent)
    {
        Instance.INTERNAL_PlayAudioEvent(audioEvent, parent);
    }

    private void INTERNAL_PlayAudioEvent(AudioEventBase audioEvent, Vector2 position)
    {
        var pooledSource = _audioSourcePrefab.Get<PooledAudioSource>();
        pooledSource.transform.position = position;
        
        Play(audioEvent, pooledSource);
    }

    private void INTERNAL_PlayAudioEvent(AudioEventBase audioEvent, Transform parent)
    {
        var pooledSource = _audioSourcePrefab.Get<PooledAudioSource>();
        pooledSource.transform.parent = parent;
        pooledSource.transform.localPosition = Vector3.zero;
        
        Play(audioEvent, pooledSource);

    }

    private void Play(AudioEventBase audioEvent, PooledAudioSource pooledSource)
    {
        pooledSource.enabled = true;
        audioEvent.Play(pooledSource.Source);
        StartCoroutine(WaitForAudioSourceToFinishPlaying(pooledSource));
    }


    IEnumerator WaitForAudioSourceToFinishPlaying(PooledAudioSource pooledSource)
    {
        pooledSource.Source.enabled = true;
        yield return null;
        
        yield return new WaitForSeconds(pooledSource.Source.clip.length);
        
        pooledSource.gameObject.SetActive(false);
    }
}