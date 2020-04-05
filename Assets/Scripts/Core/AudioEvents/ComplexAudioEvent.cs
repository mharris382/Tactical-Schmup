#region

using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#endregion

[CreateAssetMenu(menuName ="Audio Events/Complex")]
public class ComplexAudioEvent : AudioEventBase
{
    [ListDrawerSettings(Expanded = true, CustomAddFunction = "CreateNewAudioEvent")]
    [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
    [SerializeField]
    private SimpleAudioEvent[] _audioEvents;


#if UNITY_EDITOR
    SimpleAudioEvent CreateNewAudioEvent()
    {
        var ae = CreateInstance<SimpleAudioEvent>();
        ae.name = $"{name} - {_audioEvents.Length}";
        AssetDatabase.AddObjectToAsset(ae, this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return ae;
    }
#endif

    public override void Play(AudioSource source)
    {
        int cnt = _audioEvents.Length;
        var index = Random.Range(0, cnt);
        _audioEvents[index].Play(source);
    }
}