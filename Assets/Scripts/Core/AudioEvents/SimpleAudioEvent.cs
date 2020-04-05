#region

using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

#endregion

[CreateAssetMenu(menuName ="Audio Events/Simple")]
public class SimpleAudioEvent : AudioEventBase
{
    [ListDrawerSettings(Expanded =true),    SerializeField]
    private AudioClip[] clips = new AudioClip[0];

    [SerializeField,  MinMaxRange(0f, 1000f)]
    private RangedFloat distance = new RangedFloat(1, 1000);

    [SerializeField]
    private AudioMixerGroup mixer;

    [MinMaxRange(0, 2f), SerializeField]
    private RangedFloat pitch = new RangedFloat(0.9f, 1);


    [SerializeField]
    private RangedFloat volume = new RangedFloat(0.5f, 1);

    public override void Play(AudioSource source)
    {
        source.outputAudioMixerGroup = mixer;

        int clipIndex = Random.Range(0, clips.Length);
        source.clip = clips[clipIndex];

        source.pitch = Random.Range(pitch.min, pitch.max);
        source.volume = Random.Range(volume.min, volume.max);

        source.minDistance = distance.min;
        source.maxDistance = distance.max;

        source.Play();
    }
}