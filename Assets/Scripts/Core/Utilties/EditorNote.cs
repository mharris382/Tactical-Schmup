#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EditorNote : MonoBehaviour
{
    

    [BoxGroup("NOTE",false)]
    [HideLabel, TextArea(5, 15),GUIColor("guiColor")]
    public string note;
    
    
    [HorizontalGroup("h1",LabelWidth = 65)]
    [PropertyOrder(-2),GUIColor("referenceGuiColor"),InlineEditor(Expanded = false)]
    public UnityEngine.Object reference;
    Color guiColor = Color.Lerp(Color.red, Color.white, 0.65f);
    private Color referenceGuiColor => reference == null ? Color.grey : Color.Lerp(Color.red, Color.white, 0.65f);
    [HorizontalGroup("h1"),HideLabel]
    [PropertyOrder(-1),ShowIf("@reference!=null"), GUIColor("referenceGuiColor"),Multiline(2)]
    public string referenceNote;
}

#endif