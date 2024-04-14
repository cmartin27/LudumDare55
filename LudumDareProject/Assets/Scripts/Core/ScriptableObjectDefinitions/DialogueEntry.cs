using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "dialogEntry", menuName = "Scriptable Object/Dialogue Entries", order = 1)]
public class DialogueEntry : ScriptableObject
{
    [TextArea(1, 8)] public string questNotInitialized_;
    [TextArea(1, 8)] public string questInProgress_;
    [TextArea(1, 8)] public string questAfterSuccess_;
    [TextArea(1, 8)] public string questAfterFail_;
    [TextArea(1, 8)] public string questFinished_;
}
